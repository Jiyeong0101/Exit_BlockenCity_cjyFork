using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewsBookUI : MonoBehaviour
{
    [System.Serializable]
    public class NewsSlot
    {
        public StoryNewsData newsData;     // 신문 데이터
        public GameObject newsImageObject; // 미리 배치해 둔 해당 신문의 UI 오브젝트
    }

    [Header("도감에 연결된 전체 신문 오브젝트 목록")]
    [SerializeField] private List<NewsSlot> newsSlots;

    [Header("신문 스크랩 팝업 UI")]
    [SerializeField] private GameObject newsPanel; // 신문 UI 전체 부모 객체
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI dateText;

    private void OnEnable()
    {
        // 창이 활성화될 때마다 이벤트 등록 및 UI 갱신
        InitSlotButtons();
        UpdateBookUI();
    }

    /// <summary>
    /// 각 신문 오브젝트의 Button 컴포넌트에 클릭 이벤트를 등록합니다.
    /// </summary>
    private void InitSlotButtons()
    {
        if (newsSlots == null || newsSlots.Count == 0)
        {
            Debug.LogError("[NewsBookUI] newsSlots 리스트가 비어있습니다! Inspector를 확인해주세요.");
            return;
        }

        foreach (var slot in newsSlots)
        {
            if (slot.newsImageObject == null)
            {
                Debug.LogWarning("[NewsBookUI] newsImageObject가 연결되지 않은 슬롯이 있습니다.");
                continue;
            }

            // 자식 오브젝트에 Button이 붙어있어도 찾을 수 있도록 GetComponentInChildren 사용
            Button button = slot.newsImageObject.GetComponentInChildren<Button>();

            if (button == null)
            {
                Debug.LogError($"[NewsBookUI] '{slot.newsImageObject.name}' 오브젝트 또는 자식에 Button 컴포넌트가 없습니다!");
                continue;
            }

            StoryNewsData data = slot.newsData; // 람다 캡처

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                Debug.Log($"[NewsBookUI] '{slot.newsImageObject.name}' 클릭됨! (Data 존재: {data != null})");
                OpenNewsPopup(data);
            });
        }
    }

    /// <summary>
    /// [데모버전] 저장 데이터와 상관없이 모든 신문 오브젝트를 활성화합니다.
    /// </summary>
    public void UpdateBookUI()
    {
        foreach (var slot in newsSlots)
        {
            if (slot.newsImageObject == null) continue;
            slot.newsImageObject.SetActive(true);
        }
    }

    /// <summary>
    /// 클릭한 신문의 데이터를 팝업 UI에 채우고 켜줍니다.
    /// </summary>
    public void OpenNewsPopup(StoryNewsData data)
    {
        if (newsPanel == null)
        {
            Debug.LogError("[NewsBookUI] newsPanel이 Inspector에 연결되지 않았습니다!");
            return;
        }

        // 데이터가 없더라도 일단 패널은 켜서 작동 여부 확인
        newsPanel.SetActive(true);
        newsPanel.transform.SetAsLastSibling(); // 다른 UI 뒤에 가려지지 않도록 맨 앞으로 이동

        if (data != null)
        {
            if (titleText != null) titleText.text = data.title;
            if (contentText != null) contentText.text = data.content;

            if (iconImage != null)
            {
                iconImage.sprite = data.icon;
                iconImage.gameObject.SetActive(data.icon != null);
            }

            if (dateText != null)
            {
                dateText.text = $"{data.targetMonth:D2}.xx";
            }
        }
        else
        {
            Debug.LogWarning("[NewsBookUI] 데이터(newsData)가 null 상태로 팝업이 열렸습니다.");
        }
    }

    public void CloseNewsPopup()
    {
        if (newsPanel != null)
        {
            newsPanel.SetActive(false);
        }
    }
}