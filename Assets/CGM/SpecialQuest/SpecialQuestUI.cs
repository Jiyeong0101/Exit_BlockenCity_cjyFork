using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialQuestUI : MonoBehaviour
{
    public static SpecialQuestUI CurrentUI;

    public Text questNameText;
    public Text descriptionText;
    public int rewardText;
    public Text progressText;
    public Slider time;

    private float currentTime;
    private bool timerRunning = false;

    private int questID;

    private SpecialQuestData quest;

    // UI에 퀘스트 데이터 바인딩
    public void SetQuest(SpecialQuestData quest)
    {
        CurrentUI = this;
        this.quest = quest;
        questID = quest.branchID;
        questNameText.text = quest.questName;
        descriptionText.text = quest.description;
        rewardText = quest.reward;

        //시간
        time.maxValue = quest.timeValue;
        currentTime = quest.timeValue;
        time.value = quest.timeValue;

        timerRunning = quest.HasTimeLimit();
    }

    private void Update()
    {
        if (!timerRunning) return;

        if (currentTime > 0)
        { 
            currentTime -= Time.deltaTime;
            time.value = currentTime;
        }

        else
        {
            timerRunning = false;
            time.value = 0;

            // 시간 종료 알림
            Debug.Log("스페셜퀘스트 시간 종료됨!");
            SpecialQuestManager.Instance.OnTimeExpired(quest);
        }
    }
    public void UpdateTimer(float value)
    {
        if (time == null) return;
        time.value = value;
    }
}