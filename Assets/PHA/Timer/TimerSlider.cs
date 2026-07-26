using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour
{
    public Slider timerSlider;

    void Start()
    {
        if (GameManager.Instance != null && timerSlider != null)
        {
            timerSlider.maxValue = GameManager.Instance.gameTime;
            timerSlider.value = GameManager.Instance.gameTime;
        }
    }

    void Update()
    {
        // GameManager의 정제된 시간을 가져와 슬라이더에 표시만 함 (직접 시간을 깎지 않음!)
        if (GameManager.Instance != null && timerSlider != null)
        {
            timerSlider.value = GameManager.Instance.gameTime;
        }
    }
}