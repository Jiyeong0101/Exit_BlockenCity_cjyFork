using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "NewQuest/QuestData")]
public class NewQuestData : ScriptableObject
{
    public int branchID;    //  브런치 번호 (acceptBranch와 일치)
    public string questName;    //  이름
    [TextArea] public string description;   //  퀘스트 설명
    public string reward;   //  보상 설명
    public float progress;  //  진행도 
    public Sprite factionBackgroundImage; // 소속 세력 배경 이미지
}