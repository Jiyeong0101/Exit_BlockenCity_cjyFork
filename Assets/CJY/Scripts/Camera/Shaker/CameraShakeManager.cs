using System;
using UnityEngine;


public enum CameraShakeType
{
    Light,
    LineClear,
    Explosion,
    StoryImpact,
    Heavy
}


[Serializable]
public class CameraShakePreset
{
    [Header("Type")]
    public CameraShakeType type;


    [Header("Basic")]
    [Header("전체 흔들림 세기")]
    [Min(0f)]
    public float intensity = 1f;

    [Header("몇 초 동안 흔들릴지")]
    [Min(0.01f)]
    public float duration = 0.25f;

    [Header("얼마나 빠르게 흔들릴지")]
    [Min(0.01f)]
    public float frequency = 15f;


    [Header("Position Shake")]
    public Vector3 positionStrength =
        new Vector3(
            0.03f,
            0.05f,
            0.01f
        );


    [Header("Rotation Shake")]
    public Vector3 rotationStrength =
        new Vector3(
            0.4f,
            0.2f,
            0.35f
        );
}


[DefaultExecutionOrder(1000)]
public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance
    {
        get;
        private set;
    }


    [Header("Shake Presets")]
    [SerializeField]
    private CameraShakePreset[] presets;


    [Header("Debug")]
    [SerializeField]
    private bool enableDebugKeys = true;


    // =============================================
    // 현재 Shake 상태
    // =============================================

    private bool isShaking = false;

    private float elapsedTime;
    private float currentDuration;
    private float currentIntensity;
    private float currentFrequency;

    private Vector3 currentPositionStrength;
    private Vector3 currentRotationStrength;


    // =============================================
    // Noise
    // =============================================

    private float noiseSeed;


    // =============================================
    // 이전 프레임에 적용한 Shake
    // 다음 프레임에서 제거하기 위해 저장
    // =============================================

    private Vector3 lastPositionOffset =
        Vector3.zero;

    private Quaternion lastRotationOffset =
        Quaternion.identity;


    private void Awake()
    {
        if (Instance != null &&
            Instance != this)
        {
            Debug.LogWarning(
                "[CameraShakeManager] " +
                "CameraShakeManager가 중복으로 존재합니다."
            );
        }

        Instance = this;
    }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


    private void Update()
    {
        if (!enableDebugKeys)
        {
            return;
        }


        // =============================================
        // 테스트용 키
        // =============================================

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Play(
                CameraShakeType.Light
            );
        }


        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Play(
                CameraShakeType.LineClear
            );
        }


        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Play(
                CameraShakeType.Explosion
            );
        }


        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Play(
                CameraShakeType.StoryImpact
            );
        }


        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Play(
                CameraShakeType.Heavy
            );
        }
    }


    private void LateUpdate()
    {
        // =============================================
        // 지난 프레임 Shake 제거
        // =============================================

        RemovePreviousOffset();


        if (!isShaking)
        {
            return;
        }


        // Time.timeScale이 0이어도
        // 카메라 연출은 재생되도록 unscaled 사용
        elapsedTime +=
            Time.unscaledDeltaTime;


        // =============================================
        // Shake 종료
        // =============================================

        if (elapsedTime >= currentDuration)
        {
            StopInternal();
            return;
        }


        // =============================================
        // 진행도
        //
        // 0 = 시작
        // 1 = 종료
        // =============================================

        float normalizedTime =
            Mathf.Clamp01(
                elapsedTime /
                currentDuration
            );


        // =============================================
        // 감쇠
        //
        // 처음 강하게 시작해서
        // 끝으로 갈수록 빠르게 약해짐
        // =============================================

        float decay =
            1f - normalizedTime;

        decay *= decay;


        // =============================================
        // Perlin Noise Sample
        // =============================================

        float sampleTime =
            noiseSeed +
            elapsedTime *
            currentFrequency;


        Vector3 positionNoise =
            new Vector3(
                GetNoise(
                    sampleTime
                ),
                GetNoise(
                    sampleTime + 20f
                ),
                GetNoise(
                    sampleTime + 40f
                )
            );


        Vector3 rotationNoise =
            new Vector3(
                GetNoise(
                    sampleTime + 60f
                ),
                GetNoise(
                    sampleTime + 80f
                ),
                GetNoise(
                    sampleTime + 100f
                )
            );


        // =============================================
        // Position Offset
        // =============================================

        Vector3 positionOffset =
            Vector3.Scale(
                positionNoise,
                currentPositionStrength
            );

        positionOffset *=
            currentIntensity *
            decay;


        // =============================================
        // Rotation Offset
        // =============================================

        Vector3 rotationOffset =
            Vector3.Scale(
                rotationNoise,
                currentRotationStrength
            );

        rotationOffset *=
            currentIntensity *
            decay;


        Quaternion rotationQuaternion =
            Quaternion.Euler(
                rotationOffset
            );


        // =============================================
        // 실제 카메라 적용
        // =============================================

        transform.localPosition +=
            positionOffset;

        transform.localRotation *=
            rotationQuaternion;


        // 다음 프레임에서 제거하기 위해 저장
        lastPositionOffset =
            positionOffset;

        lastRotationOffset =
            rotationQuaternion;
    }


    // =============================================
    // Type 기반 Shake
    // =============================================

    public void Play(
        CameraShakeType type)
    {
        CameraShakePreset preset =
            FindPreset(type);


        if (preset == null)
        {
            Debug.LogWarning(
                $"[CameraShakeManager] " +
                $"{type} 프리셋을 찾을 수 없습니다."
            );

            return;
        }


        Play(preset);
    }


    // =============================================
    // Preset 실행
    // =============================================

    private void Play(
        CameraShakePreset preset)
    {
        if (preset == null)
        {
            return;
        }


        // =============================================
        // 현재 더 강한 Shake가 실행 중이면
        // 약한 Shake는 무시
        // =============================================

        if (isShaking &&
            preset.intensity <
            currentIntensity)
        {
            return;
        }


        StartShake(
            preset.intensity,
            preset.duration,
            preset.frequency,
            preset.positionStrength,
            preset.rotationStrength
        );
    }


    // =============================================
    // 직접 값 지정 Shake
    //
    // Story 등에서 특수 연출이 필요할 때 사용 가능
    // =============================================

    public void Play(
        float intensity,
        float duration,
        float frequency = 15f)
    {
        StartShake(
            intensity,
            duration,
            frequency,
            new Vector3(
                0.03f,
                0.05f,
                0.01f
            ),
            new Vector3(
                0.4f,
                0.2f,
                0.35f
            )
        );
    }


    // =============================================
    // Shake 시작
    // =============================================

    private void StartShake(
        float intensity,
        float duration,
        float frequency,
        Vector3 positionStrength,
        Vector3 rotationStrength)
    {
        currentIntensity =
            Mathf.Max(
                0f,
                intensity
            );

        currentDuration =
            Mathf.Max(
                0.01f,
                duration
            );

        currentFrequency =
            Mathf.Max(
                0.01f,
                frequency
            );


        currentPositionStrength =
            positionStrength;

        currentRotationStrength =
            rotationStrength;


        elapsedTime = 0f;


        // Perlin Noise 시작 위치를 매번 다르게
        noiseSeed =
            UnityEngine.Random.Range(
                0f,
                1000f
            );


        isShaking = true;
    }


    // =============================================
    // 이전 프레임 Offset 제거
    // =============================================

    private void RemovePreviousOffset()
    {
        if (lastPositionOffset !=
            Vector3.zero)
        {
            transform.localPosition -=
                lastPositionOffset;

            lastPositionOffset =
                Vector3.zero;
        }


        if (lastRotationOffset !=
            Quaternion.identity)
        {
            transform.localRotation =
                transform.localRotation *
                Quaternion.Inverse(
                    lastRotationOffset
                );

            lastRotationOffset =
                Quaternion.identity;
        }
    }


    // =============================================
    // 내부 종료
    // =============================================

    private void StopInternal()
    {
        isShaking = false;

        elapsedTime = 0f;

        currentIntensity = 0f;

        lastPositionOffset =
            Vector3.zero;

        lastRotationOffset =
            Quaternion.identity;
    }


    // =============================================
    // Perlin Noise
    //
    // Mathf.PerlinNoise는 0 ~ 1
    // 이것을 -1 ~ 1로 변환
    // =============================================

    private float GetNoise(
        float value)
    {
        return
            Mathf.PerlinNoise(
                value,
                0f
            ) * 2f - 1f;
    }


    // =============================================
    // Preset 검색
    // =============================================

    private CameraShakePreset FindPreset(
        CameraShakeType type)
    {
        if (presets == null)
        {
            return null;
        }


        for (int i = 0;
             i < presets.Length;
             i++)
        {
            CameraShakePreset preset =
                presets[i];


            if (preset != null &&
                preset.type == type)
            {
                return preset;
            }
        }


        return null;
    }
}