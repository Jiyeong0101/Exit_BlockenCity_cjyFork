using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class ObstacleSoundManager : MonoBehaviour
{
    public static ObstacleSoundManager Instance
    {
        get;
        private set;
    }


    [Header("Obstacle Sound Data")]

    [SerializeField]
    private ObstacleSoundData soundData;


    [Header("Audio Mixer")]

    [Tooltip("Audio Mixer의 SFX Group")]
    [SerializeField]
    private AudioMixerGroup sfxMixerGroup;


    [Header("Master Volume")]

    [Tooltip("모든 방해물 효과음에 공통으로 적용되는 볼륨")]
    [FormerlySerializedAs("volume")]
    [SerializeField, Range(0f, 1f)]
    private float masterVolume = 1f;


    // 일반 1회성 효과음 재생용
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


        // =========================================
        // AudioSource 준비
        // =========================================

        audioSource =
            GetComponent<AudioSource>();


        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;


        // =========================================
        // SFX Mixer Group 찾기
        // =========================================

        FindSFXMixerGroup();


        if (sfxMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup =
                sfxMixerGroup;
        }
        else
        {
            Debug.LogWarning(
                "[ObstacleSoundManager] " +
                "SFX AudioMixerGroup을 찾지 못했습니다."
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
                "[ObstacleSoundManager] " +
                "ObstacleSoundData가 연결되어 있지 않습니다."
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
            sfxMixerGroup =
                groups[0];
        }
    }


    // =============================================
    // 개별 방해물 최종 Volume 계산
    // =============================================

    private float GetFinalVolume(
        ObstacleType type)
    {
        if (soundData == null)
            return masterVolume;


        float obstacleVolume =
            soundData.GetVolume(type);


        return Mathf.Clamp01(
            masterVolume *
            obstacleVolume
        );
    }


    // =============================================
    // 일반 1회성 효과음
    // =============================================

    public void PlayObstacleSound(
        ObstacleType type)
    {
        if (soundData == null ||
            audioSource == null)
        {
            return;
        }


        // Awake 순서 때문에 Mixer를 못 찾은 경우
        // 재생 직전에 다시 확인
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
                $"[ObstacleSoundManager] " +
                $"{type} 효과음이 없습니다."
            );

            return;
        }


        float finalVolume =
            GetFinalVolume(type);


        audioSource.PlayOneShot(
            clip,
            finalVolume
        );
    }


    // =============================================
    // 지속형 효과음
    //
    // 장마처럼 해당 Effect Object가 존재하는 동안
    // 계속 재생해야 하는 효과음
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
                $"[ObstacleSoundManager] " +
                $"{type} Loop 효과음이 없습니다."
            );

            return null;
        }


        // Mixer가 아직 연결되지 않았다면 다시 탐색
        if (sfxMixerGroup == null)
        {
            FindSFXMixerGroup();
        }


        AudioSource loopSource =
            targetObject.AddComponent<AudioSource>();


        loopSource.playOnAwake = false;
        loopSource.loop = true;
        loopSource.spatialBlend = 0f;

        loopSource.clip = clip;


        // 방해물별 개별 Volume 적용
        loopSource.volume =
            GetFinalVolume(type);


        if (sfxMixerGroup != null)
        {
            loopSource.outputAudioMixerGroup =
                sfxMixerGroup;
        }


        loopSource.Play();


        return loopSource;
    }
}