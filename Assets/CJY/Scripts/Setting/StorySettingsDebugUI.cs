using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StorySettingsDebugUI : MonoBehaviour
{
    public TMP_Text debugText;

    private void Update()
    {
        var settings = StorySettingsManager.Instance;

        debugText.text =
            $"Text Size : {settings.GetTextSize()}\n" +
            $"Advance Mode : {(settings.IsAutoAdvance() ? "Auto" : "Manual")}";
    }
}

