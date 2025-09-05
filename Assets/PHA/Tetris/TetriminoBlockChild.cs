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

    //추가
    //Child가 삭제될 때 자기 좌표를 알아야 하므로 필드와 세터 메서드 추가
    private Vector3Int gridPosition;
    public Vector3Int GridPosition => gridPosition;
    public void SetGridPosition(Vector3Int pos)
    {
        gridPosition = pos;
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

        // 추가
        // 타워에서도 제거
        var tower = TetrisManager.Instance.tower;
        if (tower != null)
        {
            tower.RemoveBlockFromTower(GridPosition);
        }

        Destroy(gameObject);
    }
}
