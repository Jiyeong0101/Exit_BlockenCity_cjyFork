using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//스페셜퀘스트로 활용중
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
    public List<SpecialQuestData> allQuestData; // Branch 번호 기반 인덱스 접근

    void Start()
    {
        acceptButton.onClick.AddListener(OnAcceptClicked);
        declineButton.onClick.AddListener(OnDeclineClicked);

        HideAllButtons();
        dialogPanel.SetActive(false);

        InputManager.Instance.OnKeyV += HandleKeyV;
        InputManager.Instance.OnKey1 += HandleKey1;
        InputManager.Instance.OnKey2 += HandleKey2;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnKeyV -= HandleKeyV;
            InputManager.Instance.OnKey1 -= HandleKey1;
            InputManager.Instance.OnKey2 -= HandleKey2;
        }
    }

    private void HandleKeyV()
    {
        if (!dialogPanel.activeSelf) return;

        if (isTyping)
        {
            FinishTyping(); // 타이핑 중이면 전체 출력
            return;
        }

        if (isWaitingForChoice)
        {
            return; // 선택지 상태에서는 스킵만 허용
        }
        ShowNextDialog();   // 선택지가 없고, 타이핑도 끝난 상태면 다음 대사 진행
    }


    private void HandleKey1()
    {
        if (!dialogPanel.activeSelf) return;
        if (isWaitingForChoice) OnAcceptClicked();
    }

    private void HandleKey2()
    {
        if (!dialogPanel.activeSelf) return;
        if (isWaitingForChoice) OnDeclineClicked();
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
            SpecialQuestData matchedQuest = allQuestData.Find(q => q.branchID == currentLine.acceptBranch);
            if (matchedQuest != null)
            {
                SpecialQuestManager.Instance.AddQuest(matchedQuest);
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
