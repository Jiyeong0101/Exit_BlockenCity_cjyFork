using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraBinder : MonoBehaviour
{
    private static Camera originalCamera;
    private static bool cached = false;

    public static void BindAllCanvas(Camera targetCamera)
    {
        if (targetCamera == null) return;

        var canvases = FindObjectsOfType<Canvas>(true);

        foreach (var canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera ||
                canvas.renderMode == RenderMode.WorldSpace)
            {
                if (!cached)
                    originalCamera = canvas.worldCamera;

                canvas.worldCamera = targetCamera;
            }
        }

        cached = true;
    }

    public static void RestoreAllCanvas()
    {
        if (!cached || originalCamera == null) return;

        var canvases = FindObjectsOfType<Canvas>(true);
        foreach (var canvas in canvases)
        {
            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                canvas.worldCamera = originalCamera;
        }

        cached = false;
    }
}

