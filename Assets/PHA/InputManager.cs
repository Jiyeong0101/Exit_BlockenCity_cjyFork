using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    //키보드 입력 이벤트
    public event Action OnLeftArrow;
    public event Action OnRightArrow;
    public event Action OnUpArrow;
    public event Action OnDownArrow;

    public event Action OnSpace;

    public event Action OnKeyS;
    public event Action OnKeyA;
    public event Action OnKeyD;
    public event Action OnKeyF;
    public event Action OnKeyC;
    public event Action OnKeyV;

    public event Action OnKey1;
    public event Action OnKey2;

    public event Action OnTab;


    //마우스 입력 이벤트
    public event Action OnMouse1Button;
    public event Action OnMouse2Button;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 씬이 바뀌어도 오브젝트 유지
            DontDestroyOnLoad(gameObject); 
        }
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) OnLeftArrow?.Invoke();
        if (Input.GetKeyDown(KeyCode.RightArrow)) OnRightArrow?.Invoke();
        if (Input.GetKeyDown(KeyCode.DownArrow)) OnUpArrow?.Invoke();
        if (Input.GetKeyDown(KeyCode.UpArrow)) OnDownArrow?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space)) OnSpace?.Invoke();

        if (Input.GetKeyDown(KeyCode.S)) OnKeyS?.Invoke();
        if (Input.GetKeyDown(KeyCode.A)) OnKeyA?.Invoke();
        if (Input.GetKeyDown(KeyCode.D)) OnKeyD?.Invoke();
        if (Input.GetKeyDown(KeyCode.F)) OnKeyF?.Invoke();
        if (Input.GetKeyDown(KeyCode.C)) OnKeyC?.Invoke();
        if (Input.GetKeyDown(KeyCode.V)) OnKeyV?.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1)) OnKey1?.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha2)) OnKey2?.Invoke();

        if (Input.GetKeyDown(KeyCode.Tab)) OnTab?.Invoke();

        if (Input.GetMouseButtonDown(0)) OnMouse1Button?.Invoke();
        if (Input.GetMouseButtonDown(1)) OnMouse2Button?.Invoke();
    }
}