using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCinematicCamera2 : MonoBehaviour
{
    public TetrisController tetrisController;

    public Transform[] cameraPoints; // Front, Right, Back, Left
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    public CameraDir currentDir = CameraDir.Front;

    void Start()
    {
        ApplyImmediate();
        NotifyDirChanged();
    }

    void Update()
    {
        HandleInput();
        MoveCamera();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            RotateLeft();

        if (Input.GetKeyDown(KeyCode.E))
            RotateRight();
    }

    void RotateLeft()
    {
        currentDir = (CameraDir)(((int)currentDir + 3) % 4);
        tetrisController.SetCameraDir(currentDir);
    }

    void RotateRight()
    {
        currentDir = (CameraDir)(((int)currentDir + 1) % 4);
        tetrisController.SetCameraDir(currentDir);
    }

    void MoveCamera()
    {
        Transform target = cameraPoints[(int)currentDir];

        transform.position = Vector3.Lerp(
            transform.position,
            target.position,
            Time.deltaTime * moveSpeed
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            target.rotation,
            Time.deltaTime * rotateSpeed
        );
    }

    void ApplyImmediate()
    {
        Transform target = cameraPoints[(int)currentDir];
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    void NotifyDirChanged()
    {
        if (tetrisController != null)
            tetrisController.SetCameraDir(currentDir);
    }

}

