using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class TetrisMoveSFX : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField]
    private AudioClip blockDownSFX;

    [Header("Audio Mixer")]
    [Tooltip("Audio Mixer의 SFX 그룹을 연결하세요.")]
    [SerializeField]
    private AudioMixerGroup sfxMixerGroup;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)]
    private float volume = 1f;


    private AudioSource audioSource;

    // Space 하드드롭 시
    // 같은 프레임에 여러 번 Move가 호출되는 것 방지
    private int lastPlayedFrame = -1;


    private void Awake()
    {
        audioSource =
            GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // UI/Game SFX이므로 2D


        // SFX Mixer Group 연결
        if (sfxMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup =
                sfxMixerGroup;
        }
        else
        {
            Debug.LogWarning(
                "[TetrisMoveSFX] " +
                "SFX AudioMixerGroup이 연결되어 있지 않습니다."
            );
        }
    }


    private void OnEnable()
    {
        TetriminoBlock.OnAnyBlockMoved +=
            HandleBlockMoved;
    }


    private void OnDisable()
    {
        TetriminoBlock.OnAnyBlockMoved -=
            HandleBlockMoved;
    }


    private void HandleBlockMoved(
        TetriminoBlock block,
        Vector3 direction)
    {
        if (block == null)
            return;


        // 아래로 내려갈 때만 소리
        if (direction != Vector3.down)
            return;


        PlayDownSFX();
    }


    private void PlayDownSFX()
    {
        if (blockDownSFX == null)
            return;


        // Space는 같은 프레임에 여러 칸을
        // 한 번에 Move하므로 SFX는 한 번만
        if (lastPlayedFrame == Time.frameCount)
            return;


        lastPlayedFrame =
            Time.frameCount;


        audioSource.PlayOneShot(
            blockDownSFX,
            volume
        );
    }
}