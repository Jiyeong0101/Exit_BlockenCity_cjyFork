using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMonthInitializer : MonoBehaviour
{
    [Header("Test Option")]
    public bool useTestMonth = false;   // 테스트 모드 여부
    public int testMonth = 1;           // 테스트용 월

    [Header("Runtime")]
    public int currentMonth = 1;

    [SerializeField] private Text uiText;

    private void Start()
    {
        if (GameObstacleSystem.Instance == null)
        {
            Debug.LogError("GameObstacleSystem.Instance is null!");
            return;
        }

        if (uiText == null)
        {
            Debug.LogError("uiText is null!");
            return;
        }

        // 테스트 모드 분기
        if (useTestMonth)
        {
            currentMonth = Mathf.Clamp(testMonth, 1, 12);
        }
        else
        {
            currentMonth = Datamanager.Instance.saveData.progress.currentStage;
            currentMonth = Mathf.Clamp(currentMonth, 1, 12);
        }

        ApplyMonth(currentMonth);
    }

    private void ApplyMonth(int month)
    {
        GameObstacleSystem.Instance.SelectObstacleForMonth(month);

        var obstacle = GameObstacleSystem.Instance.GetSelectedObstacle();
        uiText.text = $"{month}월 방해물: {obstacle.type}";
    }
}