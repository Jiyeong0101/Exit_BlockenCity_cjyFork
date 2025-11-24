using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewQuestManager : MonoBehaviour
{
    public static NewQuestManager Instance;

    public Transform questUIParent;          // 퀘스트 UI를 붙일 부모 오브젝트.
    public GameObject questUIPrefab;         // 퀘스트를 표시할 프리팹 (NewQuestUI 스크립트 포함)

    private void Awake()
    {
        if (Instance == null) Instance = this;
        //else Destroy(gameObject);
    }

    public void AddQuest(NewQuestData quest)
    {
        GameObject questUIObj = Instantiate(questUIPrefab, questUIParent);

        // NewQuestUI 컴포넌트에 접근해서 설정
        NewQuestUI questUI = questUIObj.GetComponent<NewQuestUI>();
        if (questUI != null)
        {
            questUI.SetQuest(quest);
        }
        else
        {
            Debug.LogWarning("NewQuestUI 컴포넌트를 찾을 수 없습니다!");
        }
    }
}