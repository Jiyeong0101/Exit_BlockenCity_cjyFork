using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisController : MonoBehaviour
{
    // 현재 시점 카메라
    public GameObject gameCamera;

    // 현재 조작중인 블럭
    public TetriminoBlock currentBlock;

    private InputManager input;

    [Header("카메라 입력 반전")]
    public bool reverseInput = false;

    // 추가
    // 입력 제어 클래스
    public InputBlocker inputBlocker;

    // 입력 처리 활성화
    private void OnEnable()
    {
        input = InputManager.Instance ?? FindObjectOfType<InputManager>();
        if (input == null)
        {
            Debug.LogError("[TetrisController] InputManager가 씬에 없습니다.");
            enabled = false;
            return;
        }

        inputBlocker.OnLeftArrow += MoveLeft;
        inputBlocker.OnRightArrow += MoveRight;
        inputBlocker.OnUpArrow += MoveForward;
        inputBlocker.OnDownArrow += MoveBack;

        inputBlocker.OnKeyF += SoftDrop;
        inputBlocker.OnSpace += HardDrop;

        inputBlocker.OnKeyA += XRotate;
        inputBlocker.OnKeyS += YRotate;
        inputBlocker.OnKeyD += ZRotate;

        inputBlocker.OnKeyC += BlockChange;
    }


    // 입력 처리 비활성화
    private void OnDisable()
    {
        inputBlocker.UnhookInput();
    }

    //추가
    //바람에 따라 밀리는 블록
    public void MoveBlockByWind(Vector3 dir)
    {
        if (currentBlock == null) return;
        if (currentBlock.CanMove(dir))
            currentBlock.Move(dir);
    }

    private void MoveLeft() 
    {
        if (currentBlock == null) return;
        Vector3 dir = GetCameraRelativeDirection(Vector3.left);
        if (currentBlock.CanMove(dir)) currentBlock.Move(dir);
    }
    private void MoveRight()
    {
        if (currentBlock == null) return;
        Vector3 dir = GetCameraRelativeDirection(Vector3.right);
        if (currentBlock.CanMove(dir)) currentBlock.Move(dir);
    }
    private void MoveForward()
    {
        if (currentBlock == null) return;
        Vector3 dir = GetCameraRelativeDirection(Vector3.back);
        if (currentBlock.CanMove(dir)) currentBlock.Move(dir);
    }
    private void MoveBack()
    {
        if (currentBlock == null) return;
        Vector3 dir = GetCameraRelativeDirection(Vector3.forward);
        if (currentBlock.CanMove(dir)) currentBlock.Move(dir);
    }

    private void SoftDrop() 
    {
        if (currentBlock == null) return;
        Vector3 dir = Vector3.down;
        if (currentBlock.CanMove(dir)) currentBlock.Move(dir);
    }

    private void HardDrop() 
    {
        if (currentBlock == null) return;

        // 최대 하강 횟수 = 타워 높이
        int guard = TetrisManager.Instance.tetrisTowerSize.y;

        while (guard-- > 0 && currentBlock.CanMove(Vector3.down))
        {
            currentBlock.Move(Vector3.down); // 한 칸씩 즉시 내림 (한 프레임에 처리됨 -> 눈에 보이는 이동 없음)
        }

        currentBlock.BlockLock(); // 바닥/블럭에 닿았으면 바로 락
    }

    private void XRotate()
    {
        if (currentBlock == null) return;
        currentBlock.RotateX();
    }

    private void YRotate()
    {
        if (currentBlock == null) return;
        currentBlock.RotateY();
    }

    private void ZRotate()
    {
        if (currentBlock == null) return;
        currentBlock.RotateZ();
    }

    private void BlockChange() 
    {
        if (currentBlock == null) return;

        var spawner = FindObjectOfType<TetrisSpawner>();
        if (spawner == null) return;

        Vector3 target = currentBlock.transform.position;
        bool ok = spawner.TrySwapWithNext(target);

        if (ok)
        {
            currentBlock = spawner.GetTetriminoBlock();
        }
    }

    private Vector3 GetCameraRelativeDirection(Vector3 inputDir)
    {
        Vector3 forward = gameCamera.transform.forward;
        Vector3 right = gameCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // 입력 방향을 카메라 기준으로 변환
        Vector3 moveDir = inputDir.x * right + inputDir.z * forward;

        if (reverseInput)
            moveDir *= -1f;

        return moveDir;
    }

    public void SetCurrentBlock(TetriminoBlock block)
    {
        currentBlock = block;
    }

    public void SetReverseInput(bool isReversed)
    {
        reverseInput = isReversed;
    }

}
