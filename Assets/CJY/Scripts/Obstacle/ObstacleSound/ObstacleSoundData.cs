using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ObstacleSoundData",
    menuName = "Obstacle/Sound Data")]
public class ObstacleSoundData : ScriptableObject
{
    [Serializable]
    public struct Entry
    {
        public ObstacleType type;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;
    }

    [SerializeField]
    private List<Entry> soundList;

    private Dictionary<ObstacleType, Entry> soundDict;


    // =============================================
    // 초기화
    // =============================================

    public void Initialize()
    {
        soundDict =
            new Dictionary<ObstacleType, Entry>();

        foreach (Entry entry in soundList)
        {
            if (soundDict.ContainsKey(entry.type))
                continue;

            soundDict.Add(
                entry.type,
                entry);
        }
    }


    // =============================================
    // AudioClip 가져오기
    // =============================================

    public AudioClip GetClip(
        ObstacleType type)
    {
        if (soundDict == null)
        {
            Initialize();
        }


        if (soundDict.TryGetValue(
                type,
                out Entry entry))
        {
            return entry.clip;
        }


        return null;
    }


    // =============================================
    // 개별 Volume 가져오기
    // =============================================

    public float GetVolume(
        ObstacleType type)
    {
        if (soundDict == null)
        {
            Initialize();
        }


        if (soundDict.TryGetValue(
                type,
                out Entry entry))
        {
            return Mathf.Clamp01(
                entry.volume);
        }


        // 데이터가 없을 경우 기본 볼륨
        return 1f;
    }
}