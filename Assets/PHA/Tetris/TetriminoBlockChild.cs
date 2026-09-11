using System.Collections;
using System.Collections.Generic;
using TetrisGame;
using Unity.VisualScripting;
using UnityEngine;

public class TetriminoBlockChild : MonoBehaviour
{
    public BlockType BlockType;

    private bool isDestroyed = false;

    [SerializeField]
    private BlockType blockType;    //블럭 종류

    [SerializeField] private Transform visualRoot; // 메시가 달린 오브젝트(프리팹 내부)
    private Quaternion initialWorldRotation;

    public bool PendingDestroy { get; private set; }

    [SerializeField]
    //private string isChangingPropertyName = "IsChanging";

    private Renderer[] cachedRenderers;

    private bool isLineClearEffectPlaying = false;

    void Awake()
    {
        blockType = BlockType.None;

        if (visualRoot == null) visualRoot = transform; // 없으면 자기 자신
        initialWorldRotation = visualRoot.rotation;     // 스폰 시 월드 회전 저장

        // Line Clear Effect
        cachedRenderers =
            GetComponentsInChildren<Renderer>(true);
    }

    void LateUpdate()
    {
        // 스폰 시 회전으로 고정 (어떻게 돌려도 텍스처 안 뒤집힘)
        visualRoot.rotation = initialWorldRotation;
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

    public void DeletBlock() // 살짝 수정
    {
        if (isDestroyed) // 중복 호출 방지
            return;
        isDestroyed = true;
        // 우호도나 스테이더스 영향


        ProcessDestroyData();


        PendingDestroy = true;

        gameObject.SetActive(false);

        Destroy(gameObject);
    }

    public void ShiftDownOneCell()
    {
        gridPosition += Vector3Int.down;
    }

    private void SetIsChanging(bool value)
    {
        if (cachedRenderers == null)
            return;


        foreach (Renderer rend in cachedRenderers)
        {
            if (rend == null)
                continue;


            // 중요:
            // sharedMaterial이 아니라 material을 사용한다.
            // 이 Renderer만 사용하는 Material Instance가 생성된다.
            Material[] materials = rend.materials;


            foreach (Material mat in materials)
            {
                if (mat == null)
                    continue;


                if (value)
                {
                    mat.EnableKeyword("_ISCHANGING");
                }
                else
                {
                    mat.DisableKeyword("_ISCHANGING");
                }
            }
        }
    }

    private void ProcessDestroyData()
    {
        if (SpecialQuestManager.Instance != null)
        {
            SpecialQuestManager.Instance
                .OnBlockDestroyed(blockType);
        }


        if (TetrisManager.Instance != null)
        {
            TetrisManager.Instance
                .DecreaseTypeBlockCount(blockType);


            var tower =
                TetrisManager.Instance.tower;

            if (tower != null)
            {
                tower.RemoveBlockFromTower(
                    GridPosition
                );
            }
        }
    }

    public void PlayLineClearEffect()
    {
        if (isDestroyed)
            return;

        if (isLineClearEffectPlaying)
            return;


        isLineClearEffectPlaying = true;


        // Shader 변화
        SetIsChanging(true);


        // 현재 블록 위치에서 폭발 VFX 생성
        if (BlockVFXManager.Instance != null)
        {
            BlockVFXManager.Instance.PlayExplode(
                transform.position
            );
        }
    }
}

