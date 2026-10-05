using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class ItemSFXPlayer : MonoBehaviour
{
    public static ItemSFXPlayer Instance
    {
        get;
        private set;
    }


    [Header("Item SFX")]

    [Tooltip("폭탄이 실제로 폭발할 때")]
    [SerializeField]
    private AudioClip bombSFX;

    [Tooltip("Undo 아이템이 성공적으로 사용될 때")]
    [SerializeField]
    private AudioClip whistleSFX;


    [Header("Audio Mixer")]

    [Tooltip("Audio Mixer의 SFX Group")]
    [SerializeField]
    private AudioMixerGroup sfxMixerGroup;


    [Header("Volume")]

    [SerializeField, Range(0f, 1f)]
    private float bombVolume = 1f;

    [SerializeField, Range(0f, 1f)]
    private float whistleVolume = 1f;


    private AudioSource audioSource;


    private void Awake()
    {
        if (Instance != null &&
            Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        audioSource =
            GetComponent<AudioSource>();


        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;


        // Inspector에서 연결하지 않았으면
        // 기존 SoundManager의 SFX Mixer 자동 탐색
        if (sfxMixerGroup == null &&
            SoundManager.Instance != null &&
            SoundManager.Instance.mainMixer != null)
        {
            AudioMixerGroup[] groups =
                SoundManager.Instance
                    .mainMixer
                    .FindMatchingGroups("SFX");


            if (groups != null &&
                groups.Length > 0)
            {
                sfxMixerGroup =
                    groups[0];
            }
        }


        if (sfxMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup =
                sfxMixerGroup;
        }
        else
        {
            Debug.LogWarning(
                "[ItemSFXPlayer] " +
                "SFX AudioMixerGroup을 찾지 못했습니다."
            );
        }
    }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


    // =============================================
    // Bomb
    // =============================================

    public void PlayBombSFX()
    {
        if (bombSFX == null)
            return;


        audioSource.PlayOneShot(
            bombSFX,
            bombVolume
        );
    }


    // =============================================
    // Undo Whistle
    // =============================================

    public void PlayWhistleSFX()
    {
        if (whistleSFX == null)
            return;


        audioSource.PlayOneShot(
            whistleSFX,
            whistleVolume
        );
    }
}