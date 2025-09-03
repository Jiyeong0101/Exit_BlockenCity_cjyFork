using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisController : MonoBehaviour
{
    // 현재 시점 카메라
    public Camera gameCamera;
    // 현재 조작중인 블럭
    public TetriminoBlock currentBlock;

    private InputManager input;

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

        input.OnLeftArrow += MoveLeft;
        input.OnRightArrow += MoveRight;
        input.OnUpArrow += MoveForward;
        input.OnDownArrow += MoveBack;
        input.OnKeyF += SoftDrop;
        input.OnSpace += HardDrop;
        input.OnKeyA += XRotate;
        input.OnKeyS += YRotate;
        input.OnKeyD += ZRotate;
        input.OnKeyC += BlockChange;
    }


    // 입력 처리 비활성화
    private void OnDisable()
    {
        if (input == null) return;
        input.OnLeftArrow -= MoveLeft;
        input.OnRightArrow -= MoveRight;
        input.OnUpArrow -= MoveForward;
        input.OnDownArrow -= MoveBack;
        input.OnKeyF -= SoftDrop;
        input.OnSpace -= HardDrop;
        input.OnKeyA -= XRotate;
        input.OnKeyS -= YRotate;
        input.OnKeyD -= ZRotate;
        input.OnKeyC -= BlockChange;
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
                
        return moveDir;
    }

    public void SetCurrentBlock(TetriminoBlock block)
    {
        currentBlock = block;
    }

}
