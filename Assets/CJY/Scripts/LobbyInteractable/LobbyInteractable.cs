using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HighlightEffect))]
public class LobbyInteractable : MonoBehaviour, IInteractable
{
    public string objectName;

    [Header("UI 설정")]
    [Tooltip("마우스 호버 시 표시될 화살표 UI 오브젝트입니다.")]
    public GameObject arrowUI;

    [Header("사운드 설정")]
    [SerializeField] private AudioClip clickSound;
    [Tooltip("체크하면 클릭한 위치(3D 거리)에 따라 소리가 다르게 들립니다.")]
    [SerializeField] private bool use3DSound = false;

    [Header("클릭 시 실행할 이벤트")]
    public UnityEvent onClickEvent;

    private HighlightEffect highlightEffect;

    private void Start()
    {
        highlightEffect = GetComponent<HighlightEffect>();

        // 시작 시 화살표 UI 숨김
        if (arrowUI != null)
        {
            arrowUI.SetActive(false);
        }
    }

    public void OnHoverEnter()
    {
        // 1. 아웃라인 하이라이트 켜기
        //if (highlightEffect != null)
        //    highlightEffect.EnableHighlight();

        // 2. 화살표 UI 켜기
        if (arrowUI != null)
            arrowUI.SetActive(true);
    }

    public void OnHoverExit()
    {
        // 1. 아웃라인 하이라이트 끄기
        //if (highlightEffect != null)
        //    highlightEffect.DisableHighlight();

        // 2. 화살표 UI 끄기
        if (arrowUI != null)
            arrowUI.SetActive(false);
    }

    public void OnClick()
    {
        // 1. 효과음 재생
        PlayClickSound();

        // 2. 기존 클릭 이벤트 실행
        onClickEvent?.Invoke();
    }

    /// <summary>
    /// 클릭 시 SFXManager를 통해 효과음을 재생
    /// </summary>
    private void PlayClickSound()
    {
        if (clickSound == null || SFXManager.Instance == null) return;

        if (use3DSound)
        {
            // 오브젝트 위치에서 3D 입체 음향으로 재생
            SFXManager.Instance.PlaySFXAtPosition(clickSound, transform.position);
        }
        else
        {
            // 거리 상관없이 동일한 크기(2D)로 재생
            SFXManager.Instance.PlaySFX(clickSound);
        }
    }
}