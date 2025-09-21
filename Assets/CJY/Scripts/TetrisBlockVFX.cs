using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TetrisBlockVFX : MonoBehaviour
{
    [Header("Metallic Map 리스트")]
    [SerializeField] private Texture2D[] metallicMaps;

    [Header("선택할 Metallic Map 인덱스 (-1 = 없음)")]
    [SerializeField] private int selectedMapIndex = -1;

    [Header("Metallic 값 (0=비금속, 1=완전 금속)")]
    [Range(0f, 1f)]
    [SerializeField] private float metallicValue = 0.0f;

    [Header("Smoothness 값 (0=거칠음, 1=매끄러움)")]
    [Range(0f, 1f)]
    [SerializeField] private float smoothnessValue = 0.5f;

    private MeshRenderer meshRenderer;
    private Material materialInstance;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // 런타임에서는 인스턴스 머터리얼 사용 → 인게임에서 바로 반영
        materialInstance = meshRenderer.material;
        ApplyAll();
    }

    private void Update()
    {
        // 플레이 중에도 값 변경 시 반영
        ApplyAll();
    }

    // 에디터에서 Inspector 값 바꿀 때 바로 반영
    private void OnValidate()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
#if UNITY_EDITOR
            // Edit Mode에서는 sharedMaterial 사용 → 메모리 누수 방지
            materialInstance = meshRenderer.sharedMaterial;
#else
            materialInstance = meshRenderer.material;
#endif
            ApplyAll();
        }
    }

    private void ApplyAll()
    {
        ApplyMetallicMap();
        ApplyMetallicValue();
        ApplySmoothnessValue();
    }

    private void ApplyMetallicMap()
    {
        if (materialInstance == null) return;

        Texture map = null;
        if (selectedMapIndex >= 0 && selectedMapIndex < metallicMaps.Length)
            map = metallicMaps[selectedMapIndex];

        // URP Lit Shader에서 사용하는 프로퍼티
        if (materialInstance.HasProperty("_MetallicSpecGlossMap"))
            materialInstance.SetTexture("_MetallicSpecGlossMap", map);

        if (map != null)
            materialInstance.EnableKeyword("_METALLICSPECGLOSSMAP");
        else
            materialInstance.DisableKeyword("_METALLICSPECGLOSSMAP");
    }

    private void ApplyMetallicValue()
    {
        if (materialInstance != null && materialInstance.HasProperty("_Metallic"))
            materialInstance.SetFloat("_Metallic", Mathf.Clamp01(metallicValue));
    }

    private void ApplySmoothnessValue()
    {
        if (materialInstance != null && materialInstance.HasProperty("_Smoothness"))
            materialInstance.SetFloat("_Smoothness", Mathf.Clamp01(smoothnessValue));
    }

    // 외부에서 값 변경용
    public void SetMetallicMap(int index) => selectedMapIndex = index;
    public void SetMetallicValue(float value) => metallicValue = Mathf.Clamp01(value);
    public void SetSmoothness(float value) => smoothnessValue = Mathf.Clamp01(value);
}
