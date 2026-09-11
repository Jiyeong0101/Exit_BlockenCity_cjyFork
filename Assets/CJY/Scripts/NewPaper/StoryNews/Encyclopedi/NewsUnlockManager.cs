using UnityEngine;

public static class NewsUnlockManager
{
    // =====================================================
    // Demo
    // =====================================================

    // true면 세이브 데이터와 관계없이
    // 모든 신문이 해금된 것으로 처리
    public static bool isDemoUnlockAll = false;


    // =====================================================
    // Current News Data
    // =====================================================

    private static NewsData CurrentNewsData
    {
        get
        {
            // 아직 세이브를 불러오지 않았다면
            // 최초 1회만 Load
            Datamanager.Instance
                .EnsureLoaded();


            SaveData saveData =
                Datamanager.Instance.saveData;


            if (saveData == null)
            {
                Debug.LogWarning(
                    "[NewsUnlockManager] SaveData가 없습니다."
                );

                return null;
            }


            // 예전 세이브 파일에 NewsData가
            // 존재하지 않는 경우 호환 처리
            if (saveData.news == null)
            {
                saveData.news =
                    new NewsData();
            }


            return saveData.news;
        }
    }


    // =====================================================
    // Unlock
    // =====================================================

    /// <summary>
    /// 신문을 해금합니다.
    /// 이미 해금된 신문이면 아무 작업도 하지 않습니다.
    /// </summary>
    public static void UnlockNews(
        string newsId)
    {
        if (string.IsNullOrEmpty(newsId))
        {
            return;
        }


        NewsData newsData =
            CurrentNewsData;


        if (newsData == null)
        {
            Debug.LogWarning(
                "[NewsUnlockManager] NewsData를 가져올 수 없습니다."
            );

            return;
        }


        // 이미 해금된 신문이면
        // 중복 저장하지 않음
        if (newsData.IsUnlocked(newsId))
        {
            return;
        }


        // -------------------------
        // 신문 해금
        // -------------------------

        newsData.Unlock(newsId);


        // -------------------------
        // 변경 완료 후 저장 1회
        // -------------------------

        Datamanager.Instance
            .SaveGameData();


        Debug.Log(
            $"[NewsUnlockManager] 신문 해금 완료 | {newsId}"
        );
    }


    // =====================================================
    // Check
    // =====================================================

    /// <summary>
    /// 해당 신문이 해금되어 있는지 확인합니다.
    /// </summary>
    public static bool IsUnlocked(
        string newsId)
    {
        if (string.IsNullOrEmpty(newsId))
        {
            return false;
        }


        // 데모 전체 해금
        if (isDemoUnlockAll)
        {
            return true;
        }


        NewsData newsData =
            CurrentNewsData;


        if (newsData == null)
        {
            return false;
        }


        return newsData.IsUnlocked(
            newsId);
    }
}