using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialQuestSpawner : MonoBehaviour
{
    [Header("Timer Slider")]
    public Slider timeSlider;

    [Header("Status Sliders (A,B,C)")]
    public Slider sliderA;
    public Slider sliderB;
    public Slider sliderC;

    [Header("Spawn Ratio Range (0~1)")]
    [Range(0f, 1f)] public float minSpawnRatio = 0.2f;
    [Range(0f, 1f)] public float maxSpawnRatio = 0.8f;

    private float nextSpawnRatio;
    private bool hasSpawnedThisCycle = false;

    private bool cycleStarted = false;

    void Start()
    {
        SetNextSpawnRatio();
    }

    void Update()
    {
        if (timeSlider == null) return;

        float ratio = 1f - (timeSlider.value / timeSlider.maxValue);

        if (!cycleStarted && ratio > 0.01f)
        {
            cycleStarted = true;
            hasSpawnedThisCycle = false;
            nextSpawnRatio = Random.Range(minSpawnRatio, maxSpawnRatio);

            Debug.Log($"새 스폰 타이밍: {nextSpawnRatio}");
        }

        if (ratio > 0.99f)
        {
            cycleStarted = false;
        }

        if (cycleStarted && !hasSpawnedThisCycle && ratio >= nextSpawnRatio)
        {
            SpawnSpecialQuest();
            hasSpawnedThisCycle = true;
        }
    }

    void SetNextSpawnRatio()
    {
        nextSpawnRatio = Random.Range(minSpawnRatio, maxSpawnRatio);
    }

    int SliderToDigit(Slider slider)
    {
        if (slider == null) return 0;
        float t = Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value);
        return Mathf.Clamp(Mathf.RoundToInt(t * 9f), 0, 9);
    }

    void SpawnSpecialQuest()
    {
        int month = 1;  // 1~12

        int a = SliderToDigit(sliderA);
        int b = SliderToDigit(sliderB);
        int c = SliderToDigit(sliderC);

        int generatedID = month * 1000 + a * 100 + b * 10 + c;

        Debug.Log($"생성된 QuestID → {generatedID}");

        int finalID = FindBestBranchID(generatedID);

        Debug.Log($"최종 QuestID → {finalID}");

        DialogUI.Instance.StartDialog(finalID);

        SetNextSpawnRatio();
    }

    // ⭐ 자리별 차이 기반 가장 가까운 ID 찾기
    int FindBestBranchID(int targetID)
    {
        List<int> allBranch = DialogManager.Instance.GetAllBranchIDs();

        if (allBranch.Contains(targetID))
            return targetID;

        int bestID = -1;
        int bestScore = int.MaxValue;

        int t100 = (targetID / 100) % 10;
        int t10 = (targetID / 10) % 10;
        int t1 = targetID % 10;

        foreach (int id in allBranch)
        {
            if (id / 1000 != targetID / 1000) continue; // 같은 월만

            int i100 = (id / 100) % 10;
            int i10 = (id / 10) % 10;
            int i1 = id % 10;

            int score =
                Mathf.Abs(t100 - i100) +
                Mathf.Abs(t10 - i10) +
                Mathf.Abs(t1 - i1);

            if (score < bestScore)
            {
                bestScore = score;
                bestID = id;
            }
        }

        return bestID;
    }
}