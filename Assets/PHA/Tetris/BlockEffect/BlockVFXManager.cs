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

    [FormerlySerializedAs("explodeVFXPrefab")]
    [SerializeField]
    private GameObject lineClearVFXPrefab;


    [SerializeField]
    private GameObject bombDestroyVFXPrefab;


    [SerializeField]
    private GameObject undoVFXPrefab;


    [SerializeField]
    private GameObject lockVFXPrefab;


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


    private void OnEnable()
    {
        TetriminoBlock.OnAnyBlockLocked +=
            HandleBlockLocked;
    }


    private void OnDisable()
    {
        TetriminoBlock.OnAnyBlockLocked -=
            HandleBlockLocked;
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
    // Block Lock
    // =============================================

    public void PlayBlockLock(
        Vector3 worldPosition)
    {
        PlayVFX(
            lockVFXPrefab,
            worldPosition,
            "BlockLock"
        );
    }


    private void HandleBlockLocked(
        TetriminoBlock block)
    {
        if (block == null)
            return;


        TetriminoBlockChild[] children =
            block.GetComponentsInChildren
            <TetriminoBlockChild>();


        foreach (TetriminoBlockChild child
                 in children)
        {
            if (child == null)
                continue;


            PlayBlockLock(
                child.transform.position
            );
        }
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