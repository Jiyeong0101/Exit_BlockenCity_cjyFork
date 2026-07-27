using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    public enum ButtonType
    {
        GoToLobby,
        RetryStage,
        NextResultScene,
        PauseGame,
        ResumeGame
    }

    public ButtonType buttonType;

    [Header("UI Window (Pause/Resume 시 켜고 끌 창)")]
    public GameObject settingsWindow;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn == null) return;

        btn.onClick.AddListener(() => {
            switch (buttonType)
            {
                case ButtonType.GoToLobby:
                    SceneManager.LoadScene("Lobby");
                    break;

                case ButtonType.RetryStage:
                    if (StageManager.Instance != null)
                        StageManager.Instance.RestartStage();
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.ResetGame();
                    }

                    break;

                case ButtonType.NextResultScene:
                    if (GameManager.Instance != null && GameManager.Instance.scoreManager != null)
                    {
                        GameManager.Instance.scoreManager.OnNextButtonClick();
                    }
                    break;

                case ButtonType.PauseGame:
                    PauseGame();
                    break;

                case ButtonType.ResumeGame:
                    ResumeGame();
                    break;
            }
        });
    }

    public void PauseGame()
    {
        // 1. GameManager 타이머 정지
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }

        // 2. TetrisManager 일시정지 처리
        if (TetrisManager.Instance != null)
        {
            TetrisManager.Instance.SetPause(true);
        }

        // 3. 설정 창 열기
        if (settingsWindow != null)
        {
            settingsWindow.SetActive(true);
        }

        Debug.Log("[PauseGame] 일시정지 완료");
    }

    public void ResumeGame()
    {
        // 1. 설정 창 닫기
        if (settingsWindow != null)
        {
            settingsWindow.SetActive(false);
        }

        // 2. GameManager 타이머 재개
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }

        // 3. TetrisManager 일시정지 해제
        if (TetrisManager.Instance != null)
        {
            TetrisManager.Instance.SetPause(false);
        }

        Debug.Log("[ResumeGame] 게임 재개 완료");
    }
}