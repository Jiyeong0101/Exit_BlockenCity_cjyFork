using UnityEngine;
using UnityEngine.UI;

public class TitleMenuUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button continueButton;

    private void Start()
    {
        RefreshContinueButton();
    }

    private void RefreshContinueButton()
    {
        bool hasSaveData =
            Datamanager.Instance.HasValidSaveData();

        continueButton.interactable = hasSaveData;
    }
}