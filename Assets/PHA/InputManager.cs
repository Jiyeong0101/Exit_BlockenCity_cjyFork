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

    private HashSet<string> restrictedInputs = new HashSet<string>();
    public bool IsInputRestricted => restrictedInputs.Count > 0;

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

    public void SetRestrictedInputs(IEnumerable<string> inputs)
    {
        restrictedInputs.Clear();

        foreach (var input in inputs)
        {
            restrictedInputs.Add(input.Trim());
        }
    }

    public void ClearRestrictedInputs()
    {
        restrictedInputs.Clear();
    }

    private bool IsRestricted(string inputName)
    {
        return restrictedInputs.Contains(inputName);
    }

    // Update is called once per frame
    void Update()   //꺅 더러워졌어~
    {
    if (Input.GetKeyDown(KeyCode.LeftArrow) && !IsRestricted("LeftArrow"))
        OnLeftArrow?.Invoke();

    if (Input.GetKeyDown(KeyCode.RightArrow) && !IsRestricted("RightArrow"))
        OnRightArrow?.Invoke();

    if (Input.GetKeyDown(KeyCode.UpArrow) && !IsRestricted("UpArrow"))
        OnDownArrow?.Invoke();

    if (Input.GetKeyDown(KeyCode.DownArrow) && !IsRestricted("DownArrow"))  //여기 테트리스때문에 위아레 바꿔둔거지??
        OnUpArrow?.Invoke();

    if (Input.GetKeyDown(KeyCode.Space) && !IsRestricted("Space"))
        OnSpace?.Invoke();

    if (Input.GetKeyDown(KeyCode.S) && !IsRestricted("S"))
        OnKeyS?.Invoke();

    if (Input.GetKeyDown(KeyCode.A) && !IsRestricted("A"))
        OnKeyA?.Invoke();

    if (Input.GetKeyDown(KeyCode.D) && !IsRestricted("D"))
        OnKeyD?.Invoke();

    if (Input.GetKeyDown(KeyCode.F) && !IsRestricted("F"))
        OnKeyF?.Invoke();

    if (Input.GetKeyDown(KeyCode.C) && !IsRestricted("C"))
        OnKeyC?.Invoke();

    if (Input.GetKeyDown(KeyCode.V) && !IsRestricted("V"))
        OnKeyV?.Invoke();

    if (Input.GetKeyDown(KeyCode.Alpha1) && !IsRestricted("1"))
        OnKey1?.Invoke();

    if (Input.GetKeyDown(KeyCode.Alpha2) && !IsRestricted("2"))
        OnKey2?.Invoke();

    if (Input.GetKeyDown(KeyCode.Tab) && !IsRestricted("Tab"))
        OnTab?.Invoke();

    if (Input.GetMouseButtonDown(0) && !IsRestricted("Mouse1"))
        OnMouse1Button?.Invoke();

    if (Input.GetMouseButtonDown(1) && !IsRestricted("Mouse2"))
        OnMouse2Button?.Invoke();
    }
}