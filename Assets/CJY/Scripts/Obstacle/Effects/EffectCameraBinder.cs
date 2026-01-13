using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCameraBinder : MonoBehaviour
{
    private IEnumerator Start()
    {
        // 카메라 생성/활성화까지 대기
        yield return null;

        BindToMainCamera(gameObject);
    }

    public void BindToMainCamera(GameObject effectRoot)
    {
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogWarning("Main Camera를 찾지 못했습니다.");
            return;
        }

        var canvases = effectRoot.GetComponentsInChildren<Canvas>(true);
        foreach (var canvas in canvases)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = mainCam;
        }
    }
}

