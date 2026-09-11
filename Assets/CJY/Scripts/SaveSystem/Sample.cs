using UnityEngine;

public class Sample : MonoBehaviour
{
    [Header("UI")]
    public ScoreUIBinder scoreUIBinder;

    private void Start()
    {
        Datamanager.Instance
            .EnsureLoaded();


        GameDataManager.Instance
            .NormalizeLoadedData();


        StageManager.Instance
            .StartStage();


        scoreUIBinder?.Refresh(false);
    }


    private void OnApplicationQuit()
    {
        GameDataManager.Instance.SaveGameData();
    }


    public void NextStage()
    {
        int currentStage =
            GameDataManager.Instance.GetCurrentStage();


        // 마지막 스테이지에서는 증가하지 않음
        if (currentStage >= GameDataManager.MAX_STAGE)
        {
            return;
        }


        // 1. 이번 스테이지 보상 반영
        StageManager.Instance.ClearStage();


        // 2. 현재 스테이지 증가
        bool advanced =
            GameDataManager.Instance.AdvanceStage();

        if (!advanced)
        {
            return;
        }


        // 3. 모든 변경 완료 후 저장 한 번
        GameDataManager.Instance.SaveGameData();


        // 4. 다음 스테이지 임시 데이터 초기화
        StageManager.Instance.StartStage();


        // 5. 화면 갱신
        scoreUIBinder?.Refresh(false);
    }
}