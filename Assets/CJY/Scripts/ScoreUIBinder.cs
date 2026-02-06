using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUIBinder : MonoBehaviour
{
    [Header("Text References")]
    public TMP_Text playerNameText;
    public TMP_Text playerNameText2;
    public TMP_Text totalMoneyText;
    public TMP_Text salaryText;

    [Header("Debug / Test")]
    public int testSalary = 0; // 아직 시스템 없으므로 임시

    public void Refresh()
    {
        var saveData = Datamanager.Instance.saveData;

        // 플레이어 이름
        playerNameText.text = saveData.player.playerName;
        playerNameText2.text = saveData.player.playerName;

        // 총 자산
        totalMoneyText.text =
            saveData.player.totalMoney.ToString();

        // 월급 (임시값)
        salaryText.text =
            testSalary.ToString();
    }
}
