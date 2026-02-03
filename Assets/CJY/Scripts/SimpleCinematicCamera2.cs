using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCinematicCamera2 : MonoBehaviour
{
    [Header("카메라 이동 포인트 (원형 구조)")]
    public Transform[] cameraPoints; // 4개

    [Header("이동 속도")]
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    [Header("이동 옵션")]
    public bool smoothMove = true;
    public float stopThreshold = 0.001f;

    [Header("연결 컨트롤러")]
    public TetrisController tetrisController;

    private int currentIndex = 0;

    void Start()
    {
        if (cameraPoints.Length == 0) return;

        transform.position = cameraPoints[currentIndex].position;
        transform.rotation = cameraPoints[currentIndex].rotation;
    }

    void Update()
    {
        HandleInput();

        Transform target = cameraPoints[currentIndex];
        if (target == null) return;

        // 위치
        if (smoothMove)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                Time.deltaTime * moveSpeed
            );
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );
        }

        if (Vector3.Distance(transform.position, target.position) < stopThreshold)
            transform.position = target.position;

        // 회전 (끊김 없음)
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            target.rotation,
            Time.deltaTime * rotateSpeed
        );
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            RotateLeft();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateRight();
        }
    }

    private void RotateLeft()
    {
        currentIndex = (currentIndex - 1 + cameraPoints.Length) % cameraPoints.Length;

        UpdateTetrisInput();
    }

    private void RotateRight()
    {
        currentIndex = (currentIndex + 1) % cameraPoints.Length;

        UpdateTetrisInput();
    }

    private void UpdateTetrisInput()
    {
        if (tetrisController == null) return;

        // 카메라가 좌측 절반이면 입력 반전
        bool reverse = currentIndex < cameraPoints.Length / 2;
        tetrisController.SetReverseInput(reverse);
    }
}

