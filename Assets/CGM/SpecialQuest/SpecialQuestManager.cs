using System.Collections;
using System.Collections.Generic;
using TetrisGame;
using UnityEngine;

public class SpecialQuestManager : MonoBehaviour
{
    public static SpecialQuestManager Instance;

    public Transform questUIParent;          // 퀘스트 UI를 붙일 부모 오브젝트.
    public GameObject questUIPrefab;         // 퀘스트를 표시할 프리팹 (NewQuestUI 스크립트 포함)

    private SpecialQuestData currentQuest;

    // 현재 진행 상황
    private int breakCount = 0;     // BlockBreak, BlockNoBreak
    private int noBreakCount = 0;   // BlockNoBreak

    private float keepHeightTimer = 0f; // KeepHeight용 내부 타이머
    private int heightKeepBaseHeight;

    private void Awake()
    {
        Instance = this;
    }

    public void AddQuest(SpecialQuestData quest)
    {
        // UI 생성
        GameObject questUIObj = Instantiate(questUIPrefab, questUIParent);
        SpecialQuestUI questUI = questUIObj.GetComponent<SpecialQuestUI>();
        questUI.SetQuest(quest);

        activeQuests.Add(quest);

        StartQuest(quest);

        Debug.Log($"퀘스트 생성됨 및 시작됨: {quest.questName}");
    }


    private List<SpecialQuestData> activeQuests = new();

    public void StartQuest(SpecialQuestData quest)
    {
        currentQuest = quest;
        currentQuest.IsCompleted = false;

        breakCount = 0;
        noBreakCount = 0;
        keepHeightTimer = quest.keepDuration;

        if (quest.questType == SpecialQuestType.HeightKeep)
        {
            heightKeepBaseHeight = TetrisManager.Instance.tower.GetCurrentHeight();
            Debug.Log($"[HeightKeep] 기준 높이 설정: {heightKeepBaseHeight + 1}");
        }

        if (quest.questType == SpecialQuestType.InputRestriction)
        {
            var inputs = quest.restrictedInput.Split(',');
            InputManager.Instance.SetRestrictedInputs(inputs);

            Debug.Log($"[InputRestriction] 제한 입력: {quest.restrictedInput}");
        }
        Debug.Log($"[SpecialQuest] 퀘스트 시작: {quest.questName}");
    }

    private void CompleteQuest()
    {
        if (currentQuest.IsCompleted) return;

        if (currentQuest == null) return;

        currentQuest.IsCompleted = true;

        if (currentQuest.questType == SpecialQuestType.InputRestriction)
        {
            InputManager.Instance.ClearRestrictedInputs();
            Debug.Log("[InputRestriction] 입력 제한 해제");
        }
        Debug.Log($"[SpecialQuest] 완료: {currentQuest.questName}");

        if (SpecialQuestUI.CurrentUI != null)
        {
            SpecialQuestUI.CurrentUI.ShowCompleted();
        }

        // TODO: UI 갱신
        // TODO: 보상 지급
    }

    private void FailQuest()
    {
        if (currentQuest == null) return;
        if (currentQuest.IsCompleted) return;

        currentQuest.IsCompleted = true;

        Debug.Log($"[SpecialQuest] 실패: {currentQuest.questName}");

        if (SpecialQuestUI.CurrentUI != null)
        {
            Destroy(SpecialQuestUI.CurrentUI.gameObject);
            SpecialQuestUI.CurrentUI = null;
        }

        // TODO: UI 갱신
    }

    //TODO각 조건별로 구성해야함 아직삭제는없음
    public void OnTimeExpired(SpecialQuestData quest)
    {
        if (currentQuest == null || quest != currentQuest)
            return;

        if (currentQuest.IsCompleted)
            return;

        Debug.Log("[SpecialQuest] 시간 종료");

        switch (quest.questType)
        {
            case SpecialQuestType.HeightKeep:
                // ❗ 중간에 실패 안 했으면 성공
                CompleteQuest();
                break;

            case SpecialQuestType.InputRestriction:
                CompleteQuest();
                break;

            // 나머지는 기존 로직 유지
            default:
                FailQuest();
                break;
        }
    }


    // 블럭 파괴 이벤트
    public void OnBlockDestroyed(BlockType destroyedType)
    {
        if (currentQuest == null) return;

        // BlockBreak 퀘스트만 처리
        if (currentQuest.questType != SpecialQuestType.BlockBreak)
            return;

        // 지정된 블럭이 아니면 무시
        if (destroyedType != currentQuest.blockType)
            return;

        breakCount++;

        // UI 갱신
        if (SpecialQuestUI.CurrentUI != null)
        {
            SpecialQuestUI.CurrentUI.UpdateBlockBreak(
                breakCount,
                currentQuest.targetCount
            );
        }

        // 즉시 완료 조건
        if (breakCount >= currentQuest.targetCount)
        {
            CompleteQuest();
        }
    }


    // 블럭 드랍 이벤트
    public void OnBlockDropped()
    {
        return;
    }

    // 현재 높이 변경 이벤트
    public void OnHeightChanged(int currentHeight)
    {
        if (currentQuest == null || currentQuest.IsCompleted)
            return;

        SpecialQuestUI.CurrentUI?.UpdateHeightProgress(currentHeight, currentQuest.targetHeight);

        switch (currentQuest.questType)
        {
            case SpecialQuestType.HeightKeep:
                // ❗ 기준 높이와 다르면 즉시 실패
                if (currentHeight != heightKeepBaseHeight)
                {
                    Debug.Log(
                        $"[HeightKeep] 높이 변경 감지! 기준:{heightKeepBaseHeight + 1}, 현재:{currentHeight + 1}"
                    );
                    FailQuest();
                }
                break;

            case SpecialQuestType.HeightLimit:
                if (currentHeight + 1 > currentQuest.targetHeight)
                    FailQuest();
                break;

            case SpecialQuestType.HeightAchievement:
                if (currentHeight + 1 >= currentQuest.targetHeight)
                    CompleteQuest();
                break;
        }

    }


    public void OnBlockPlaced(Vector3Int pos, BlockType type)
    {
        if (currentQuest == null) return;
        if (currentQuest.questType != SpecialQuestType.HeightSpecialBlock)
            return;

        int currentHeight = TetrisManager.Instance.tower.GetCurrentHeight();

        bool installed =
            pos.y + 1 == currentQuest.targetHeight &&
            type == currentQuest.blockType;

        SpecialQuestUI.CurrentUI?.UpdateHeightSpecialBlock(
            currentHeight,
            currentQuest.targetHeight,
            currentQuest.blockType,
            installed
        );

        if (installed)
            CompleteQuest();
    }

}