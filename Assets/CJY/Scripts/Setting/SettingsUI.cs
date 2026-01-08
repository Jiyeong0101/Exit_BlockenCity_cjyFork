using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    [Header("Settings Panel")]
    public GameObject settingsPanel;

    private bool isOpen = false;

    private void Start()
    {
        // 시작할 때 설정창 닫아두기
        settingsPanel.SetActive(false);
        isOpen = false;
    }

    public void ToggleSettings()
    {
        isOpen = !isOpen;
        settingsPanel.SetActive(isOpen);

        if (isOpen)
            OnOpen();
        else
            OnClose();
    }

    public void OpenSettings()
    {
        isOpen = true;
        settingsPanel.SetActive(true);
        OnOpen();
    }

    public void CloseSettings()
    {
        isOpen = false;
        settingsPanel.SetActive(false);
        OnClose();
    }

    private void OnOpen()
    {
        // 설정 열릴 때 필요한 처리
    }

    private void OnClose()
    {
        // 설정 닫힐 때 필요한 처리
    }
}
