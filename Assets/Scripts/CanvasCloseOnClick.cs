using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCloseOnClick : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Debug.Log("애니메이터를 가져왔습니다.");
    }

    private void OnEnable()
    {
        if (animator != null)
        {
            animator.Rebind();
            Debug.Log("트리거는 기본 상태입니다.");
        }
    }

    public void PlayCloseAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("IsClose");
            Debug.Log("IsClose를 트리거합니다.");
        }
    }

    public void DeactivateSelf()
    {
        gameObject.SetActive(false);
        Debug.Log("Active가 false가 되었습니다.");
    }
}
