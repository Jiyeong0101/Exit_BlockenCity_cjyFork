using UnityEngine;
using UnityEngine.Audio; // AudioMixerGroup을 사용하기 위해 필요합니다.

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Header("Audio Mixer Output")]
    [Tooltip("AudioMixer에서 지정한 SFX 그룹을 여기에 넣어주세요.")]
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    private AudioSource audioSource2D;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // SFXManager 자체에 AudioSource 자동 생성
        audioSource2D = gameObject.AddComponent<AudioSource>();
        audioSource2D.playOnAwake = false;

        // 오디오 아웃풋을 'SFX' Mixer Group으로 설정
        if (sfxMixerGroup != null)
        {
            audioSource2D.outputAudioMixerGroup = sfxMixerGroup;
        }
    }

    /// <summary>
    /// 일반 2D 효과음 재생 (카메라 거리와 상관없이 일정하게 들림)
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null) return;
        audioSource2D.PlayOneShot(clip, volume);
    }

    /// <summary>
    /// 3D 위치 기반 효과음 (모델 위치에서 소리가 나며, 아웃풋을 SFX 믹서로 보냄)
    /// </summary>
    public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1.0f)
    {
        if (clip == null) return;

        // 임시 3D 사운드 오브젝트 생성
        GameObject tempAudioGO = new GameObject("TempSFX_" + clip.name);
        tempAudioGO.transform.position = position;

        AudioSource tempSource = tempAudioGO.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.volume = volume;
        tempSource.spatialBlend = 1.0f; // 3D 사운드로 설정 (거리별 음량 감소)

        // 3D 소리도 SFX 믹서 그룹으로 전송
        if (sfxMixerGroup != null)
        {
            tempSource.outputAudioMixerGroup = sfxMixerGroup;
        }

        tempSource.Play();

        // 재생이 완료되면 자동 삭제
        Destroy(tempAudioGO, clip.length);
    }
}