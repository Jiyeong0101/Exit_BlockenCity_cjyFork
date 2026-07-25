using System;
using UnityEngine;

[Serializable]
public class StoryChoiceData
{
    [TextArea(2, 4)]
    [SerializeField]
    private string choiceText;

    [Tooltip("이 선택지를 골랐을 때 이동할 노드 ID")]
    [SerializeField]
    private string targetNodeId;

    [Header("선택 결과")]

    [Tooltip("선택 결과를 저장할 키. 비워도 됩니다.")]
    [SerializeField]
    private string resultKey;

    [Tooltip("선택 결과로 저장할 값. 비워도 됩니다.")]
    [SerializeField]
    private string resultValue;

    [Header("선택지 표시 조건")]

    [Tooltip("이 선택지에 해금 조건을 사용할지 여부")]
    [SerializeField]
    private bool useCondition;

    [Tooltip("선택지 해금 여부를 확인할 조건 키")]
    [SerializeField]
    private string requiredKey;

    [Tooltip("선택지 해금에 필요한 값")]
    [SerializeField]
    private string requiredValue = "True";

    [Tooltip("잠긴 선택지를 화면에서 완전히 숨길지 여부")]
    [SerializeField]
    private bool hideWhenLocked = true;

    [Tooltip("잠긴 선택지를 보여줄 때 사용할 문구. 비어 있으면 원래 문구를 표시합니다.")]
    [SerializeField]
    private string lockedText;

    public string ChoiceText => choiceText;
    public string TargetNodeId => targetNodeId;

    public string ResultKey => resultKey;
    public string ResultValue => resultValue;

    public bool UseCondition => useCondition;
    public string RequiredKey => requiredKey;
    public string RequiredValue => requiredValue;
    public bool HideWhenLocked => hideWhenLocked;
    public string LockedText => lockedText;
}