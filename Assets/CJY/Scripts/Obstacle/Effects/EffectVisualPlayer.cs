using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class EffectVisualPlayer : MonoBehaviour
{
    //[Header("==== ����Ʈ ������ ���� ====")]
    //[Tooltip("��ǳ ����Ʈ (���� ��� Resources/Effects/StrongWindEffect���� �ε�)")]
    //[SerializeField] private GameObject strongWindPrefab;

    //[Tooltip("Ȳ�� ����Ʈ (���� ��� Resources/Effects/DustEffect���� �ε�)")]
    //[SerializeField] private GameObject dustPrefab;

    //[Tooltip("���� ����Ʈ (���� ��� Resources/Effects/LightningEffect���� �ε�)")]
    //[SerializeField] private GameObject lightningPrefab;

    //[Tooltip("�� ����Ʈ (���� ��� Resources/Effects/RainEffect���� �ε�)")]
    //[SerializeField] private GameObject rainPrefab;

    //[Tooltip("����� ����Ʈ (���� ��� Resources/Effects/SmogEffect���� �ε�)")]
    //[SerializeField] private GameObject smogPrefab;

    //[Header("==== UI ������ ====")]
    //[Tooltip("���� ��� UI (���� ��� Resources/Effects/OverheatUI���� �ε�)")]
    //[SerializeField] private GameObject overheatUIPrefab;

    //[Header("==== ��� ���־� ���� ====")]
    //[Tooltip("���� ��� ȿ�� ���� (0~1)")]
    //[SerializeField, Range(0f, 1f)] private float iceEffect = 0.5f;

    // �⺻ ���ҽ� ��� (�����)
    private const string StrongWindPath = "Effects/StrongWindEffect";
    private const string DustPath = "Effects/DustEffect";
    private const string LightningPath = "Effects/LightningEffect";
    private const string RainPath = "Effects/RainEffect";
    private const string SmogPath = "Effects/SmogEffect";
    private const string OverheatPath = "Effects/OverheatUI";

    [Header("��� ���� �̹���")]
    [SerializeField] private float IceEffect = 0.5f;

    // 1�� ���� ��� �̹���
    public GameObject VisualFreezeBlock(TetriminoBlock block)
    {
        if (block == null)
        {
            Debug.LogWarning("VisualFreezeBlock ȣ�� �� block�� null!");
            return null;
        }

        // ��� �ڽ� VFX ã��
        var vfxList = block.GetComponentsInChildren<TetrisBlockVFX>();
        if (vfxList.Length == 0)
        {
            Debug.LogWarning($"{block.name}�� �ڽĿ� TetrisBlockVFX ����!");
            return block.gameObject;
        }

        // �� �ڽ� VFX�� ���� + �����
        foreach (var vfx in vfxList)
        {
            vfx.SetTextureSlider(IceEffect);
            Debug.Log($"���� ȿ�� ����: {vfx.gameObject.name}, TextureSlider = 0.5");
        }

        return block.gameObject;
    }

    // 2�� ��ǳ ȿ�� (�ܹ� ȿ�� �� ��ȯ X)
    public GameObject PlayStrongWindEffect()
    {
        //var prefab = strongWindPrefab ?? Resources.Load<GameObject>(StrongWindPath);
        var prefab = Resources.Load<GameObject>(StrongWindPath);

        if (prefab == null) return null;

        return Instantiate(prefab);
    }

    // 3�� �ܼ� ȿ��
    public GameObject SnowfallEffect()
    {
        // ����: �� ������ �������� ��ȯ
        return null;
    }

    // 4,5�� Ȳ�� ȿ�� (�ݺ� �� IEnumerator ��ȯ)
    public GameObject DustStormEffect()
    {
        //var prefab = dustPrefab ?? Resources.Load<GameObject>(DustPath);
        var prefab = Resources.Load<GameObject>(DustPath);
        if (prefab == null) return null;

        return Instantiate(prefab);
    }

    // 6�� ���� ����Ʈ (�ܹ� ȿ��)
    public GameObject PlayLightningEffect()
    {
        //var prefab = lightningPrefab ?? Resources.Load<GameObject>(LightningPath);
        var prefab = Resources.Load<GameObject>(LightningPath);
        if (prefab == null) return null;

        return Instantiate(prefab);
    }

    // 7�� �� ������ ȿ��
    public GameObject PlayRainEffect()
    {
        //var prefab = rainPrefab ?? Resources.Load<GameObject>(RainPath);
        var prefab = Resources.Load<GameObject>(RainPath);
        if (prefab == null) return null;

        return Instantiate(prefab);
    }

    // 8�� ���� ȿ��
    public GameObject PlayOverheatWarning(string message = "���� ��� ����!")
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

    // 10�� �Ǳ� ��� �ı� ȿ��
    public void PlayBlockCrumbleEffect(Vector3 position)
    {
        // ����
    }

    // 11�� ����� ȿ��
    public GameObject PlaySmogEffect()
    {
        //var prefab = smogPrefab ?? Resources.Load<GameObject>(SmogPath);
        var prefab = Resources.Load<GameObject>(SmogPath);
        if (prefab == null) return null;

        return Instantiate(prefab);
    }
}
