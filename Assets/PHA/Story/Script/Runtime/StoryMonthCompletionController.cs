using UnityEngine;

public class StoryMonthCompletionController : MonoBehaviour
{
    [Header("씬 전환")]
    [SerializeField]
    private SceneChange sceneChange;

    [SerializeField]
    private string lobbySceneName = "LobbyScene";

    [Header("월 진행")]
    [Min(1)]
    [SerializeField]
    private int maxMonth = 12;

    private bool isTransitioning;

    /// <summary>
    /// 해당 월의 모든 스토리가 종료된 뒤 호출합니다.
    /// 현재 월을 증가시키고 저장한 다음 로비 씬으로 이동합니다.
    /// </summary>
    public void CompleteMonthAndReturnToLobby()
    {
        if (isTransitioning)
        {
            return;
        }

        isTransitioning = true;

        if (Datamanager.Instance == null)
        {
            Debug.LogError(
                "Datamanager.Instance가 없습니다.",
                this
            );

            isTransitioning = false;
            return;
        }

        var saveData =
            Datamanager.Instance.saveData;

        if (saveData == null ||
            saveData.progress == null)
        {
            Debug.LogError(
                "저장 데이터의 progress가 없습니다.",
                this
            );

            isTransitioning = false;
            return;
        }

        int previousMonth =
            saveData.progress.currentStage;

        if (saveData.progress.currentStage < maxMonth)
        {
            saveData.progress.currentStage++;
        }
        else
        {
            saveData.progress.currentStage =
                maxMonth;

            Debug.Log(
                $"마지막 월({maxMonth}월)에 도달했습니다.",
                this
            );
        }

        Datamanager.Instance.SaveGameData();

        Debug.Log(
            $"월 진행 완료: " +
            $"{previousMonth}월 → " +
            $"{saveData.progress.currentStage}월",
            this
        );

        LoadLobbyScene();
    }

    private void LoadLobbyScene()
    {
        if (string.IsNullOrWhiteSpace(
                lobbySceneName))
        {
            Debug.LogError(
                "로비 씬 이름이 비어 있습니다.",
                this
            );

            isTransitioning = false;
            return;
        }

        if (sceneChange == null)
        {
            sceneChange =
                GetComponent<SceneChange>();
        }

        if (sceneChange == null)
        {
            Debug.LogError(
                "SceneChange가 연결되지 않았습니다.",
                this
            );

            isTransitioning = false;
            return;
        }

        sceneChange.LoadScene(
            lobbySceneName
        );
    }
}