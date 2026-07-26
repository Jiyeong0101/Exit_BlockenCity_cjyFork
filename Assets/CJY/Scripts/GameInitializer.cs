using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    [Header("씬 이름 설정")]
    [SerializeField] private string nicknameSceneName = "Nickname";
    [SerializeField] private string lobbySceneName = "Lobby";

    public void CheckGameDataAndNavigate()
    {
        Datamanager.Instance.LoadGameData();

        var saveData = Datamanager.Instance.saveData;

        // 1. null 검사
        // 2. 빈 문자열 검사
        // 3. 기본 이름("Player")과 같은지 검사
        bool hasValidNickname = saveData != null &&
                                saveData.player != null &&
                                !string.IsNullOrEmpty(saveData.player.playerName) &&
                                saveData.player.playerName != "한서안"; // <--- 사용 중인 기본값 입력

        if (hasValidNickname)
        {
            SceneManager.LoadScene(lobbySceneName);
        }
        else
        {
            SceneManager.LoadScene(nicknameSceneName);
        }
    }
}