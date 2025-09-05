using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTest : MonoBehaviour
{
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

        GameObstacleSystem.Instance.SelectObstacleForMonth(currentMonth);

        var obstacle = GameObstacleSystem.Instance.GetSelectedObstacle();
        uiText.text = $"{currentMonth}월 방해물: {obstacle.type}";
    }


}
