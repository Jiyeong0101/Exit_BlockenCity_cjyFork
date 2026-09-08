using UnityEngine;

public class BlockVFXManager : MonoBehaviour
{
    public static BlockVFXManager Instance { get; private set; }

    [Header("Block VFX")]
    [SerializeField]
    private GameObject explodeVFXPrefab;


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


    public void PlayExplode(Vector3 worldPosition)
    {
        if (explodeVFXPrefab == null)
        {
            Debug.LogWarning(
                "[BlockVFXManager] " +
                "Explode VFX Prefab이 연결되어 있지 않습니다."
            );

            return;
        }

        Instantiate(
            explodeVFXPrefab,
            worldPosition,
            explodeVFXPrefab.transform.rotation
        );
    }


#if UNITY_EDITOR

    [ContextMenu("Test Explode VFX")]
    private void TestExplodeVFX()
    {
        PlayExplode(transform.position);
    }

#endif
}