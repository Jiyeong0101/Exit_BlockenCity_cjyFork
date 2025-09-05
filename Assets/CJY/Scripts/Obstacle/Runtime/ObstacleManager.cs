using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 방해물 조건 평가 및 효과 적용을 담당
/// </summary>
public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private ObstacleEffects effects;
    [SerializeField] private EffectVisualPlayer visualPlayer;
    [SerializeField] private InputBlocker inputBlocker;
    [SerializeField] private TetrisController tetrisController;
    [SerializeField] private TetrisSpawner tetrisspawner;

    [Header("테스트 모드")]
    [SerializeField] private bool useInspectorObstacle = false;
    [SerializeField] private MonthlyObstacleTable.Entry selectedObstacleEntry; // 테스트 전용

    private List<ObstacleRuntime> obstacles = new();
    [SerializeField] private bool obstacleSystemEnabled = true;

    private List<Coroutine> runningCoroutines = new();
    private List<GameObject> spawnedObjects = new();
    private ObstacleEffects effectsInstance;

    void Start()
    {
        Initialize();
        EvaluateCurrentObstacle();

        // 블록 스포너 이벤트 구독
        tetrisspawner.OnBlockSpawned += HandleBlockSpawned;
        // 블록 고정 이벤트 구독
        TetriminoBlock.OnAnyBlockLocked += HandleBlockLocked;
    }

    private void OnDestroy()
    {
        tetrisspawner.OnBlockSpawned -= HandleBlockSpawned;
        TetriminoBlock.OnAnyBlockLocked -= HandleBlockLocked;
    }

    private void Initialize()
    {
        if (effects == null)
        {
            Debug.LogError("ObstacleEffects가 Inspector에서 연결되지 않았습니다!");
            return;
        }

        var factory = new ObstacleFactory(effects);
        obstacles = factory.CreateAllObstacles();
        effectsInstance = effects;
    }

    /// <summary>
    /// 현재 활성화된 방해물 엔트리 가져오기
    /// </summary>
    private MonthlyObstacleTable.Entry GetActiveObstacleEntry()
    {
        if (useInspectorObstacle && selectedObstacleEntry != null)
        {
            //Debug.Log($"[ObstacleManager] 테스트 모드 사용 중 → {selectedObstacleEntry.type}");
            return selectedObstacleEntry;
        }

        var entry = GameObstacleSystem.Instance.GetSelectedObstacle();
        //Debug.Log($"[ObstacleManager] 로비 확정 방해물 사용 → {entry?.type.ToString() ?? "없음"}");
        return entry;
    }

    private void HandleBlockSpawned(TetriminoBlock block)
    {
        Debug.Log($"[HandleBlockSpawned] 호출, block={block}");
        if (!obstacleSystemEnabled || block == null) return;

        var activeObstacle = GetActiveObstacleEntry();
        if (activeObstacle == null) return;

        var state = new ObstacleGameState(
            activeObstacle.type,
            visualPlayer,
            inputBlocker,
            tetrisController
        )
        {
            SpawnedBlock = block,
            StartManagedCoroutine = routine =>
            {
                var co = StartCoroutine(routine);
                runningCoroutines.Add(co);
                return co;
            },
            RegisterObject = o => spawnedObjects.Add(o)
        };

        foreach (var obstacle in obstacles)
        {
            if (obstacle.AreConditionsMet(state))
                obstacle.ExecuteSpawnEffects(state);
        }
    }

    private void HandleBlockLocked(TetriminoBlock block)
    {
        if (!obstacleSystemEnabled) return;

        var activeObstacle = GetActiveObstacleEntry();
        if (activeObstacle == null) return;

        var state = new ObstacleGameState(
            activeObstacle.type,
            visualPlayer,
            inputBlocker,
            tetrisController
        )
        {
            LockedBlock = block,
            StartManagedCoroutine = routine =>
            {
                var co = StartCoroutine(routine);
                runningCoroutines.Add(co);
                return co;
            },
            RegisterObject = o => spawnedObjects.Add(o)
        };

        foreach (var obstacle in obstacles)
        {
            if (obstacle.AreConditionsMet(state))
                obstacle.ExecuteLockEffects(state);
        }
    }

    private void EvaluateCurrentObstacle()
    {
        if (!obstacleSystemEnabled) return;

        var activeObstacle = GetActiveObstacleEntry();
        if (activeObstacle == null) return;

        var state = new ObstacleGameState(
            activeObstacle.type,
            visualPlayer,
            inputBlocker,
            tetrisController
        )
        {
            StartManagedCoroutine = routine =>
            {
                var co = StartCoroutine(routine);
                runningCoroutines.Add(co);
                return co;
            },
            RegisterObject = o => spawnedObjects.Add(o)
        };

        foreach (var obstacle in obstacles)
        {
            if (obstacle.AreConditionsMet(state))
                obstacle.ExecuteEffects(state);
        }

        Debug.Log($"[ObstacleManager] {activeObstacle.type} 방해물 평가 실행");
    }

    private void StopAllObstacleEffects()
    {
        foreach (var c in runningCoroutines)
            if (c != null) StopCoroutine(c);
        runningCoroutines.Clear();

        foreach (var obj in spawnedObjects)
            if (obj != null) Destroy(obj);
        spawnedObjects.Clear();

        inputBlocker.ResetAll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            obstacleSystemEnabled = !obstacleSystemEnabled;
            if (!obstacleSystemEnabled)
            {
                Debug.Log("방해물 중지");
                StopAllObstacleEffects();
            }
            else
            {
                EvaluateCurrentObstacle();
            }
        }
    }
}
