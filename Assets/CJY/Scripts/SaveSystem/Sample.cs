using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    void Start()
    {
        Datamanager.Instance.LoadGameData();

        Debug.Log("현재 스테이지: " +
            Datamanager.Instance.saveData.progress.currentStage);
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
        }
    }

    public void AddMoney(int amount)
    {
        Datamanager.Instance.saveData.player.totalMoney += amount;
        Datamanager.Instance.SaveGameData();
    }

    public void AddDanwolFavor(int amount)
    {
        Datamanager.Instance.saveData.relationship.danwol += amount;
        Datamanager.Instance.SaveGameData();
    }
}
