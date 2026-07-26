using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelClickSound : MonoBehaviour
{
    [Header("클릭 시 재생할 효과음")]
    [SerializeField] private AudioClip clickSound;

    [Header("3D 공간 음향 적용 여부")]
    [Tooltip("체크하면 클릭한 위치(거리)에 따라 소리가 다르게 들립니다.")]
    [SerializeField] private bool use3DSound = false;

    // 마우스로 이 3D 모델을 클릭했을 때 유니티가 자동으로 실행하는 함수
    private void OnMouseDown()
    {
        if (clickSound == null || SFXManager.Instance == null) return;

        if (use3DSound)
        {
            // 모델이 있는 3D 위치에서 소리 발생
            SFXManager.Instance.PlaySFXAtPosition(clickSound, transform.position);
        }
        else
        {
            // 거리 상관없이 화면에 똑같은 크기로 소리 발생
            SFXManager.Instance.PlaySFX(clickSound);
        }
    }
}
