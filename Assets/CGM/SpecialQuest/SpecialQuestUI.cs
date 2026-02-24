using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TetrisGame;
using TMPro;

[System.Serializable]
public class BackgroundSliderColor
{
    public Sprite background;
    public Color sliderColor;
}

public class SpecialQuestUI : MonoBehaviour
{
    private ContentSizeFitter sizeFitter;
    private CanvasGroup canvasGroup;

    public static SpecialQuestUI CurrentUI;

    [Header("공통UI")]
    public TMP_Text questNameText;
    public TMP_Text descriptionText;
    public int rewardText;
    public TMP_Text progressText;
    public Slider time;
    [Header("배경")]
    public Image backgroundImage;
    [Header("슬라이더 색상 설정")]
    public Image sliderFillImage;   // Slider → Fill 영역 Image

    [Header("퀘스트별UI")]
    [Header("블럭파괴")]
    public GameObject blockBreakPanel;
    public TMP_Text blockBreakProgress;

    [Header("블럭파괴금지")]
    public GameObject blockNoBreakPanel;
    public TMP_Text blockNoBreakProgress;

    [Header("높이 제한")]
    public GameObject HeightLimitPanel;
    public TMP_Text HeightLimitProgress;

    [Header("높이 달성")]
    public GameObject HeightAchievementPanel;
    public TMP_Text HeightAchievementProgress;

    [Header("높이 유지")]
    public GameObject HeightKeepPanel;
    public TMP_Text HeightKeepProgress;

    [Header("높이 높이 블럭 설치")]
    public GameObject HeightSpecialBlockPanel;
    public TMP_Text HeightSpecialBlockProgress;


    [Header("입력제한")]
    public GameObject InputRestrictionPanel;
    public TMP_Text InputRestrictionProgress;

    [Header("시야 방해")]
    public GameObject ViewObstructionPanel;
    public TMP_Text ViewObstructionProgress;

    private int questID;

    private SpecialQuestData quest;
    [SerializeField] private Sprite defaultBackground;

    // UI에 퀘스트 데이터 바인딩
    public void SetQuest(SpecialQuestData quest)
    {
        CurrentUI = this;
        this.quest = quest;
        questID = quest.branchID;
        questNameText.text = quest.questName;
        descriptionText.text = quest.description;
        rewardText = quest.reward;

        if (backgroundImage != null)
        {
            backgroundImage.sprite =
                quest.background != null ? quest.background : defaultBackground;
            backgroundImage.enabled = true;
        }
        sliderFillImage.color = quest.sliderColor;
        
        //시간
        if (quest.timeType == TimeType.Seconds)
        {
            time.maxValue = quest.timeValue;
            time.value = quest.timeValue;
        }
        else if (quest.timeType == TimeType.BlockDropCount)
        {
            time.maxValue = quest.timeValue;
            time.value = quest.timeValue;

        }

        // 모든 패널 OFF
        blockBreakPanel.SetActive(false);
        blockNoBreakPanel.SetActive(false);

        HeightLimitPanel.SetActive(false);
        HeightAchievementPanel.SetActive(false);
        HeightKeepPanel.SetActive(false);
        HeightSpecialBlockPanel.SetActive(false);

        InputRestrictionPanel.SetActive(false);
        ViewObstructionPanel.SetActive(false);
        // 타입별 UI 설정
        switch (quest.questType)
        {
            case SpecialQuestType.BlockBreak:
                blockBreakPanel.SetActive(true);
                blockBreakProgress.text = $"0 / {quest.targetCount}";
                descriptionText.text =
                    $"{quest.blockType} 블럭 {quest.targetCount}개 파괴";
                break;

            case SpecialQuestType.BlockNoBreak:
                blockNoBreakPanel.SetActive(true);
                blockNoBreakProgress.text = $"{quest.blockType} 블럭 파괴시 실패";
                break;

            case SpecialQuestType.HeightLimit:
                HeightLimitPanel.SetActive(true);
                HeightLimitProgress.text = $"0 / {quest.targetHeight}";
                break;

            case SpecialQuestType.HeightAchievement:
                HeightAchievementPanel.SetActive(true);
                HeightAchievementProgress.text = $"0 / {quest.targetHeight}";
                break;

            case SpecialQuestType.HeightKeep:
                HeightKeepPanel.SetActive(true);
                HeightKeepProgress.text = $"기준 높이 유지1";
                break;

            case SpecialQuestType.HeightSpecialBlock:
                HeightSpecialBlockPanel.SetActive(true);
                HeightSpecialBlockProgress.text =
                    $"현재 높이 : 0\n설치 블럭 : {quest.blockType} X";
                break;

            case SpecialQuestType.InputRestriction:
                InputRestrictionPanel.SetActive(true);
                InputRestrictionProgress.text = $"{quest.restrictedInput} 입력 제한";
                break;

            case SpecialQuestType.ViewObstruction:
                ViewObstructionPanel.SetActive(true);
                ViewObstructionProgress.text = $"{quest.targetHeight} 시야 방해";
                break;
        }
        RefreshLayoutAndShow();
    }
    private void Awake()
    {
        sizeFitter = GetComponent<ContentSizeFitter>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // 처음엔 안 보이게
        canvasGroup.alpha = 0f;
    }

    public void RefreshLayoutAndShow()
    {
        StartCoroutine(RefreshAndShowCoroutine());
    }
    private IEnumerator RefreshAndShowCoroutine()
    {
        yield return new WaitForEndOfFrame();

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);

        canvasGroup.alpha = 1f;
    }

    public void UpdateTimer(float value)
    {
        if (time == null) return;
        time.value = value;
    }

    public void UpdateBlockBreak(int current, int target)
    {
        if (blockBreakProgress == null) return;
        blockBreakProgress.text = $"{current} / {target}";
    }

    public void UpdateHeightProgress(int currentHeight, int targetHeight)
    {
        int logicalHeight = currentHeight + 1;

        if (HeightLimitProgress != null && HeightLimitPanel.activeSelf)
            HeightLimitProgress.text = $"현재 높이 : {logicalHeight} / 목표 높이 : {targetHeight}";

        if (HeightAchievementProgress != null && HeightAchievementPanel.activeSelf)
            HeightAchievementProgress.text = $"현재 높이 : {logicalHeight} / 목표 높이 : {targetHeight}";

        if (HeightKeepProgress != null && HeightKeepPanel.activeSelf)
            HeightKeepProgress.text = $"높이 {logicalHeight} 유지";
    }

    public void UpdateHeightSpecialBlock(int currentHeight, int targetHeight, BlockType targetBlock, bool isInstalled)
    {
        if (HeightSpecialBlockProgress == null) return;

        int logicalCurrent = currentHeight + 1;
        int logicalTarget = targetHeight + 1;

        HeightSpecialBlockProgress.text =
            $"현재 높이 : {logicalCurrent}\n" +
            $"목표 : {logicalTarget}층에 {targetBlock} " +
            $"{(isInstalled ? "✔" : "✘")}";
    }

    public void ShowCompleted()
    {
        if (progressText != null)
            progressText.text = "퀘스트 완료!";

        // 필요하면 일정 시간 후 UI 제거
        StartCoroutine(AutoClose());
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        CurrentUI = null;
    }

}