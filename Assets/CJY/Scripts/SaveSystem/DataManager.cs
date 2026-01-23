using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
public class Datamanager : MonoBehaviour
{
    static GameObject container;
    static Datamanager instance;

    public static Datamanager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject("Datamanager");
                instance = container.AddComponent<Datamanager>();
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    string GameDataFileName = "GameData.json";

    public SaveData saveData = new SaveData();

    public void LoadGameData()
    {
        string path = Application.persistentDataPath + "/" + GameDataFileName;

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("불러오기 완료");

            Debug.Log("저장 파일 경로: " + Application.persistentDataPath);

        }
    }

    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(saveData, true);
        string path = Application.persistentDataPath + "/" + GameDataFileName;

        System.IO.File.WriteAllText(path, json);
        Debug.Log("저장 완료");
    }
}
