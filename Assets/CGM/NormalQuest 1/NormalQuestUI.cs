using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalQuestUI : MonoBehaviour
{
    public Text questNameText;
    public Text descriptionText;
    public int rewardText;
    public Text progressText;

    private int questID;

    // UI에 퀘스트 데이터 바인딩
    public void SetQuest(NormalQuest quest)
    {
        questID = quest.questID;
        questNameText.text = quest.questName;
        descriptionText.text = quest.description;
        rewardText = quest.reward;
        progressText.text = $"{quest.currentCount}/{quest.targetCount}";
    }

    // 완료 버튼 클릭 시 호출
    public void OnCompleteButton()
    {
        QuestManager.Instance.CompleteQuest(questID);
        Destroy(gameObject); // UI 제거
    }
}