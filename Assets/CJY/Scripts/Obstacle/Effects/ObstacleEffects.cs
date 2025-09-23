using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEffects : MonoBehaviour
{
    [Header("얼어붙은 블록 (회전 금지)")]
    [SerializeField] private float freezeChance = 0.9f;

    [Header("잔설 (입력 지연)")]
    [SerializeField] private float inputDelayTime = 0.5f;

    [Header("강풍 (블록 밀림)")]
    [SerializeField] private float windDuration = 3f;
    [SerializeField] private float windInterval = 0.3f;
    [SerializeField] private Vector2 windWaitIntervalRange = new Vector2(2f, 5f);

    [Header("황사 (시야 차단)")]
    [SerializeField] private Vector2 dustSpawnIntervalRange = new Vector2(0.5f, 3f);
    [SerializeField] private Vector2 dustLifetimeRange = new Vector2(1f, 2f);

    [Header("낙뢰 (조작 일시 정지)")]
    [SerializeField] private Vector2 lightningIntervalRange = new Vector2(2f, 5f);
    [SerializeField] private float lightningStopTime = 1.0f;

    [Header("장마 (낙하 속도 저하)")]
    [SerializeField] private float rainDropSpeedMultiplier = 0.5f; // 기본 속도의 50%

    [Header("건기 (블록 파괴 확률)")]
    [SerializeField] private float BreakBlockChance = 0.5f;

    // 1월 : 회전 금지 (얼어붙은 블록)
    public void TryFreezeBlock(ObstacleGameState state)
    {
        Debug.Log("얼어붙은 블록(회전금지) 효과 실행");

        TetriminoBlock block = state.LockedBlock; // 설치 블록 기준
        if (block == null) return;

        if (UnityEngine.Random.value <= freezeChance)
        {
            Debug.Log("회전금지 효과");
            state.InputBlocker.blockRotation = true;
            state.VisualPlayer?.VisualFreezeBlock(block);
        }
    }


    // 2월 : 블록 밀림 (강풍)
    public void PushBlockRandomly(ObstacleGameState state)
    {
        Debug.Log("강풍 (블록 밀림) 효과 실행");
        if (state == null || state.SpawnedBlock == null)  // ← SpawnedBlock 사용
        {
            //Debug.LogWarning("[ObstacleEffects] PushBlockRandomly 호출 시 state 또는 SpawnedBlock이 null입니다.");
            return;
        }

        // 코루틴 한 번만 시작하면 WindPushLoop가 반복 관리
        state.StartManagedCoroutine?.Invoke(WindPushLoop(state));
    }

    private IEnumerator WindPushLoop(ObstacleGameState state)
    {
        if (state.SpawnedBlock == null) yield break;  // ← SpawnedBlock 사용

        // 바람 방향 설정
        Vector3 dir = UnityEngine.Random.value > 0.5f ? Vector3.left : Vector3.right;
        Debug.Log($"[Wind] 바람 시작! 방향={dir}, 지속={windDuration}s");

        float timer = 0f;
        var obj = state.VisualPlayer?.PlayStrongWindEffect();
        if (obj != null)
        {
            state.RegisterObject?.Invoke(obj);
            Destroy(obj, windDuration);
        }

        // 바람 지속 시간 동안 블록 이동
        while (timer < windDuration && state.SpawnedBlock != null)  // ← SpawnedBlock 사용
        {
            state.TetrisController?.MoveBlockByWind(dir);
            yield return new WaitForSeconds(windInterval);
            timer += windInterval;
        }

        Debug.Log("[Wind] 바람 종료");
    }



    // 3월 : 입력 지연 (잔설)
    public void InputDelay(ObstacleGameState state)
    {
        Debug.Log("잔설 입력 지연 효과 실행");

        var blocker = state.InputBlocker;
        if (blocker == null) return;

        blocker.SetInputProcessDelay(inputDelayTime);

        var obj = state.VisualPlayer?.SnowfallEffect();
        if (obj != null) state.RegisterObject?.Invoke(obj);
    }

    // 4~5월 : 시야 차단 (황사)
    public void ApplyDustStormEffect(ObstacleGameState state)
    {
        Debug.Log("황사 (시야 차단) 효과 실행");

        var blocker = state.InputBlocker;
        if (blocker == null) return;

        blocker.StartCoroutine(DustStormLoop(state.VisualPlayer));
    }

    private IEnumerator DustStormLoop(EffectVisualPlayer visualPlayer)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(dustSpawnIntervalRange.x, dustSpawnIntervalRange.y));

            var instance = visualPlayer?.DustStormEffect();
            if (instance != null)
                Destroy(instance, Random.Range(dustLifetimeRange.x, dustLifetimeRange.y));
        }
    }

    // 6월 : 조작 일시 정지 (낙뢰)
    public void DisableControlTemporary(ObstacleGameState state)
    {
        Debug.Log("낙뢰 (조작 일시 정시) 효과 실행");

        var blocker = state.InputBlocker;
        if (blocker == null) return;

        state.StartManagedCoroutine?.Invoke(LightningStormLoop(blocker, state.VisualPlayer));
    }

    private IEnumerator LightningStormLoop(InputBlocker blocker, EffectVisualPlayer visualPlayer)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(lightningIntervalRange.x, lightningIntervalRange.y));

            //Debug.Log("(코루틴)조작 일시 정지");
            blocker.isInputBlocked = true;

            var obj = visualPlayer?.PlayLightningEffect();
            if (obj != null)
                Destroy(obj, lightningStopTime); // 번개 지속 시간 후 삭제
            

            yield return new WaitForSeconds(lightningStopTime); // 인스턴스 필드 접근

            blocker.isInputBlocked = false;
        }
    }

    // 7월 : 스페이스 불가능
    public void DisableSpace(ObstacleGameState state)
    {
        Debug.Log("장마 (스페이스불가능) 효과 실행");

        var blocker = state.InputBlocker;
        if (blocker == null) return;

        blocker.blockHardDrop = true;

        var obj = state.VisualPlayer?.PlayRainEffect();
        if (obj != null) state.RegisterObject?.Invoke(obj);
    }

    // 7월 : 낙하속도 저하 (장마)
    public void SlowDropSpeed(ObstacleGameState state)
    {
        Debug.Log("장마 (낙하속도저하) 효과 실행");
        if (state == null) return;

        state.CurrentDropSpeed = rainDropSpeedMultiplier;
    }

    // 8월 : 다음 블록 UI 비활성화 (폭염)
    public void HideNextBlockUI(ObstacleGameState state)
    {
        Debug.Log("폭염 (다음UI 비활성화) 효과 실행");

        var obj = state.VisualPlayer?.PlayOverheatWarning("건축 기계 과열!");
        if (obj != null) state.RegisterObject?.Invoke(obj);
    }

    // 10월 : 블록 파괴 확률 (건기)
    public void BreakBlockOnPlace(ObstacleGameState state)
    {
        Debug.Log("건기 (블록파괴) 효과 실행");

        var block = state.LockedBlock;  // ← 설치된 블록 참조
        if (block == null) return;

        var children = block.GetComponentsInChildren<TetriminoBlockChild>();
        if (children.Length == 0) return;

        bool anyDeleted = false; // 칸이 하나라도 삭제됐는지 체크

        foreach (var child in children)
        {
            if (child != null && UnityEngine.Random.value <= BreakBlockChance) // 50% 확률
            {
                child.DeletBlock();
                anyDeleted = true;
            }
        }

        // 칸이 하나라도 삭제된 경우에만 이펙트 실행
        if (anyDeleted)
        {
            state.VisualPlayer.PlayBlockCrumbleEffect(block.transform.position);
        }

        // 혹시 블록이 다 부서졌으면 껍데기도 삭제
        block.CleanupIfEmpty();
    }

    // 11월 : 정보 UI 스모그 효과 (스모그)
    public void ApplySmogOverlay(ObstacleGameState state)
    {
        Debug.Log("스모그 (테두리 시야방해) 효과 실행");

        var obj = state.VisualPlayer?.PlaySmogEffect();
        if (obj != null) state.RegisterObject?.Invoke(obj);
    }
}
