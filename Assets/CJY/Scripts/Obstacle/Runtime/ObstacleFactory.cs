using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory
{
    private readonly Dictionary<string, System.Action<ObstacleGameState>> _effectTable;

    // ObstacleEffects 인스턴스를 주입받아서 effectTable을 생성
    public ObstacleFactory(ObstacleEffects effectsInstance)
    {
        _effectTable = ObstacleEffectMap.Create(effectsInstance);
    }

    public List<ObstacleRuntime> CreateAllObstacles()
    {
        var obstacles = new List<ObstacleRuntime>();

        // 얼어붙은 블록 - 회전 금지
        obstacles.Add(new ObstacleRuntime(
            new List<IObstacleCondition> { new Condition_ObstacleType(ObstacleType.FrozenBlock) },
            new List<System.Action<ObstacleGameState>> { _effectTable["DisableRotation"] }
        ));

        // 강풍 - 블록 밀림
        obstacles.Add(new ObstacleRuntime(
            new List<IObstacleCondition> { new Condition_ObstacleType(ObstacleType.StrongWind) },
            new List<System.Action<ObstacleGameState>> { _effectTable["PushBlockRandomly"] }
        ));

        // 잔설 - 입력 지연
        obstacles.Add(new ObstacleRuntime(
            new List<IObstacleCondition> { new Condition_ObstacleType(ObstacleType.SnowDelay) },
            new List<System.Action<ObstacleGameState>> { _effectTable["InputDelay"] }
        ));

        // 황사 - 시야 차단
        obstacles.Add(new ObstacleRuntime(
            new List<IObstacleCondition> { new Condition_ObstacleType(ObstacleType.Sandstorm) },
            new List<System.Action<ObstacleGameState>> { _effectTable["ApplyDustStormEffect"] }
        ));
        

        // 낙뢰 - 조작 일시 정지
        obstacles.Add(new ObstacleRuntime(
            new List<IObstacleCondition> { new Condition_ObstacleType(ObstacleType.Thunder) },
            new List<System.Action<ObstacleGameState>> { _effectTable["DisableControlTemporary"] }
        ));

        // 장마 - 스페이스 금지 + 낙하 속도 감소
        obstacles.Add(new ObstacleRuntime(
            new List<IObstacleCondition> { new Condition_ObstacleType(ObstacleType.Rainy) },
            new List<System.Action<ObstacleGameState>> {
                _effectTable["DisableSpace"],
                _effectTable["SlowDropSpeed"]
            }
        ));

        // 폭염 - 다음 블록 UI 숨기기
        obstacles.Add(new ObstacleRuntime(
            new List<IObstacleCondition> { new Condition_ObstacleType(ObstacleType.Heatwave) },
            new List<System.Action<ObstacleGameState>> { _effectTable["HideNextBlockUI"] }
        ));

        // 건기 - 블록 파괴 확률
        obstacles.Add(new ObstacleRuntime(
            new List<IObstacleCondition> { new Condition_ObstacleType(ObstacleType.Drought) },
            new List<System.Action<ObstacleGameState>> { _effectTable["BreakBlockOnPlace"] }
        ));

        // 스모그 - UI 스모그 오버레이
        obstacles.Add(new ObstacleRuntime(
            new List<IObstacleCondition> { new Condition_ObstacleType(ObstacleType.Smog) },
            new List<System.Action<ObstacleGameState>> { _effectTable["ApplySmogOverlay"] }
        ));

        return obstacles;
    }
}
