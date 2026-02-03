using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderPrefabFollower : MonoBehaviour
{
    private Transform mainCam;

    // 카메라 기준 상대 위치 / 회전
    private Vector3 cameraLocalPos;
    private Quaternion cameraLocalRot;

    void Start()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("Main Camera not found");
            enabled = false;
            return;
        }

        mainCam = cam.transform;

        // 소환 시점 기준으로 "카메라 기준" 위치/회전 저장
        cameraLocalPos = mainCam.InverseTransformPoint(transform.position);
        cameraLocalRot = Quaternion.Inverse(mainCam.rotation) * transform.rotation;
    }

    // LateUpdate에서 처리 (카메라 이동이 끝난 뒤)
    void LateUpdate()
    {
        if (mainCam == null) return;

        // 항상 카메라 기준 관계를 다시 맞춘다
        transform.position = mainCam.TransformPoint(cameraLocalPos);
        transform.rotation = mainCam.rotation * cameraLocalRot;
    }
}
