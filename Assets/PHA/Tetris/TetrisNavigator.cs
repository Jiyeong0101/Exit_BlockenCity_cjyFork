using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisNavigator : MonoBehaviour
{
    [SerializeField] private Material ghostMaterial;   // 회색 60% 투명 (인스펙터로 넣어도 됨)
    private TetriminoBlock owner;                      // 내가 따라다닐 실제 블럭
    private Transform ghostRoot;                       // 고스트 자식들을 붙일 루트
    private readonly List<Transform> ghostCubes = new();

    [SerializeField, Range(0.1f, 2f)] private float ghostScale = 0.8f; // 인스펙터에서 조절 가능

    // 외부에서 소유자/머티리얼 넘겨 초기화
    public void Initialize(TetriminoBlock owner, Material mat = null)
    {
        this.owner = owner;
        ghostMaterial = mat ?? ghostMaterial ?? CreateDefaultGhostMaterial();
        BuildGhostChildren();
    }

    // 고스트 자식들을 실제 블럭 자식 개수만큼 만든다 (큐브 프리미티브)
    private void BuildGhostChildren()
    {
        if (ghostRoot != null) Destroy(ghostRoot.gameObject);
        ghostRoot = new GameObject("GhostRoot").transform;
        ghostRoot.SetParent(transform, false);
        ghostCubes.Clear();

        var children = new List<Transform>();
        foreach (Transform child in owner.transform)
        {
            children.Add(child);
        }

        for (int i = 0; i < children.Count - 1; i++) // 마지막은 건너뜀
        {
            var src = children[i];
            //Debug.Log($"[DEBUG] src.name = {src.name}, src.localScale = {src.localScale}");

            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var col = go.GetComponent<Collider>();
            if (col) col.enabled = false;

            var rend = go.GetComponent<Renderer>();
            if (rend) rend.sharedMaterial = ghostMaterial;

            var t = go.transform;
            t.SetParent(ghostRoot, false);

            t.localScale = src.localScale * ghostScale;
            Debug.Log($"[DEBUG] ghost cube scale = {t.localScale}");

            ghostCubes.Add(t);
        }
    }

    private void LateUpdate()
    {
        if (owner == null) return;

        // 1) 최대 낙하 거리 계산
        int fall = ComputeFallDistance();

        // 2) 고스트 자식들의 위치를 소유자 자식 위치 + (아래로 fall)로 세팅
        int i = 0;
        foreach (Transform src in owner.transform)
        {
            if (i >= ghostCubes.Count) break;
            ghostCubes[i].position = src.position + Vector3.down * fall;
            i++;
        }
    }

    // 바닥/설치 블럭에 닿기 직전까지의 거리 계산 (실제 블럭은 안 움직임)
    private int ComputeFallDistance()
    {

        // 타워 높이만큼만 탐색
        int maxProbe = TetrisManager.Instance.tetrisTowerSize.y + 2; // Height는 아래에서 노출함
        for (int d = 1; d <= maxProbe; d++)
        {
            if (WouldCollide(d)) return d - 1;
        }
        return 0;
    }

    // d칸 아래로 가면 충돌/경계 밖인지 검사
    private bool WouldCollide(int d)
    {
        var tower = TetrisManager.Instance.tower;
        foreach (Transform src in owner.transform)
        {
            Vector3 world = src.position + Vector3.down * d;
            Vector3 local = world - tower.transform.position;
            Vector3Int cell = new Vector3Int(
                Mathf.RoundToInt(local.x),
                Mathf.RoundToInt(local.y),
                Mathf.RoundToInt(local.z)
            );

            if (!tower.IsInsideTower(cell)) return true;
            if (tower.IsFilled(cell)) return true;
        }
        return false;
    }

    // 회색 머티리얼 생성
    private Material CreateDefaultGhostMaterial()
    {
        var mat = new Material(Shader.Find("Standard"));
        mat.color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
        // 투명 렌더링 모드 설정
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
        return mat;
    }
}
