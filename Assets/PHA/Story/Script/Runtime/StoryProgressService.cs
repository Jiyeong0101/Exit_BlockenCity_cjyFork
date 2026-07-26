using System.Collections.Generic;
using UnityEngine;

public class StoryProgressService
{
    public int GetCurrentMonth()
    {
        SaveData saveData =
            Datamanager.Instance.saveData;

        if (saveData == null ||
            saveData.progress == null)
        {
            Debug.LogWarning(
                "진행 데이터가 없어 1월로 처리합니다."
            );

            return 1;
        }

        return Mathf.Clamp(
            saveData.progress.currentStage,
            1,
            12
        );
    }

    public StoryProgressData GetProgress()
    {
        SaveData saveData =
            Datamanager.Instance.saveData;

        if (saveData.story == null)
        {
            saveData.story =
                new StoryProgressData();
        }

        if (saveData.story.completedStoryIds == null)
        {
            saveData.story.completedStoryIds =
                new List<string>();
        }

        if (saveData.story.introducedFactionIds == null)
        {
            saveData.story.introducedFactionIds =
                new List<string>();
        }

        if (saveData.story.choiceResults == null)
        {
            saveData.story.choiceResults =
                new List<StoryChoiceResultData>();
        }

        return saveData.story;
    }

    public bool IsStoryCompleted(
        string storyId)
    {
        if (string.IsNullOrWhiteSpace(storyId))
        {
            return false;
        }

        return GetProgress()
            .completedStoryIds
            .Contains(storyId);
    }

    public bool IsFactionIntroduced(
        string factionId)
    {
        if (string.IsNullOrWhiteSpace(factionId))
        {
            return false;
        }

        return GetProgress()
            .introducedFactionIds
            .Contains(factionId);
    }

    public string GetChoiceValue(
        string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        StoryChoiceResultData result =
            GetProgress()
                .choiceResults
                .Find(
                    item =>
                        item != null &&
                        item.key == key
                );

        return result?.value;
    }

    public void SaveChoiceResult(
        string key,
        string value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        StoryProgressData progress =
            GetProgress();

        StoryChoiceResultData savedResult =
            progress.choiceResults.Find(
                item =>
                    item != null &&
                    item.key == key
            );

        if (savedResult == null)
        {
            savedResult =
                new StoryChoiceResultData
                {
                    key = key,
                    value = value
                };

            progress.choiceResults.Add(
                savedResult
            );
        }
        else
        {
            savedResult.value = value;
        }

        Datamanager.Instance.SaveGameData();

        Debug.Log(
            $"선택 결과 저장: {key} = {value}"
        );
    }

    public float GetRelationshipValue(
        string factionId)
    {
        SaveData saveData =
            Datamanager.Instance.saveData;

        if (saveData == null ||
            saveData.relationship == null)
        {
            return 0f;
        }

        var relationship =
            saveData.relationship;

        switch (factionId)
        {
            case "Danwol":
                return relationship.danwol;

            case "Yaseo":
                return relationship.yaseo;

            case "Macheon":
                return relationship.macheon;

            case "Hongryeon":
                return relationship.hongryeon;

            case "JeonSangYeon":
                return relationship.JeonSangYeon;

            default:
                Debug.LogWarning(
                    $"등록되지 않은 세력 ID: {factionId}"
                );

                return 0f;
        }
    }

    public void CompleteStory(
        StoryData story)
    {
        if (story == null ||
            string.IsNullOrWhiteSpace(
                story.StoryId))
        {
            return;
        }

        StoryProgressData progress =
            GetProgress();

        if (!progress.completedStoryIds.Contains(
                story.StoryId))
        {
            progress.completedStoryIds.Add(
                story.StoryId
            );
        }

        if (!string.IsNullOrWhiteSpace(
                story.UnlockFactionId) &&
            !progress.introducedFactionIds.Contains(
                story.UnlockFactionId))
        {
            progress.introducedFactionIds.Add(
                story.UnlockFactionId
            );
        }

        Datamanager.Instance.SaveGameData();

        Debug.Log(
            $"스토리 완료 저장: {story.StoryId}"
        );
    }
}