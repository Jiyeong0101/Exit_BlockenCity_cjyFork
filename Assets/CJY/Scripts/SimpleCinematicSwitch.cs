using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCinematicCamera : MonoBehaviour
{
    public Transform pointA;     // 위치 A
    public Transform pointB;     // 위치 B
    public Transform lookTarget; // 회전해서 바라볼 목표

    public float moveSpeed = 2f;     // 위치 이동 속도
    public float rotateSpeed = 2f;   // 회전 속도

    private bool atPointA = true;

    void Update()
    {
        // 누르면 위치 전환
        if (Input.GetKeyDown(KeyCode.P))
        {
            atPointA = !atPointA;
        }

        // 목표 위치 계산
        Transform targetPoint = atPointA ? pointA : pointB;

        // 위치 이동 (부드럽게)
        transform.position = Vector3.Lerp(
            transform.position,
            targetPoint.position,
            Time.deltaTime * moveSpeed
        );

        // 회전: 목표 오브젝트를 바라보도록 보간
        if (lookTarget != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotateSpeed
            );
        }
    }
}
