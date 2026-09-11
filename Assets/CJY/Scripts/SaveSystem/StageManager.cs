using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public StageData stageData = new StageData();


    private void Awake()
    {
        Instance = this;
    }


    public void StartStage()
    {
        stageData.Reset();
    }


    // =====================================================
    // Stage Money
    // =====================================================

    public void AddMoney(int amount)
    {
        stageData.earnedMoney += amount;
    }


    // =====================================================
    // Stage Relationship
    // =====================================================

    public void AddRelationship(
        RelationshipType type,
        float amount)
    {
        stageData.AddRelationship(
            type,
            amount);
    }


    public float GetRelationshipDelta(
        RelationshipType type)
    {
        return stageData.GetRelationship(type);
    }


    // =====================================================
    // Game Over
    // =====================================================

    public void OverStage()
    {
        stageData.Reset();
    }


    // =====================================================
    // Clear
    // =====================================================

    public void ClearStage()
    {
        int currentStage =
            GameDataManager.Instance.GetCurrentStage();

        int baseSalary =
            GetBaseSalary(currentStage);


        // 재화 반영
        int totalReward =
            baseSalary + stageData.earnedMoney;

        if (totalReward > 0)
        {
            GameDataManager.Instance.AddMoney(
                totalReward);
        }


        // 우호도 반영
        foreach (
            RelationshipType type
            in System.Enum.GetValues(typeof(RelationshipType)))
        {
            float delta =
                stageData.GetRelationship(type);

            if (!Mathf.Approximately(delta, 0f))
            {
                GameDataManager.Instance.AddRelationship(
                    type,
                    delta);
            }
        }


        // 이번 판 임시 데이터 초기화
        stageData.Reset();
    }

    // =====================================================
    // Salary
    // =====================================================

    public int GetBaseSalary(int stage)
    {
        return 10000 +
               ((stage - 1) / 3) * 2000;
    }


    // =====================================================
    // Restart
    // =====================================================

    public void RestartStage()
    {
        StartStage();

        string currentSceneName =
            SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(
            currentSceneName);
    }
}