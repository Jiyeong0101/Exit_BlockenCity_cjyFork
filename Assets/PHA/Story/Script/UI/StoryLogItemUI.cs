using TMPro;
using UnityEngine;

public class StoryLogItemUI : MonoBehaviour
{
    [Header("로그 텍스트")]
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text lineText;

    public void Setup(
     string speakerName,
     string dialogue,
     float fontSize)
    {
        bool hasSpeaker =
            !string.IsNullOrWhiteSpace(speakerName);

        if (nameText != null)
        {
            nameText.gameObject.SetActive(hasSpeaker);

            nameText.text =
                hasSpeaker
                    ? speakerName
                    : string.Empty;

            nameText.fontSize =
                fontSize * 0.8f;
        }

        if (lineText != null)
        {
            lineText.text =
                dialogue ?? string.Empty;

            lineText.fontSize =
                fontSize;
        }
    }

    public void ApplyFontSize(float fontSize)
    {
        if (nameText != null)
        {
            nameText.fontSize =
                fontSize * 0.8f;
        }

        if (lineText != null)
        {
            lineText.fontSize =
                fontSize;
        }
    }
}