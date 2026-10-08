using UnityEngine;
using UnityEngine.UI;

public class BackgroundTestController : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private Image backgroundImage;

    [Header("Background Sprites")]
    [SerializeField] private Sprite background1;
    [SerializeField] private Sprite background2;
    [SerializeField] private Sprite background3;

    [Header("Black Overlay")]
    [SerializeField] private GameObject blackOverlay;

    private void Start()
    {
        if (blackOverlay != null)
            blackOverlay.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeBackground(background1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeBackground(background2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeBackground(background3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleBlackOverlay();
        }
    }

    private void ChangeBackground(Sprite sprite)
    {
        if (backgroundImage != null && sprite != null)
        {
            backgroundImage.sprite = sprite;
        }
    }

    private void ToggleBlackOverlay()
    {
        if (blackOverlay != null)
        {
            blackOverlay.SetActive(!blackOverlay.activeSelf);
        }
    }
}