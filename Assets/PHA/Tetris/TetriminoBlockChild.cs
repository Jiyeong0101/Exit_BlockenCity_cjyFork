using System.Collections;
using System.Collections.Generic;
using TetrisGame;
using Unity.VisualScripting;
using UnityEngine;

public class TetriminoBlockChild : MonoBehaviour
{
    [SerializeField]
    private BlockType blockType;    //블럭 종류

    void Awake()
    {
        blockType = BlockType.None;
    }

    // 블럭 타입 설정
    public void SetBlockType(BlockType blockType)
    {
        this.blockType = blockType;
    }

    // 블럭 머티리얼 설정
    public void SetBlockMaterial(Material material)
    {
        Renderer rend = this.GetComponent<Renderer>();
        if (rend != null && material != null)
            rend.material = material;
    }

    public void BlockLock()
    {
        //타입 블럭 카운트
        TetrisManager.Instance.IncreaseTypeBlockCount(blockType);


        //타입에 따른 스테이더스 영향
    }

    public void DeletBlock()
    {
        // 우호도나 스테이더스 영향

        TetrisManager.Instance.DecreaseTypeBlockCount(blockType);

        Destroy(gameObject);
    }
}
