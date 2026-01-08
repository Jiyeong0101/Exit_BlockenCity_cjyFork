using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySettingsUI : MonoBehaviour
{
    public void SetTextSmall()
    {
        StorySettingsManager.Instance.SetTextSize(StoryTextSize.Small);
    }

    public void SetTextMedium()
    {
        StorySettingsManager.Instance.SetTextSize(StoryTextSize.Medium);
    }

    public void SetTextLarge()
    {
        StorySettingsManager.Instance.SetTextSize(StoryTextSize.Large);
    }

    public void SetAutoAdvance()
    {
        StorySettingsManager.Instance.SetAdvanceMode(StoryAdvanceMode.Auto);
    }

    public void SetManualAdvance()
    {
        StorySettingsManager.Instance.SetAdvanceMode(StoryAdvanceMode.Manual);
    }
}
