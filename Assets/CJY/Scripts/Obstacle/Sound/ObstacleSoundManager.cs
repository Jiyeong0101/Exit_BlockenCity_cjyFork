using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class ObstacleSoundManager : MonoBehaviour
{
    public static ObstacleSoundManager Instance { get; private set; }

    [Header("Obstacle Sound Data")]
    [SerializeField]
    private ObstacleSoundData soundData;

    [Header("Audio Mixer")]
    [Tooltip("Audio Mixer의 SFX Group")]
    [SerializeField]
    private AudioMixerGroup sfxMixerGroup;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)]
    private float volume = 1f;

    // 일반적인 1회성 효과음 재생용
    private AudioSource audioSource;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        // =========================================
        // AudioSource 준비
        // =========================================
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;


        // =========================================
        // SFX Mixer Group 찾기
        // =========================================
        FindSFXMixerGroup();


        if (sfxMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = sfxMixerGroup;
        }
        else
        {
            Debug.LogWarning(
                "[ObstacleSoundManager] SFX AudioMixerGroup을 찾지 못했습니다."
            );
        }


        // =========================================
        // Sound Data 초기화
        // =========================================
        if (soundData != null)
        {
            soundData.Initialize();
        }
        else
        {
            Debug.LogWarning(
                "[ObstacleSoundManager] ObstacleSoundData가 연결되어 있지 않습니다."
            );
        }
    }


    // =============================================
    // SFX Mixer Group 탐색
    // =============================================
    private void FindSFXMixerGroup()
    {
        if (sfxMixerGroup != null)
            return;

        if (SoundManager.Instance == null)
            return;

        if (SoundManager.Instance.mainMixer == null)
            return;


        AudioMixerGroup[] groups =
            SoundManager.Instance
                .mainMixer
                .FindMatchingGroups("SFX");


        if (groups != null &&
            groups.Length > 0)
        {
            sfxMixerGroup = groups[0];
        }
    }


    // =============================================
    // 일반 1회성 효과음
    // =============================================
    public void PlayObstacleSound(ObstacleType type)
    {
        if (soundData == null ||
            audioSource == null)
        {
            return;
        }


        // Awake 순서 때문에 Mixer를 못 찾았을 경우
        // 재생 직전에 다시 한 번 확인
        if (sfxMixerGroup == null)
        {
            FindSFXMixerGroup();

            if (sfxMixerGroup != null)
            {
                audioSource.outputAudioMixerGroup =
                    sfxMixerGroup;
            }
        }


        AudioClip clip =
            soundData.GetClip(type);


        if (clip == null)
        {
            Debug.LogWarning(
                $"[ObstacleSoundManager] {type} 효과음이 없습니다."
            );

            return;
        }


        audioSource.PlayOneShot(
            clip,
            volume
        );
    }


    // =============================================
    // 지속형 효과음
    //
    // 장마처럼 해당 Effect Object가 존재하는 동안
    // 계속 재생해야 하는 효과음용
    // =============================================
    public AudioSource PlayLoopSoundOnObject(
        ObstacleType type,
        GameObject targetObject)
    {
        if (soundData == null ||
            targetObject == null)
        {
            return null;
        }


        AudioClip clip =
            soundData.GetClip(type);


        if (clip == null)
        {
            Debug.LogWarning(
                $"[ObstacleSoundManager] {type} Loop 효과음이 없습니다."
            );

            return null;
        }


        // Mixer가 아직 연결되지 않았다면 다시 탐색
        if (sfxMixerGroup == null)
        {
            FindSFXMixerGroup();
        }


        // Rain Effect 전용 AudioSource를
        // 해당 Effect Object에 추가한다.
        AudioSource loopSource =
            targetObject.AddComponent<AudioSource>();


        loopSource.playOnAwake = false;
        loopSource.loop = true;
        loopSource.spatialBlend = 0f;

        loopSource.clip = clip;
        loopSource.volume = volume;


        if (sfxMixerGroup != null)
        {
            loopSource.outputAudioMixerGroup =
                sfxMixerGroup;
        }


        loopSource.Play();


        return loopSource;
    }
}