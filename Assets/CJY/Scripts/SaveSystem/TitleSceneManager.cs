using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject nicknameUI;

    private void Start()
    {
        Datamanager.Instance.LoadGameData();

        if (string.IsNullOrEmpty(Datamanager.Instance.saveData.player.playerName))
        {
            nicknameUI.SetActive(true);
        }
        else
        {
            nicknameUI.SetActive(false);
        }
    }
}
