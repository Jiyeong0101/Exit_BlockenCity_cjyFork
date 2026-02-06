using System;
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

        if (!File.Exists(path))
            return;

        try
        {
            string wrapperJson = File.ReadAllText(path);
            SaveFileWrapper wrapper = JsonUtility.FromJson<SaveFileWrapper>(wrapperJson);

            string hashCheck = SaveCrypto.ComputeHash(wrapper.data);
            if (hashCheck != wrapper.hash)
            {
                Debug.LogWarning("저장 데이터가 변조되었습니다.");
                return;
            }

            string json = SaveCrypto.Decrypt(wrapper.data);
            saveData = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("암호화 로드 완료");
            Debug.Log(path);
        }
        catch (Exception e)
        {
            Debug.LogError("저장 데이터 로드 실패: " + e.Message);
        }
    }


    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(saveData, true);

        byte[] encrypted = SaveCrypto.Encrypt(json);
        string hash = SaveCrypto.ComputeHash(encrypted);

        SaveFileWrapper wrapper = new SaveFileWrapper
        {
            data = encrypted,
            hash = hash
        };

        string wrapperJson = JsonUtility.ToJson(wrapper, true);
        string path = Application.persistentDataPath + "/" + GameDataFileName;

        File.WriteAllText(path, wrapperJson);
        Debug.Log("암호화 저장 완료");
    }

}
