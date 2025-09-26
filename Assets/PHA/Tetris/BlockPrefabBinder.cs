using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TetrisGame;

[System.Serializable]
public class BlockPrefabEntry
{
    public BlockType type;
    public GameObject prefab;
}

// Project Settings > Script Execution Order 에서 BlockPrefabBinder를
// Default Time 보다 먼저 실행(음수 우선순위) 되도록 설정
public class BlockPrefabBinder : MonoBehaviour
{
    [SerializeField] private List<BlockPrefabEntry> prefabList;

    public static readonly Dictionary<BlockType, GameObject> Prefabs = new();

    private void Awake()
    {
        Prefabs.Clear();
        foreach (var entry in prefabList)
        {
            if (entry.type == BlockType.None) continue; // 안전장치
            if (entry.prefab == null) continue;

            if (!Prefabs.ContainsKey(entry.type))
                Prefabs.Add(entry.type, entry.prefab);
        }
    }
}
