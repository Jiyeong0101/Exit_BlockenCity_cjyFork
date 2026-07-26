using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NicknameUI : MonoBehaviour
{
    [Header("UI 연결")]
    public TMP_InputField nicknameInput;
    public GameObject warningText;
    public Animator nameAnimator; // 애니메이터를 코드로 제어하기 위해 추가

    [Header("씬 설정")]
    [SerializeField] private string lobbySceneName = "Lobby";

    private Coroutine warningCoroutine;

    private const int MIN_LENGTH = 1;
    private const int MAX_LENGTH = 8;

    // [옆의 체크 버튼]에 연결할 함수
    public void OnClickCheck()
    {
        string inputName = nicknameInput.text.Trim();

        // 빈 값 검사
        if (string.IsNullOrEmpty(inputName))
        {
            ShowWarning("닉네임을 입력해주세요.");
            return;
        }

        // 글자 수 제한 검사
        if (inputName.Length < MIN_LENGTH || inputName.Length > MAX_LENGTH)
        {
            ShowWarning($"닉네임은 {MIN_LENGTH}~{MAX_LENGTH}자 사이여야 합니다.");
            return;
        }

        // 검사를 통과했을 때만 애니메이션 실행!
        if (nameAnimator != null)
        {
            nameAnimator.Play("Confirm");
        }
    }

    // ["정말 저장하시겠습니까?" 팝업의 '네' 버튼]에 연결할 함수
    public void OnClickSaveConfirm()
    {
        string inputName = nicknameInput.text.Trim();

        if (string.IsNullOrEmpty(inputName)) return;

        // 최종 데이터 저장
        Datamanager.Instance.saveData.player.playerName = inputName;
        Datamanager.Instance.SaveGameData();

        // 로비 씬으로 이동
        SceneManager.LoadScene(lobbySceneName);
    }

    private void ShowWarning(string message)
    {
        if (warningCoroutine != null)
            StopCoroutine(warningCoroutine);

        warningCoroutine = StartCoroutine(WarningRoutine(message));
    }

    private IEnumerator WarningRoutine(string message)
    {
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        warningText.gameObject.SetActive(false);
    }
}