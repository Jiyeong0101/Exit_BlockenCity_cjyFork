using TMPro;
using UnityEngine;

public class MonthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monthText;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        monthText.text =
            GameDataManager.Instance.GetCurrentStage().ToString("00");
    }
}