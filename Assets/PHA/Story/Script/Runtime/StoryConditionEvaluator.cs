using UnityEngine;

public class StoryConditionEvaluator
{
    private readonly StoryProgressService
        progressService;

    public StoryConditionEvaluator(
        StoryProgressService progressService)
    {
        this.progressService =
            progressService;
    }

    public bool IsStoryAvailable(
        StoryData story,
        int currentMonth)
    {
        if (story == null)
        {
            return false;
        }

        // 0은 월 제한 없는 이벤트
        if (story.Month != 0 &&
            story.Month != currentMonth)
        {
            return false;
        }

        if (story.PlayOnce &&
            progressService.IsStoryCompleted(
                story.StoryId))
        {
            return false;
        }

        if (story.Conditions == null)
        {
            return true;
        }

        foreach (StoryConditionData condition
                 in story.Conditions)
        {
            if (!CheckCondition(condition))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckCondition(
        StoryConditionData condition)
    {
        if (condition == null ||
            condition.ConditionType ==
            StoryConditionType.None)
        {
            return true;
        }

        switch (condition.ConditionType)
        {
            case StoryConditionType.StoryCompleted:
                return progressService
                    .IsStoryCompleted(
                        condition.Key
                    );

            case StoryConditionType.StoryNotCompleted:
                return !progressService
                    .IsStoryCompleted(
                        condition.Key
                    );

            case StoryConditionType.FactionIntroduced:
                return progressService
                    .IsFactionIntroduced(
                        condition.Key
                    );

            case StoryConditionType.FactionNotIntroduced:
                return !progressService
                    .IsFactionIntroduced(
                        condition.Key
                    );

            case StoryConditionType.RelationshipAtLeast:
                return progressService
                    .GetRelationshipValue(
                        condition.Key
                    ) >= condition.IntValue;

            case StoryConditionType.RelationshipAtMost:
                return progressService
                    .GetRelationshipValue(
                        condition.Key
                    ) <= condition.IntValue;

            case StoryConditionType.ChoiceEquals:
                return string.Equals(
                    progressService.GetChoiceValue(
                        condition.Key
                    ),
                    condition.Value,
                    System.StringComparison
                        .OrdinalIgnoreCase
                );

            default:
                Debug.LogWarning(
                    $"처리되지 않은 조건: " +
                    $"{condition.ConditionType}"
                );

                return false;
        }
    }
}