using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class EffectVisualPlayer : MonoBehaviour
{
    private const string StrongWindPath = "Effects/StrongWindEffect"; //강풍 이펙트
    private const string DustPath = "Effects/DustEffect"; //황사 이펙트
    private const string LightningPath = "Effects/LightningEffect"; //강풍 이펙트
    private const string RainPath = "Effects/RainEffect"; //비 이펙트
    private const string SmogPath = "Effects/SmogEffect"; //안개 이펙트

    private const string OverheatPath = "Effects/OverheatUI"; //과열 UI

    // 1월 얼음 블록 이미지
    public GameObject VisualFreezeBlock(TetriminoBlock block)
    {
        if (block == null)
        {
            Debug.LogWarning("VisualFreezeBlock 호출 시 block이 null!");
            return null;
        }

        // 모든 자식 VFX 찾기
        var vfxList = block.GetComponentsInChildren<TetrisBlockVFX>();
        if (vfxList.Length == 0)
        {
            Debug.LogWarning($"{block.name}의 자식에 TetrisBlockVFX 없음!");
            return block.gameObject;
        }

        // 각 자식 VFX에 적용 + 디버그
        foreach (var vfx in vfxList)
        {
            vfx.SetTextureSlider(1f);
            Debug.Log($"얼음 효과 적용: {vfx.gameObject.name}, TextureSlider = 1");
        }

        return block.gameObject;
    }

    // 2월 강풍 효과 (단발 효과 → 반환 X)
    public GameObject PlayStrongWindEffect()
    {
        var prefab = Resources.Load<GameObject>(StrongWindPath);
        if (prefab == null) return null;

        return Instantiate(prefab);
    }

    // 3월 잔설 효과
    public GameObject SnowfallEffect()
    {
        // 미정: 눈 내리는 프리팹을 반환
        return null;
    }

    // 4,5월 황사 효과 (반복 → IEnumerator 반환)
    public GameObject DustStormEffect()
    {
        var prefab = Resources.Load<GameObject>(DustPath);
        if (prefab == null) return null;

        return Instantiate(prefab);
    }

    // 6월 번개 이펙트 (단발 효과)
    public GameObject PlayLightningEffect()
    {
        var prefab = Resources.Load<GameObject>(LightningPath);
        if (prefab == null) return null;

        return Instantiate(prefab);
    }

    // 7월 비 내리는 효과
    public GameObject PlayRainEffect()
    {
        var prefab = Resources.Load<GameObject>(RainPath);
        if (prefab == null) return null;

        return Instantiate(prefab);
    }

    // 8월 폭염 효과
    public GameObject PlayOverheatWarning(string message = "건축 기계 과열!")
    {
        var prefab = Resources.Load<GameObject>(OverheatPath);
        if (prefab == null) return null;

        var canvas = GameObject.Find("Canvas");
        if (canvas == null) return null;

        var instance = Instantiate(prefab, canvas.transform);

        var text = instance.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = message;

        var rect = instance.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;
        }

        return instance;
    }

    // 10월 건기 블록 파괴 효과
    public void PlayBlockCrumbleEffect(Vector3 position)
    {
        // 미정
    }

    // 11월 스모그 효과
    public GameObject PlaySmogEffect()
    {
        var prefab = Resources.Load<GameObject>(SmogPath);
        if (prefab == null) return null;

        return Instantiate(prefab);
    }
}
