using System.Collections.Generic;
using UnityEngine;
using System.Linq; // 데이터를 쉽게 검색하기 위해 사용

[CreateAssetMenu(fileName = "WeatherNewsDatabase", menuName = "NewsSystem/WeatherNewsDatabase")]
public class WeatherNewsDatabase : ScriptableObject
{
    public List<WeatherNewsData> newsList;

    // 전달받은 날씨(ObstacleType)에 맞는 기사 데이터를 찾아 반환
    public WeatherNewsData GetNewsData(ObstacleType type)
    {
        return newsList.FirstOrDefault(news => news.targetObstacle == type);
    }
}