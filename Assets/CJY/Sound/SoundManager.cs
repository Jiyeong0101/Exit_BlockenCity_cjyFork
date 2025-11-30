using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer mainMixer;

    // PlayerPrefs 키값
    private const string MASTER_KEY = "Master";
    private const string BGM_KEY = "BGM";
    private const string SFX_KEY = "SFX";

    // 0~1 슬라이더 값 캐싱 (선택)
    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolumeSettings();
        ApplyAllVolumes();
    }

    private void Start()
    {
        // Snapshot 반영 후에 Mixer 값을 덮어쓰기!
        ApplyAllVolumes();
    }

    // 볼륨 설정 함수들
    public void SetMasterVolume(float value)   // value: 0~1 (슬라이더 값)
    {
        masterVolume = value;
        SetMixerVolume("Master", value);
        PlayerPrefs.SetFloat(MASTER_KEY, value);
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        SetMixerVolume("BGM", value);
        PlayerPrefs.SetFloat(BGM_KEY, value);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        SetMixerVolume("SFX", value);
        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    // AudioMixer에 실제 dB값으로 넣는 부분
    private void SetMixerVolume(string exposedParam, float value01)
    {
        // 0이면 완전 음소거 (-80dB 정도)
        if (value01 <= 0.0001f)
        {
            mainMixer.SetFloat(exposedParam, -80f);
        }
        else
        {
            // 0~1 값을 로그 스케일 dB로 변환
            float dB = Mathf.Log10(value01) * 20f;
            mainMixer.SetFloat(exposedParam, dB);
        }
    }

    // 저장 & 불러오기
    private void LoadVolumeSettings()
    {
        // 없으면 기본값 1.0으로
        masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        bgmVolume = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
    }

    private void ApplyAllVolumes()
    {
        SetMixerVolume("Master", masterVolume);
        SetMixerVolume("BGM", bgmVolume);
        SetMixerVolume("SFX", sfxVolume);
    }
}

