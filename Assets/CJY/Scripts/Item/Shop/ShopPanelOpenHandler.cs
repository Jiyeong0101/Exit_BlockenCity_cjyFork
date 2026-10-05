using System.Collections;
using UnityEngine;

public class ShopPanelOpenHandler : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private ShopCharacterPresenter characterPresenter;


    private Coroutine openCoroutine;


    private void OnEnable()
    {
        if (openCoroutine != null)
        {
            StopCoroutine(openCoroutine);
        }

        openCoroutine =
            StartCoroutine(InitializeShopRoutine());
    }


    private IEnumerator InitializeShopRoutine()
    {
        // 상점 UI와 자식 오브젝트들이
        // 완전히 활성화될 때까지 1프레임 기다림
        yield return null;


        if (characterPresenter != null)
        {
            characterPresenter.PlayShopGreeting();
        }


        openCoroutine = null;
    }


    private void OnDisable()
    {
        if (openCoroutine != null)
        {
            StopCoroutine(openCoroutine);

            openCoroutine = null;
        }
    }
}