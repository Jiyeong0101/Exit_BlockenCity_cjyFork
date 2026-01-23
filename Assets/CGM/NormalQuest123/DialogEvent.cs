using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEvent : MonoBehaviour
{
    public DialogUI dialogUI;

    public void Event10()
    {
        DialogUI.Instance.StartDialog(10);
    }
    public void Event1000()
    {
        DialogUI.Instance.StartDialog(1000);
    }

    public void Event20()
    {
        DialogUI.Instance.StartDialog(20);
    }

    public void Event30()
    {
        DialogUI.Instance.StartDialog(30);
    }

    public void Event40()
    {
        DialogUI.Instance.StartDialog(40);
    }

    public void Event50()
    {
        DialogUI.Instance.StartDialog(50);
    }

    public void Event60()
    {
        DialogUI.Instance.StartDialog(60);
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