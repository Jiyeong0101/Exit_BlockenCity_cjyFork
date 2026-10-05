using System;
using UnityEngine;

[Serializable]
public class StageData
{
    // 이번 스테이지에서 획득한 재화
    public int earnedMoney = 0;

    // 이번 스테이지에서 변화한 세력 우호도
    public float danwolDelta = 0;
    public float yaseoDelta = 0;
    public float macheonDelta = 0;
    public float hongryeonDelta = 0;
    public float JeonSangYeonDelta = 0;


    public void AddRelationship(
        RelationshipType type,
        float amount)
    {
        switch (type)
        {
            case RelationshipType.Danwol:
                danwolDelta += amount;
                break;

            case RelationshipType.Yaseo:
                yaseoDelta += amount;
                break;

            case RelationshipType.Macheon:
                macheonDelta += amount;
                break;

            case RelationshipType.Hongryeon:
                hongryeonDelta += amount;
                break;

            case RelationshipType.JeonSangYeon:
                JeonSangYeonDelta += amount;
                break;
        }
    }


    public float GetRelationship(
        RelationshipType type)
    {
        switch (type)
        {
            case RelationshipType.Danwol:
                return danwolDelta;

            case RelationshipType.Yaseo:
                return yaseoDelta;

            case RelationshipType.Macheon:
                return macheonDelta;

            case RelationshipType.Hongryeon:
                return hongryeonDelta;

            case RelationshipType.JeonSangYeon:
                return JeonSangYeonDelta;
        }

        return 0f;
    }


    public void Reset()
    {
        earnedMoney = 0;

        danwolDelta = 0;
        yaseoDelta = 0;
        macheonDelta = 0;
        hongryeonDelta = 0;
        JeonSangYeonDelta = 0;
    }
}