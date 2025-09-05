using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 방해물 조건 평가 및 효과 적용을 위한 최소한의 게임 상태 정보
/// </summary>
public class ObstacleGameState
{
    public ObstacleType SelectedObstacle { get; private set; }
    public EffectVisualPlayer VisualPlayer { get; private set; }
    public InputBlocker InputBlocker { get; private set; }
    public TetrisController TetrisController { get; private set; }

    // 블록 분리
    public TetriminoBlock SpawnedBlock { get; set; }  // 스폰 시점 블록
    public TetriminoBlock LockedBlock { get; set; }   // 설치 시점 블록

    public float CurrentDropSpeed { get; set; }

    public System.Action<Coroutine> RegisterCoroutine;
    public System.Action<GameObject> RegisterObject;
    public Func<IEnumerator, Coroutine> StartManagedCoroutine;

    public ObstacleGameState(ObstacleType selectedObstacle,
                             EffectVisualPlayer visualPlayer,
                             InputBlocker inputBlocker,
                             TetrisController tetrisController)
    {
        SelectedObstacle = selectedObstacle;
        VisualPlayer = visualPlayer;
        InputBlocker = inputBlocker;
        TetrisController = tetrisController;
        CurrentDropSpeed = 1f;
    }

    public void UpdateSelectedObstacle(ObstacleType type)
    {
        SelectedObstacle = type;
    }

    public bool CheckCondition(ObstacleType required)
    {
        return SelectedObstacle == required;
    }
}
