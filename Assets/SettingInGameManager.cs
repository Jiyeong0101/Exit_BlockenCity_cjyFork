using UnityEngine;

public class SettingInGameManager : MonoBehaviour
{
    [Header("UI Button Handler")]
    [SerializeField] private UIButtonHandler uiButtonHandler;

    private void Start()
    {
        // 인스펙터에 직접 할당하지 않은 경우 씬 내에서 찾습니다.
        if (uiButtonHandler == null)
        {
            uiButtonHandler = FindObjectOfType<UIButtonHandler>();
        }
    }

    private void Update()
    {
        // ESC 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    /// <summary>
    /// 일시정지 상태를 토글(ON/OFF)합니다.
    /// </summary>
    public void TogglePause()
    {
        if (uiButtonHandler == null)
        {
            uiButtonHandler = FindObjectOfType<UIButtonHandler>();

            if (uiButtonHandler == null)
            {
                Debug.LogWarning("[SettingManager] 씬 내에서 UIButtonHandler를 찾을 수 없습니다.");
                return;
            }
        }

        // 게임이 이미 종료된 상태라면 일시정지 메뉴가 열리지 않도록 예외 처리
        if (GameManager.Instance != null && GameManager.Instance.isGameEnded)
        {
            return;
        }

        // 현재 일시정지 상태 확인 후 토글 처리
        if (GameManager.Instance != null && GameManager.Instance.isPaused)
        {
            uiButtonHandler.ResumeGame(); // 일시정지 해제
        }
        else
        {
            uiButtonHandler.PauseGame(); // 일시정지 실행
        }
    }
}
