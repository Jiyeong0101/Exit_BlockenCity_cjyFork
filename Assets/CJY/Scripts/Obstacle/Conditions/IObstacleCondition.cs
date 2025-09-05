using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 방해물 발동 조건 인터페이스
/// </summary>
public interface IObstacleCondition
{
    bool Evaluate(ObstacleGameState state);
}
