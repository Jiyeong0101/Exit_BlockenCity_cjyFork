using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputBlocker : MonoBehaviour
{
    public bool blockRotation = false;
    public bool blockHardDrop = false;

    public bool isInputBlocked = false;

    // 델리게이트 참조 저장
    private Action leftArrowHandler;
    private Action rightArrowHandler;
    private Action upArrowHandler;
    private Action downArrowHandler;
    private Action keySHandler;
    private Action spaceHandler;
    private Action keyAHandler;
    private Action keyFHandler;
    private Action keyDHandler;
    private Action keyCHandler;

    public event Action OnLeftArrow;
    public event Action OnRightArrow;
    public event Action OnUpArrow;
    public event Action OnDownArrow;
    public event Action OnKeyS;
    public event Action OnSpace;
    public event Action OnKeyA;
    public event Action OnKeyF;
    public event Action OnKeyD;
    public event Action OnKeyC;

    // 입력 동작 큐 및 상태
    private Queue<Action> inputQueue = new();
    private bool isProcessingInput = false;

    // 딜레이 시간
    public float inputProcessDelay = 0.0f;

    void Start()
    {
        HookInput();
    }

    public void SetInputProcessDelay(float delay)
    {
        inputProcessDelay = delay;
    }

    public void ResetAll()
    {
        isInputBlocked = false;
        blockHardDrop = false;

        inputQueue.Clear();
        isProcessingInput = false;
        StopAllCoroutines();

        inputProcessDelay = 0;
    }

    public void HookInput()
    {
        // 큐에 넣기만 함, 바로 실행 X
        leftArrowHandler = () =>
        {
            if (!isInputBlocked)
                EnqueueInput(() => OnLeftArrow?.Invoke());
        };
        rightArrowHandler = () =>
        {
            if (!isInputBlocked)
                EnqueueInput(() => OnRightArrow?.Invoke());
        };
        upArrowHandler = () =>
        {
            if (!isInputBlocked)
                EnqueueInput(() => OnUpArrow?.Invoke());
        };
        downArrowHandler = () =>
        {
            if (!isInputBlocked)
                EnqueueInput(() => OnDownArrow?.Invoke());
        };

        keySHandler = () =>
        {
            if (!isInputBlocked && !blockRotation)
                EnqueueInput(() => OnKeyS?.Invoke());
        };

        spaceHandler = () =>
        {
            if (!isInputBlocked && !blockHardDrop)
                EnqueueInput(() => OnSpace?.Invoke());
        };

        keyAHandler = () =>
        {
            if (!isInputBlocked && !blockRotation)
                EnqueueInput(() => OnKeyA?.Invoke());
        };
        keyFHandler = () =>
        {
            if (!isInputBlocked)
                EnqueueInput(() => OnKeyF?.Invoke());
        };
        keyDHandler = () =>
        {
            if (!isInputBlocked && !blockRotation)
                EnqueueInput(() => OnKeyD?.Invoke());
        };

        keyCHandler = () =>
        {
            EnqueueInput(() => OnKeyC?.Invoke());
        };

        // 기존 InputManager 이벤트 연결
        InputManager.Instance.OnLeftArrow += leftArrowHandler;
        InputManager.Instance.OnRightArrow += rightArrowHandler;
        InputManager.Instance.OnUpArrow += upArrowHandler;
        InputManager.Instance.OnDownArrow += downArrowHandler;
        
        InputManager.Instance.OnSpace += spaceHandler;

        InputManager.Instance.OnKeyS += keySHandler;
        InputManager.Instance.OnKeyA += keyAHandler;
        InputManager.Instance.OnKeyD+= keyDHandler;
        InputManager.Instance.OnKeyF += keyFHandler;
        InputManager.Instance.OnKeyC += keyCHandler;
    }

    public void UnhookInput()
    {
        InputManager.Instance.OnLeftArrow -= leftArrowHandler;
        InputManager.Instance.OnRightArrow -= rightArrowHandler;
        InputManager.Instance.OnUpArrow -= upArrowHandler;
        InputManager.Instance.OnDownArrow -= downArrowHandler;
        
        InputManager.Instance.OnSpace -= spaceHandler;
        InputManager.Instance.OnKeyS -= keySHandler;
        InputManager.Instance.OnKeyA -= keyAHandler;
        InputManager.Instance.OnKeyF -= keyFHandler;
        InputManager.Instance.OnKeyD -= keyDHandler;
        InputManager.Instance.OnKeyC -= keyCHandler;
    }

    private void EnqueueInput(Action inputAction)
    {
        inputQueue.Enqueue(inputAction);
        if (!isProcessingInput)
        {
            StartCoroutine(ProcessInputQueue());
        }
    }

    private IEnumerator ProcessInputQueue()
    {
        isProcessingInput = true;

        while (inputQueue.Count > 0)
        {
            var inputAction = inputQueue.Dequeue();
            inputAction?.Invoke();

            yield return new WaitForSeconds(inputProcessDelay);
        }

        isProcessingInput = false;
    }
}
