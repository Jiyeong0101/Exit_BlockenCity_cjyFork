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

        Debug.Log($"[SpecialQuest] 퀘스트 시작: {quest.questName}");
    }

    private void CompleteQuest()
    {
        currentQuest.IsCompleted = true;

        Debug.Log($"[SpecialQuest] 완료: {currentQuest.questName}");

        // TODO: UI 갱신
        // TODO: 보상 지급
    }

    private void FailQuest()
    {
        currentQuest.IsCompleted = false;

        Debug.Log($"[SpecialQuest] 실패: {currentQuest.questName}");

        // TODO: UI 갱신
    }

    //TODO각 조건별로 구성해야함 아직삭제는없음
    public void OnTimeExpired(SpecialQuestData quest)
    {
        if (currentQuest == null || quest != currentQuest)
            return;

        Debug.Log("[SpecialQuest] 시간 소진. 조건 검사 중...");

        // 시간 종료 → 타입별 조건 검사
        switch (quest.questType)
        {
            case SpecialQuestType.BlockBreak:
                if (breakCount >= quest.targetCount)
                    CompleteQuest();
                else
                    FailQuest();
                break;

            case SpecialQuestType.BlockNoBreak:
                if (noBreakCount <= quest.targetCount)
                    CompleteQuest();
                else
                    FailQuest();
                break;

            case SpecialQuestType.KeepHeight:
                if (keepHeightTimer <= 0)
                    CompleteQuest();
                else
                    FailQuest();
                break;

            case SpecialQuestType.ReachHeight:
                // 시간 종료 시점에 높이가 목표 도달했는지 체크하는 방식
                FailQuest();
                break;

            default:
                FailQuest();
                break;
        }
    }

    // 블럭 파괴 이벤트
    public void OnBlockDestroyed(BlockType type)
    {
        if (currentQuest == null) return;

        if (currentQuest.questType == SpecialQuestType.BlockBreak)
        {
            breakCount++;
            if (breakCount >= currentQuest.targetCount &&
                !currentQuest.HasTimeLimit())
            {
                CompleteQuest();
            }
        }

        if (currentQuest.questType == SpecialQuestType.BlockNoBreak)
        {
            noBreakCount++;
            if (noBreakCount > currentQuest.targetCount)
            {
                FailQuest();
            }
        }
    }

    // 블럭 드랍 이벤트
    public void OnBlockDropped()
    {
        if (currentQuest == null) return;
    }

    // 현재 높이 변경 이벤트
    public void OnHeightChanged(int currentHeight)
    {
        if (currentQuest == null) return;

        // KeepHeight: 특정 시간 동안 높이를 유지해야 함
        if (currentQuest.questType == SpecialQuestType.KeepHeight)
        {
            if (currentHeight == currentQuest.targetHeight)
            {
                keepHeightTimer -= Time.deltaTime;
                if (keepHeightTimer <= 0 && !currentQuest.HasTimeLimit())
                    CompleteQuest();
            }
            else
            {
                keepHeightTimer = currentQuest.keepDuration;
            }
        }

        // 목표 도달 시 즉시 성공
        if (currentQuest.questType == SpecialQuestType.ReachHeight)
        {
            if (currentHeight >= currentQuest.targetHeight)
            {
                CompleteQuest();
            }
        }
    }
}