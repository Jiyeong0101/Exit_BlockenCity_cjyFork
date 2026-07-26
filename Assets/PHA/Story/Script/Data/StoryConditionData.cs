using System;
using UnityEngine;

public enum StoryConditionType
{
    None,

    StoryCompleted,
    StoryNotCompleted,

    FactionIntroduced,
    FactionNotIntroduced,

    RelationshipAtLeast,
    RelationshipAtMost,

    ChoiceEquals
}

[Serializable]
public class StoryConditionData
{
    [SerializeField]
    private StoryConditionType conditionType;

    [Tooltip("스토리 ID, 세력 ID, 선택 결과 키 등에 사용")]
    [SerializeField]
    private string key;

    [Tooltip("문자열 비교 조건에 사용할 값")]
    [SerializeField]
    private string value;

    [Tooltip("호감도 비교 조건에 사용할 값")]
    [SerializeField]
    private int intValue;

    public StoryConditionType ConditionType =>
        conditionType;

    public string Key => key;
    public string Value => value;
    public int IntValue => intValue;
}