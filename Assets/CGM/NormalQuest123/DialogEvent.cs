using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEvent : MonoBehaviour
{
    public DialogUI dialogUI;

    public void Evnet10()
    {
        dialogUI.StartDialog(10); // branch 2 시작
    }
    public void Evnet1000()
    {
        dialogUI.StartDialog(1000); // branch 2 시작
    }
    public void accept()
    {
        Debug.Log("수락"); // branch 2 시작
    }
    public void refuse()
    {
        Debug.Log("거절"); // branch 2 시작
    }
}