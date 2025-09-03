using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TetrisGame;

[System.Serializable]
public class BlockMaterialEntry
{
    public BlockType type;
    public Material material;
}



//Project Settings > Script Execution Order에서 BlockMaterialBinder를 등록하고
//Default Time보다 먼저 실행되도록 우선순위를 음수 값으로 조정
public class BlockMaterialBinder : MonoBehaviour
{
    [SerializeField]
    private List<BlockMaterialEntry> materialList;

    public static readonly Dictionary<BlockType, Material> Materials = new();

    private void Awake()
    {
        Materials.Clear();
        foreach (var entry in materialList)
        {
            if (!Materials.ContainsKey(entry.type))
            {
                Materials.Add(entry.type, entry.material);
            }
        }
    }
}
