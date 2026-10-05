using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    [Header("씬 이름 설정")]
    [SerializeField]
    private string nicknameSceneName = "Nickname";

    [SerializeField]
    private string lobbySceneName = "Lobby";


    public void CheckGameDataAndNavigate()
    {
        // =========================================
        // 세이브 파일 Load
        // =========================================

        Datamanager.Instance
            .LoadGameData();


        // =========================================
        // 로드된 데이터 범위 보정
        // =========================================

        GameDataManager.Instance
            .NormalizeLoadedData();


        // =========================================
        // 닉네임 조회
        // =========================================

        string playerName =
            GameDataManager.Instance
                .GetPlayerName();


        bool hasValidNickname =
            !string.IsNullOrEmpty(playerName) &&
            playerName != "한서안";


        // =========================================
        // 씬 이동
        // =========================================

        if (hasValidNickname)
        {
            SceneManager.LoadScene(
                lobbySceneName);
        }
        else
        {
            SceneManager.LoadScene(
                nicknameSceneName);
        }
    }
}