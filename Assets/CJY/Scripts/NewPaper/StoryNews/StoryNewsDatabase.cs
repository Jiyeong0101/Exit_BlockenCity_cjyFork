using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

[CreateAssetMenu(fileName = "StoryNewsDatabase", menuName = "NewsSystem/StoryNewsDatabase")]
public class StoryNewsDatabase : ScriptableObject
{
    public List<StoryNewsData> newsList;

    // 해당 월(month)의 기사 중 하나를 랜덤으로 뽑아주는 함수
    public StoryNewsData GetRandomNews(int month)
    {
        // newsList에서 targetMonth가 요청받은 month와 같은 기사들만 싹 모아서 새로운 리스트로 만듬
        List<StoryNewsData> monthNewsList = newsList.Where(news => news.targetMonth == month).ToList();

        // 만약 그 달에 등록된 기사가 하나도 없다면 null 반환
        if (monthNewsList.Count == 0) return null;

        // 기사가 하나 이상 있다면, 랜덤으로 인덱스를 뽑아서 반환 (1개면 무조건 0번이 나옴)
        int randomIndex = Random.Range(0, monthNewsList.Count);
        return monthNewsList[randomIndex];
    }
}
