using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class TetrisSFXPlayer : MonoBehaviour
{
    [Header("Tetris SFX")]

    [Tooltip("블록이 한 칸 아래로 내려갈 때")]
    [SerializeField]
    private AudioClip blockDownSFX;

    [Tooltip("블록이 타워에 최종 설치될 때")]
    [SerializeField]
    private AudioClip blockLockSFX;

    [Tooltip("한 층이 완성되어 파괴 연출이 시작될 때")]
    [SerializeField]
    private AudioClip lineClearSFX;


    [Header("Audio Mixer")]

    [Tooltip("Audio Mixer의 SFX Group")]
    [SerializeField]
    private AudioMixerGroup sfxMixerGroup;


    [Header("Volume")]

    [SerializeField, Range(0f, 1f)]
    private float downVolume = 1f;

    [SerializeField, Range(0f, 1f)]
    private float lockVolume = 1f;

    [SerializeField, Range(0f, 1f)]
    private float lineClearVolume = 1f;


    private AudioSource audioSource;


    // ---------------------------------------------
    // 하강 사운드 예약
    //
    // Space HardDrop은
    // 같은 프레임에 Move ↓ 여러 번
    // 이후 BlockLock이 발생합니다.
    //
    // 따라서 이동 이벤트에서 즉시 소리를 내지 않고
    // LateUpdate까지 기다렸다가 설치되지 않았으면
    // 하강음을 재생합니다.
    // ---------------------------------------------

    private bool pendingDownSFX = false;

    private int pendingDownFrame = -1;

    private TetriminoBlock pendingDownBlock;


    private void Awake()
    {
        audioSource =
            GetComponent<AudioSource>();


        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // 테트리스 조작음이므로 2D Sound
        audioSource.spatialBlend = 0f;


        // Inspector에 직접 연결하지 않았을 경우
        // 기존 SoundManager의 Mixer에서
        // SFX Group을 찾아본다.
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


        // 반드시 SFX Group으로 출력
        if (sfxMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup =
                sfxMixerGroup;
        }
        else
        {
            Debug.LogWarning(
                "[TetrisSFXPlayer] " +
                "SFX AudioMixerGroup을 찾지 못했습니다."
            );
        }
    }


    private void OnEnable()
    {
        TetriminoBlock.OnAnyBlockMoved +=
            HandleBlockMoved;

        TetriminoBlock.OnAnyBlockLocked +=
            HandleBlockLocked;

        TetrisTower.OnAnyLineClearStarted +=
            HandleLineClearStarted;
    }


    private void OnDisable()
    {
        TetriminoBlock.OnAnyBlockMoved -=
            HandleBlockMoved;

        TetriminoBlock.OnAnyBlockLocked -=
            HandleBlockLocked;

        TetrisTower.OnAnyLineClearStarted -=
            HandleLineClearStarted;
    }


    // =============================================
    // 블록 이동
    // =============================================

    private void HandleBlockMoved(
        TetriminoBlock block,
        Vector3 direction)
    {
        if (block == null)
            return;


        // 좌우/앞뒤 이동은 무시
        if (direction != Vector3.down)
            return;


        // 즉시 소리를 내지 않는다.
        //
        // Space HardDrop인지 판단하기 위해
        // 이번 프레임 끝까지 기다린다.

        pendingDownSFX = true;

        pendingDownFrame =
            Time.frameCount;

        pendingDownBlock =
            block;
    }


    // =============================================
    // 블록 설치
    // =============================================

    private void HandleBlockLocked(
        TetriminoBlock block)
    {
        if (block == null)
            return;


        // 같은 프레임에 하강 예약이 있었다면
        // Space HardDrop일 가능성이 있으므로
        // 하강음을 취소한다.
        if (pendingDownSFX &&
            pendingDownBlock == block &&
            pendingDownFrame == Time.frameCount)
        {
            pendingDownSFX = false;
            pendingDownBlock = null;
        }


        PlayLockSFX();
    }

    // =============================================
    // 한 층 삭제
    // =============================================

    private void HandleLineClearStarted(int y)
    {
        PlayLineClearSFX();
    }

    private void PlayLineClearSFX()
    {
        if (lineClearSFX == null)
            return;


        audioSource.PlayOneShot(
            lineClearSFX,
            lineClearVolume
        );
    }

    // =============================================
    // 프레임 마지막
    // =============================================

    private void LateUpdate()
    {
        if (!pendingDownSFX)
            return;


        // 이동이 있었고
        // 그 프레임에 BlockLock이 발생하지 않았다면
        // 정상적인 한 칸 하강으로 취급
        PlayDownSFX();


        pendingDownSFX = false;
        pendingDownBlock = null;
    }


    // =============================================
    // 실제 Sound 재생
    // =============================================

    private void PlayDownSFX()
    {
        if (blockDownSFX == null)
            return;


        audioSource.PlayOneShot(
            blockDownSFX,
            downVolume
        );
    }


    private void PlayLockSFX()
    {
        if (blockLockSFX == null)
            return;


        audioSource.PlayOneShot(
            blockLockSFX,
            lockVolume
        );
    }
}