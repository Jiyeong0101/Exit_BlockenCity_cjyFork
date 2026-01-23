using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    [Header("UI")]
    public ScoreUIBinder scoreUIBinder; // 점수 UI 바인더 참조

    void Start()
    {
        // 저장 데이터 로드
        Datamanager.Instance.LoadGameData();

        Debug.Log(
            "로드 후 이름: " +
            Datamanager.Instance.saveData.player.playerName
        );

        Debug.Log(
            "현재 스테이지: " +
            Datamanager.Instance.saveData.progress.currentStage
        );

        // 로드 완료 후 UI 갱신
        if (scoreUIBinder != null)
        {
            scoreUIBinder.Refresh();
        }
        else
        {
            Debug.LogWarning("ScoreUIBinder가 연결되지 않았습니다.");
        }
    }

    private void OnApplicationQuit()
    {
        Datamanager.Instance.SaveGameData();
    }

    public void NextStage()
    {
        var progress = Datamanager.Instance.saveData.progress;

        if (progress.currentStage < 12)
        {
            progress.currentStage++;
            Datamanager.Instance.SaveGameData();

            // 스테이지 변경 후 UI도 갱신
            scoreUIBinder?.Refresh();
        }
    }

    public void AddMoney(int amount)
    {
        Datamanager.Instance.saveData.player.totalMoney += amount;
        Datamanager.Instance.SaveGameData();

        scoreUIBinder?.Refresh();
    }

    public void AddDanwolFavor(int amount)
    {
        Datamanager.Instance.saveData.relationship.danwol += amount;
        Datamanager.Instance.SaveGameData();

        scoreUIBinder?.Refresh();
    }
}
