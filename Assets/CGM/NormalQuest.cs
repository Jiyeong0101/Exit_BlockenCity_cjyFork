using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/NormalQuest")]
public class NormalQuest : ScriptableObject
{
    public int questID;            // 퀘스트 고유 ID
    public string questName;       // 퀘스트 이름
    [TextArea] public string description; // 퀘스트 내용
    public int progress;           // 진행도 (0 ~ 목표치)
    public int reward;          // 보상 설명

    // 진행 상태 체크
    public bool IsCompleted => progress >= 100; // 예시: 100% 기준 완료
}