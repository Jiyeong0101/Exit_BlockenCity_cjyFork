using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float startGameTime = 150f;   // Inspector에서 설정
    public float gameTime;

    public bool isGameEnded { get; private set; } = false;
    public bool isPaused { get; private set; } = false;

    [HideInInspector] public ScoreUIBinder scoreManager;

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
    }

    void Update()
    {
        // 일시정지 상태이거나 게임이 종료되었으면 타이머 멈춤
        if (isGameEnded || isPaused) return;

        if (gameTime > 0f)
        {
            gameTime -= Time.deltaTime;

            if (gameTime <= 0f)
            {
                gameTime = 0f;
                TimeOver();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
    }

    public void StopGame()
    {
        isGameEnded = true;
    }

    public void TimeOver()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("일차 종료!");

        if (TetrisManager.Instance != null)
        {
            TetrisManager.Instance.GameClear();
        }

        if (scoreManager != null)
        {
            scoreManager.ToggleScoreUI(true, isGameOver: false);
        }
    }
    public void ResetGame()
    {
        gameTime = startGameTime;
        isGameEnded = false;
        isPaused = false;
    }

}