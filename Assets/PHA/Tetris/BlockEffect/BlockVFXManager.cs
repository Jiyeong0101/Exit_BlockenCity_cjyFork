using UnityEngine;
using UnityEngine.Serialization;

public class BlockVFXManager : MonoBehaviour
{
    public static BlockVFXManager Instance
    {
        get;
        private set;
    }


    [Header("Block VFX")]

    // 기존 explodeVFXPrefab Inspector 연결 유지
    [FormerlySerializedAs("explodeVFXPrefab")]
    [SerializeField]
    private GameObject lineClearVFXPrefab;


    [SerializeField]
    private GameObject bombDestroyVFXPrefab;


    [SerializeField]
    private GameObject undoVFXPrefab;


    private void Awake()
    {
        if (Instance != null &&
            Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
    }


    // =============================================
    // Line Clear
    // =============================================

    public void PlayLineClear(
        Vector3 worldPosition)
    {
        PlayVFX(
            lineClearVFXPrefab,
            worldPosition,
            "LineClear"
        );
    }


    // =============================================
    // Bomb
    // =============================================

    public void PlayBombDestroy(
        Vector3 worldPosition)
    {
        PlayVFX(
            bombDestroyVFXPrefab,
            worldPosition,
            "BombDestroy"
        );
    }


    // =============================================
    // Undo
    // =============================================

    public void PlayUndo(
        Vector3 worldPosition)
    {
        PlayVFX(
            undoVFXPrefab,
            worldPosition,
            "Undo"
        );
    }


    // =============================================
    // Common
    // =============================================

    private void PlayVFX(
        GameObject prefab,
        Vector3 worldPosition,
        string effectName)
    {
        if (prefab == null)
        {
            Debug.LogWarning(
                $"[BlockVFXManager] " +
                $"{effectName} VFX Prefab이 연결되지 않았습니다."
            );

            return;
        }


        Instantiate(
            prefab,
            worldPosition,
            prefab.transform.rotation
        );
    }
}