using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  

public class ScoreUIBinder : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerNameText2;
    public TextMeshProUGUI totalMoneyText;
    public TextMeshProUGUI salaryText;
    //public TextMeshProUGUI favorabilityText;

    [Header("Debug / Test")]
    public int testSalary = 0; // 아직 시스템 없으니까 임시 값

    public void Refresh()
    {
        //Debug.Log("UI Refresh 호출됨");

        var saveData = Datamanager.Instance.saveData;

        // 플레이어 이름
        playerNameText.text =
            saveData.player.playerName;

        playerNameText2.text =
            saveData.player.playerName;

        // 총자산
        totalMoneyText.text =
            saveData.player.totalMoney.ToString();

        // 월급 (아직 계산 시스템 없으므로 임시값)
        salaryText.text =
            testSalary.ToString();

        // 우호도 (예시: 단월국)
        //favorabilityText.text =
        //    saveData.relationship.danwol.ToString();
    }
}
