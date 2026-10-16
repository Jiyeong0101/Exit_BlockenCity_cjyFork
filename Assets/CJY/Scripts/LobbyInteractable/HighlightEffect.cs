using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightEffect : MonoBehaviour
{
    [Header("Outline Layer Settings")]
    [Tooltip("아웃라인 효과를 적용할 유니티 레이어 이름입니다.")]
    public string outlineLayerName = "Outline";

    private int originalLayer;
    private int outlineLayer;

    // 추가: 자신과 모든 하위 오브젝트의 Renderer 저장
    private Renderer[] targetRenderers;

    private static readonly int InnerOpacityID =
        Shader.PropertyToID("_InnerOpacity");

    private MaterialPropertyBlock propertyBlock;

    void Awake()
    {
        // 1. 시작할 때 원래 가지고 있던 레이어를 기억해둡니다. (예: "Interactable" 등)
        originalLayer = gameObject.layer;

        // 2. 아웃라인용 레이어의 인덱스를 찾아둡니다.
        outlineLayer = LayerMask.NameToLayer(outlineLayerName);

        if (outlineLayer == -1)
        {
            Debug.LogError($"[HighlightEffect] '{outlineLayerName}' 레이어가 존재하지 않습니다. 유니티 설정(Tags and Layers)에서 레이어를 먼저 추가해주세요.");
        }

        // 추가: 자신을 포함한 모든 하위 오브젝트의 Renderer 가져오기
        targetRenderers = GetComponentsInChildren<Renderer>(true); ;
    }

    public void EnableHighlight()
    {
        if (outlineLayer != -1)
        {
            // 나 자신과 모든 자식 오브젝트의 레이어를 Outline으로 변경
            SetLayerRecursively(gameObject, outlineLayer);
        }

        // 추가: 호버 시 InnerOpacity Y = 1
        SetInnerOpacity(1f);
    }

    public void DisableHighlight()
    {
        if (outlineLayer != -1)
        {
            // 나 자신과 모든 자식 오브젝트의 레이어를 원래 레이어로 복구
            SetLayerRecursively(gameObject, originalLayer);
        }

        // 추가: 호버 해제 시 InnerOpacity Y = 0
        SetInnerOpacity(0f);
    }

    // 자식 오브젝트들까지 모두 레이어를 바꿔주기 위한 함수
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }


    // 추가: 모든 하위 오브젝트의 두 번째 머터리얼 InnerOpacity Y값 변경
    private void SetInnerOpacity(float value)
    {
        if (targetRenderers == null)
            return;

        foreach (Renderer renderer in targetRenderers)
        {
            if (renderer == null)
                continue;

            // 추가: 각 Renderer의 머터리얼 배열 가져오기
            Material[] materials = renderer.materials;

            // 추가: 두 번째 머터리얼이 없는 경우 제외
            if (materials.Length < 2)
                continue;

            // 추가: Element 1 (Outline Shader)
            Material outlineMaterial = materials[1];

            if (outlineMaterial == null ||
                !outlineMaterial.HasProperty(InnerOpacityID))
                continue;

            // 기존 X값 유지
            Vector4 opacity =
                outlineMaterial.GetVector(InnerOpacityID);

            // Y값만 변경
            opacity.y = value;

            outlineMaterial.SetVector(
                InnerOpacityID,
                opacity
            );
        }
    }

}