using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에디터 우클릭 메뉴에서 쉽게 생성할 수 있도록 속성 추가
[CreateAssetMenu(fileName = "NewWeatherNews", menuName = "NewsSystem/WeatherNewsData")]
public class WeatherNewsData : ScriptableObject
{
    [Header("매칭 조건")]
    public ObstacleType targetObstacle;

    [Header("기사 내용")]
    public string title;

    // TextArea를 달아주면 인스펙터에서 엔터를 치며 여러 줄을 편하게 입력할 수 있습니다.
    [TextArea(3, 10)]
    public string content;

    public Sprite icon;
}