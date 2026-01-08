using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // 시작할 때 슬라이더 값을 SoundManager의 값으로 맞추기
        if (SoundManager.Instance != null)
        {
            masterSlider.value = SoundManager.Instance.masterVolume;
            bgmSlider.value = SoundManager.Instance.bgmVolume;
            sfxSlider.value = SoundManager.Instance.sfxVolume;
        }

        // 코드에서 리스너 등록해도 되고,
        // 인스펙터에서 OnValueChanged에 직접 연결해도 됨.
        masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
    }

    public void OnMasterSliderChanged(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetMasterVolume(value);
        }
    }

    public void OnBGMSliderChanged(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetBGMVolume(value);
        }
    }

    public void OnSFXSliderChanged(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSFXVolume(value);
        }
    }
}
