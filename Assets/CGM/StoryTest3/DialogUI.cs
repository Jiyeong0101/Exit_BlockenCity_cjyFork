using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [Header("UI Components")]
    public Text nameText;
    public Text dialogText;
    public GameObject dialogPanel;

    public Button acceptButton;
    public Button declineButton;

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f;

    private DialogLine currentLine;
    private bool isTyping = false;
    private string currentDialog = "";
    private Coroutine typingCoroutine;
    private bool isWaitingForChoice = false;

    [Header("Quest Data")]
    public List<NewQuestData> allQuestData; // Branch 번호 기반 인덱스 접근

    void Start()
    {
        acceptButton.onClick.AddListener(OnAcceptClicked);
        declineButton.onClick.AddListener(OnDeclineClicked);

        HideAllButtons();
        dialogPanel.SetActive(false);
    }

    void Update()
    {
        if (!dialogPanel.activeSelf) return;

        // 👉 수락/거절 대기 중일 때 숫자 키 입력 처리
        if (isWaitingForChoice)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnAcceptClicked(); // 1번 키 = 수락
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnDeclineClicked(); // 2번 키 = 거절
            }

            return; // ❗ 수락/거절 대기 중엔 Space 입력 무시
        }

        // 👉 일반 대사 진행을 위한 Space 입력 처리
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                FinishTyping(); // 타이핑 중이면 전체 출력
            }
            else
            {
                ShowNextDialog(); // 타이핑 완료 후 다음 대사
            }
        }
    }

    public void StartDialog(int branch)
    {
        DialogManager.Instance.LoadDialogByBranch(branch);
        isWaitingForChoice = false;
        HideAllButtons();
        dialogPanel.SetActive(true);
        ShowNextDialog();
    }

    public void ShowNextDialog()
    {
        HideAllButtons();

        if (!DialogManager.Instance.HasMoreDialog())
        {
            nameText.text = "";
            dialogText.text = "대화가 종료되었습니다.";
            dialogPanel.SetActive(false);
            return;
        }

        currentLine = DialogManager.Instance.GetNextDialog();

        nameText.text = currentLine.name;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeDialog(currentLine.dialog));

        if (currentLine.questionType == 1)  // 수락/거절 선택 대기
        {
            isWaitingForChoice = true;
        }
        else
        {
            isWaitingForChoice = false;
        }
    }

    private IEnumerator TypeDialog(string dialog)
    {
        isTyping = true;
        currentDialog = dialog;
        dialogText.text = "";

        foreach (char c in dialog.ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (currentLine.questionType == 1)
        {
            ShowChoiceButtons();
        }
    }

    private void FinishTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogText.text = currentDialog;
        isTyping = false;

        if (currentLine.questionType == 1)
        {
            ShowChoiceButtons();
        }
    }

    private void ShowChoiceButtons()
    {
        acceptButton.gameObject.SetActive(true);
        declineButton.gameObject.SetActive(true);
    }

    private void HideAllButtons()
    {
        acceptButton.gameObject.SetActive(false);
        declineButton.gameObject.SetActive(false);
    }

    private void OnAcceptClicked()
    {
        Debug.Log("수락 선택됨");
        isWaitingForChoice = false;
        HideAllButtons();

        if (currentLine.acceptBranch != -1)
        {
            NewQuestData matchedQuest = allQuestData.Find(q => q.branchID == currentLine.acceptBranch);
            if (matchedQuest != null)
            {
                NewQuestManager.Instance.AddQuest(matchedQuest);
            }
            Debug.Log("수락 선택됨2");
            DialogManager.Instance.LoadDialogByBranch(currentLine.acceptBranch);
        }

        ShowNextDialog();
    }

    private void OnDeclineClicked()
    {
        Debug.Log("거절 선택됨");
        isWaitingForChoice = false;
        HideAllButtons();

        if (currentLine.declineBranch != -1)
        {
            DialogManager.Instance.LoadDialogByBranch(currentLine.declineBranch);
            ShowNextDialog();
        }
        else
        {
            DialogManager.Instance.ClearQueue();
            nameText.text = "";
            dialogText.text = "대화를 거절했습니다.";
            dialogPanel.SetActive(false);
        }
    }
}