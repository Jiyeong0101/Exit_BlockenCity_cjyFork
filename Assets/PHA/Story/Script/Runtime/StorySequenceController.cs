using System.Collections.Generic;
using UnityEngine;

public class StorySequenceController
    : MonoBehaviour
{
    [Header("전체 스토리 데이터")]
    [SerializeField]
    private List<StoryData> storyDatabase =
        new();

    private readonly Queue<StoryData>
        storyQueue = new();

    private StoryProgressService
        progressService;

    private StoryConditionEvaluator
        conditionEvaluator;

    public bool IsPlayingSequence { get; private set; }

    public bool HasNextStory => storyQueue.Count > 0;

    public void Initialize(
        StoryProgressService service)
    {
        progressService = service;

        conditionEvaluator =
            new StoryConditionEvaluator(
                progressService
            );
    }

    public bool BuildCurrentSequence()
    {
        EnsureInitialized();

        int currentMonth =
            progressService.GetCurrentMonth();

        storyQueue.Clear();

        List<StoryData> beforeEvents =
            new();

        List<StoryData> monthlyCandidates =
            new();

        List<StoryData> afterEvents =
            new();

        foreach (StoryData story
                 in storyDatabase)
        {
            if (!conditionEvaluator
                    .IsStoryAvailable(
                        story,
                        currentMonth
                    ))
            {
                continue;
            }

            switch (story.PlayTiming)
            {
                case StoryPlayTiming.BeforeMonthly:
                    beforeEvents.Add(story);
                    break;

                case StoryPlayTiming.Monthly:
                    monthlyCandidates.Add(story);
                    break;

                case StoryPlayTiming.AfterMonthly:
                    afterEvents.Add(story);
                    break;
            }
        }

        SortByPriority(beforeEvents);
        SortByPriority(monthlyCandidates);
        SortByPriority(afterEvents);

        foreach (StoryData story
                 in beforeEvents)
        {
            storyQueue.Enqueue(story);
        }

        // 월 메인 스토리는 하나만 선택
        if (monthlyCandidates.Count > 0)
        {
            storyQueue.Enqueue(
                monthlyCandidates[0]
            );
        }
        else
        {
            Debug.LogWarning(
                $"{currentMonth}월에 실행 가능한 " +
                "월별 메인 스토리가 없습니다.",
                this
            );
        }

        foreach (StoryData story
                 in afterEvents)
        {
            storyQueue.Enqueue(story);
        }

        IsPlayingSequence =
            storyQueue.Count > 0;

        if (!IsPlayingSequence)
        {
            Debug.LogWarning(
                "현재 실행 가능한 스토리가 없습니다.",
                this
            );
        }

        return IsPlayingSequence;
    }

    public bool TryGetNextStory(
        out StoryData story)
    {
        if (storyQueue.Count == 0)
        {
            story = null;
            IsPlayingSequence = false;

            return false;
        }

        story = storyQueue.Dequeue();

        return true;
    }

    public void StopSequence()
    {
        storyQueue.Clear();
        IsPlayingSequence = false;
    }

    private void SortByPriority(
        List<StoryData> stories)
    {
        stories.Sort(
            (left, right) =>
                left.Priority.CompareTo(
                    right.Priority
                )
        );
    }

    private void EnsureInitialized()
    {
        if (progressService != null &&
            conditionEvaluator != null)
        {
            return;
        }

        Initialize(
            new StoryProgressService()
        );
    }
}