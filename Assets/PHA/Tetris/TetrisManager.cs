using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TetrisGame;

[System.Serializable]
public class StageTowerSize
{
    public int stage;
    public Vector3Int towerSize = new Vector3Int(4, 8, 4);
}

public class TetrisManager : MonoBehaviour
{
    public static TetrisManager Instance;

    [Header("Tower Layout")]
    public Transform towerLayout;

    public Vector3Int tetrisTowerSize = new Vector3Int(4, 8, 4);
    [Header("Stage Tower Size")]
    public List<StageTowerSize> stageTowerSizes = new List<StageTowerSize>();

    public float fallInterval;
    public TetrisTower tower;
    public TetrisSpawner spawner;
    public TetrisController controller;

    private int[] typeBlockCount = new int[(int)BlockType.None];
    public ScoreUIBinder scoreUIBinder;

    public bool isGameEnded { get; private set; } = false;
    public bool isPaused { get; private set; } = false; // [추가] 일시정지 상태 플래그

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        isGameEnded = false;
        isPaused = false;

        ApplyTowerSize();
        tower.Initialize();

        Vector3 spawnPos = tower.GetSpawnPosition();
        spawner.SetTowerSpawnPosition(spawnPos);
        SpawnNextBlock();

        for (int i = 0; i < typeBlockCount.Length; i++)
        {
            typeBlockCount[i] = 0;
        }
    }

    // [추가] 일시정지 상태 설정 함수
    public void SetPause(bool pause)
    {
        isPaused = pause;
    }

    public void GameClear()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StopGame();
        }

        if (controller != null)
        {
            controller.enabled = false;
        }

        Debug.Log("[Tetris] GAME CLEAR");

        if (scoreUIBinder != null)
        {
            scoreUIBinder.ToggleScoreUI(true, isGameOver: false);
        }
    }

    public void IncreaseTypeBlockCount(BlockType type) => typeBlockCount[(int)type]++;
    public void DecreaseTypeBlockCount(BlockType type)
    {
        typeBlockCount[(int)type]--;
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.UpdateQuestProgress(type);
        }
    }
    public int GetBlockCount(BlockType type) => typeBlockCount[(int)type];
    public int[] GetAllBlockCounts() => (int[])typeBlockCount.Clone();

    public void SpawnNextBlock()
    {
        // [수정] 게임 종료 OR 일시정지 상태면 새 블록 생성 안 함
        if (isGameEnded || isPaused) return;

        spawner.SpawnBlock();
        controller.SetCurrentBlock(spawner.GetTetriminoBlock());
    }

    public void CheckTower()
    {
        // [수정] 게임 종료 OR 일시정지 상태면 검사 차단
        if (isGameEnded || isPaused) return;

        tower.CheckAndDeleteFullLines();
    }

    public void GameOver()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StopGame();
        }

        Debug.Log("[Tetris] GAME OVER");

        if (controller != null)
        {
            controller.enabled = false;
        }

        StageManager.Instance.OverStage();

        if (scoreUIBinder != null)
        {
            scoreUIBinder.ToggleScoreUI(true, isGameOver: true);
        }
    }

    private void ApplyTowerSize()
    {
        int month = Datamanager.Instance.saveData.progress.currentStage;

        foreach (StageTowerSize data in stageTowerSizes)
        {
            if (data.stage == month)
            {
                tetrisTowerSize = data.towerSize;

                if (towerLayout != null)
                {
                    towerLayout.localScale = new Vector3(
                        tetrisTowerSize.x,
                        tetrisTowerSize.y,
                        tetrisTowerSize.z);
                }

                Debug.Log($"Stage {month} Tower Size : {tetrisTowerSize}");
                return;
            }
        }

        Debug.LogWarning($"Stage {month}의 Tower Size가 등록되어 있지 않습니다.");
    }
}