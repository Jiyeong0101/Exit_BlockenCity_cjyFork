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
        string dialogue)
    {
        bool hasSpeaker =
            !string.IsNullOrWhiteSpace(speakerName);

        if (nameText != null)
        {
            nameText.gameObject.SetActive(hasSpeaker);
            nameText.text = hasSpeaker
                ? speakerName
                : string.Empty;
        }

        if (lineText != null)
        {
            lineText.text =
                dialogue ?? string.Empty;
        }
    }
}