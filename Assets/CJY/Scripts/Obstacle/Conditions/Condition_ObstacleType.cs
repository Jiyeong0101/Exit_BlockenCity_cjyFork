using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_ObstacleType : IObstacleCondition
{
    private ObstacleType requiredType;

    public Condition_ObstacleType(ObstacleType type)
    {
        requiredType = type;
    }

    public bool Evaluate(ObstacleGameState state)
    {
        return state.CheckCondition(requiredType);
    }
}

