using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NicknameUI : MonoBehaviour
{
    [Header("UI 연결")]
    public TMP_InputField nicknameInput;
    public GameObject warningText;
    public Animator nameAnimator;

    [Header("씬 설정")]
    [SerializeField] private string lobbySceneName = "Lobby";

    private Coroutine warningCoroutine;

    private const int MIN_LENGTH = 1;
    private const int MAX_LENGTH = 8;


    // 옆의 체크 버튼
    public void OnClickCheck()
    {
        string inputName =
            nicknameInput.text.Trim();

        // 공백이면 기본 닉네임 사용
        if (string.IsNullOrEmpty(inputName))
        {
            inputName = "한서안";
        }

        // 글자 수 검사
        if (inputName.Length < MIN_LENGTH ||
            inputName.Length > MAX_LENGTH)
        {
            ShowWarning(
                $"닉네임은 {MIN_LENGTH}~{MAX_LENGTH}자 사이여야 합니다.");

            return;
        }

        // 검사 통과 시 애니메이션 실행
        if (nameAnimator != null)
        {
            nameAnimator.Play("Confirm");
        }
    }


    // 저장 확인 팝업의 '네' 버튼
    public void OnClickSaveConfirm()
    {
        string inputName =
            nicknameInput.text.Trim();

        // 공백이면 기본 닉네임 사용
        if (string.IsNullOrEmpty(inputName))
        {
            inputName = "한서안";
        }

        // 닉네임 저장
        GameDataManager.Instance.SetPlayerName(
            inputName);

        GameDataManager.Instance.SaveGameData();

        // 로비 씬 이동
        SceneManager.LoadScene(lobbySceneName);
    }


    private void ShowWarning(string message)
    {
        if (warningCoroutine != null)
        {
            StopCoroutine(
                warningCoroutine);
        }

        warningCoroutine =
            StartCoroutine(
                WarningRoutine(message));
    }


    private IEnumerator WarningRoutine(
        string message)
    {
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        warningText.gameObject.SetActive(false);
    }
}