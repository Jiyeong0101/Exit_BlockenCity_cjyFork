using System.Collections;
using System.Collections.Generic;
using TetrisGame;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/NormalQuest")]
public class NormalQuest : ScriptableObject
{
    public int questID;            // 퀘스트 고유 ID
    public string questName;       // 퀘스트 이름
    [TextArea] public string description; // 퀘스트 내용
    public int reward;             // 보상

    [Header("목표 조건")]
    public BlockType targetBlockType; // 목표 블럭 타입
    public int targetCount;           // 필요 갯수

    [HideInInspector] public int currentCount; // 현재 진행도

    // 완료 여부
    public bool IsCompleted => currentCount >= targetCount;

    // 진행 퍼센트 (UI 표시용)
    public int ProgressPercent => targetCount > 0 ? (int)((float)currentCount / targetCount * 100f) : 0;

    // 카운트 증가
    public void AddProgress(BlockType type)
    {
        if (type == targetBlockType && !IsCompleted)
        {
            currentCount++;
        }
    }
}