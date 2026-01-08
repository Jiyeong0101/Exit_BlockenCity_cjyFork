using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySettingsManager : MonoBehaviour
{
    public static StorySettingsManager Instance;

    [Header("Current Settings")]
    public StoryTextSize textSize = StoryTextSize.Medium;
    public StoryAdvanceMode advanceMode = StoryAdvanceMode.Manual;

    private const string TEXT_SIZE_KEY = "Story_TextSize";
    private const string ADVANCE_MODE_KEY = "Story_AdvanceMode";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }

    // ===== Setters (설정 UI에서 호출) =====

    public void SetTextSize(StoryTextSize size)
    {
        textSize = size;
        PlayerPrefs.SetInt(TEXT_SIZE_KEY, (int)size);
    }

    public void SetAdvanceMode(StoryAdvanceMode mode)
    {
        advanceMode = mode;
        PlayerPrefs.SetInt(ADVANCE_MODE_KEY, (int)mode);
    }

    // ===== Getters (스토리 시스템에서 사용 예정) =====

    public StoryTextSize GetTextSize()
    {
        return textSize;
    }

    public bool IsAutoAdvance()
    {
        return advanceMode == StoryAdvanceMode.Auto;
    }

    private void LoadSettings()
    {
        textSize = (StoryTextSize)PlayerPrefs.GetInt(
            TEXT_SIZE_KEY, (int)StoryTextSize.Medium);

        advanceMode = (StoryAdvanceMode)PlayerPrefs.GetInt(
            ADVANCE_MODE_KEY, (int)StoryAdvanceMode.Manual);
    }
}
