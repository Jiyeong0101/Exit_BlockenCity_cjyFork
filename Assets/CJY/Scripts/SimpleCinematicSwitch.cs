using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCinematicCamera : MonoBehaviour
{
    [Header("카메라 이동 포인트")]
    public Transform pointA;
    public Transform pointB;

    [Header("이동 속도 설정")]
    [Tooltip("위치 이동 속도 (MoveSpeed가 높을수록 빠름)")]
    public float moveSpeed = 2f;

    [Tooltip("회전 속도 (높을수록 빠름)")]
    public float rotateSpeed = 2f;

    [Header("이동 모드 설정")]
    [Tooltip("부드럽게 이동(Lerp)할지, 즉시 전환할지")]
    public bool smoothMove = true;

    [Tooltip("목표와의 거리 차이가 이 값보다 작으면 이동 중지 (흔들림 방지)")]
    public float stopThreshold = 0.001f;

    private bool atPointA = true;

    void Start()
    {
        if (pointA != null)
        {
            transform.position = pointA.position;
            transform.rotation = pointA.rotation;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            atPointA = !atPointA;

        Transform targetPoint = atPointA ? pointA : pointB;
        if (targetPoint == null)
            return;

        // 위치 이동
        if (smoothMove)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPoint.position,
                Time.deltaTime * moveSpeed
            );
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPoint.position,
                moveSpeed * Time.deltaTime
            );
        }

        // 목표 위치에 거의 도달하면 정확히 고정 (흔들림 방지)
        if (Vector3.Distance(transform.position, targetPoint.position) < stopThreshold)
            transform.position = targetPoint.position;

        // 회전 보간 (좋다고 하셨으니 그대로 유지)
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetPoint.rotation,
            Time.deltaTime * rotateSpeed
        );
    }
}
