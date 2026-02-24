using System.Collections;
using System.Collections.Generic;
using TetrisGame;
using UnityEngine;

public enum SpecialQuestType
{
    BlockBreak,     // 블럭 파괴
    BlockNoBreak,   // 블럭 파괴 금지

    HeightLimit,    //높이 제한
    HeightAchievement, //높이 달성 
    HeightKeep, //높이 유지 
    HeightSpecialBlock, //특정 높이블럭설치

    InputRestriction, // 입력 제한

    ViewObstruction,  // 시야 방해


    // 이후 다른 타입도 자유롭게 추가 가능
}
public enum TimeType
{
    None,           // 시간 제한 없음
    BlockDropCount, // 블럭이 몇 번 떨어졌는지 기준
    Seconds         // 일정 시간(초) 기준
}
public enum QuestTimeRole
{
    None,           // 시간 없음
    Survival,       // 버티면 성공
    TimeLimit       // 제한 시간 (초과 시 실패)
}

[CreateAssetMenu(fileName = "NewSpecialQuest", menuName = "Quest/SpecialQuest")]
public class SpecialQuestData : ScriptableObject
{
    [Header("기본 조건")]
    public Sprite background;
    public Color sliderColor;
    public int branchID;                // 퀘스트 고유 ID
    public string questName;           // 퀘스트 이름
    [TextArea] public string description; // 퀘스트 내용

    [Header("목표 조건")]
    public SpecialQuestType questType; // 퀘스트 타입

    public BlockType blockType;
    public int targetCount;        // BlockBreak, BlockNoBreak
    public int targetHeight;       // ReachHeight, KeepHeight
    //public int keepDuration;       // KeepHeight

    public string restrictedInput; // InputRestriction
    public float darknessLevel;    // ViewObstruction

    [Header("시간 조건")]
    public TimeType timeType;          // 시간 타입
    public QuestTimeRole timeRole;     // 시간 룰 (0.없음 / 1.서바이벌 / 2.)
    public int timeValue;              // 시간 값 (n번 드랍 또는 n초)

    [Header("보상")]
    public int reward;                 // 보상

    [HideInInspector]
    public bool IsCompleted = false;   // 완료 여부

    // === 헬퍼 메서드 ===
    public bool HasTimeLimit()
    {
        return timeType == TimeType.BlockDropCount || timeType == TimeType.Seconds;
    }

    public string GetTimeDescription()
    {
        switch (timeType)
        {
            case TimeType.BlockDropCount:
                return $"{timeValue}번 블럭 드랍 동안 유지";
            case TimeType.Seconds:
                return $"{timeValue}초 동안 유지";
            default:
                return "시간 제한 없음";
        }
    }
}