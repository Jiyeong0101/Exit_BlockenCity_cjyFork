using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour
{
    public Slider timerSlider;
    private float maxTime;
    private float currentTime;

    private bool isTimeOver = false;

    void Start()
    {
        maxTime = GameManager.Instance.gameTime;
        currentTime = maxTime;
        timerSlider.maxValue = maxTime;
        timerSlider.value = maxTime;
    }

    void Update()
    {
        if (!isTimeOver)
        {
            if (currentTime > 0f)
            {
                currentTime -= Time.deltaTime;
                timerSlider.value = currentTime;
            }
            else
            {
                isTimeOver = true;
                GameManager.Instance.TimeOver();
            }
        }
    }
}
