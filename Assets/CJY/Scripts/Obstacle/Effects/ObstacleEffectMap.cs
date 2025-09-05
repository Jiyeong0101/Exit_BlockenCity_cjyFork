using System.Collections.Generic;
using System;

public static class ObstacleEffectMap
{
    public static Dictionary<string, Action<ObstacleGameState>> Create(ObstacleEffects effects)
    {
        return new Dictionary<string, Action<ObstacleGameState>>()
        {
            { "DisableRotation", state => effects.TryFreezeBlock(state) },
            { "PushBlockRandomly", state => effects.PushBlockRandomly(state) },
            { "InputDelay", state => effects.InputDelay(state) },
            { "ApplyDustStormEffect", state => effects.ApplyDustStormEffect(state) },
            { "DisableControlTemporary", state => effects.DisableControlTemporary(state) },
            { "DisableSpace", state => effects.DisableSpace(state) },
            { "SlowDropSpeed", state => effects.SlowDropSpeed(state) },
            { "HideNextBlockUI", state => effects.HideNextBlockUI(state) },
            { "BreakBlockOnPlace", state => effects.BreakBlockOnPlace(state) },
            { "ApplySmogOverlay", state => effects.ApplySmogOverlay(state) }
        };
    }
}
