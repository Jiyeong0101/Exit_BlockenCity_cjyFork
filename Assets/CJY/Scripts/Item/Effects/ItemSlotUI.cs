using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    [Header("Item")]
    [SerializeField]
    private ItemData itemData;


    [Header("UI")]
    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private TMP_Text countText;

    [SerializeField]
    private Button useButton;


    [Header("Icon Alpha")]
    [Tooltip("아이템을 가지고 있을 때. 0% 투명 = Alpha 1")]
    [Range(0f, 1f)]
    [SerializeField]
    private float ownedAlpha = 1f;

    [Tooltip("아이템이 없을 때. 50% 투명 = Alpha 0.5")]
    [Range(0f, 1f)]
    [SerializeField]
    private float emptyAlpha = 0.5f;


    private ItemInventory inventory;


    private void Awake()
    {
        if (iconImage != null &&
            itemData != null)
        {
            iconImage.sprite = itemData.Icon;
        }


        if (useButton != null)
        {
            useButton.onClick.AddListener(
                HandleUseButtonClicked
            );
        }
    }


    private void Start()
    {
        BindInventory();
    }


    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnItemCountChanged
                -= HandleItemCountChanged;
        }


        if (useButton != null)
        {
            useButton.onClick.RemoveListener(
                HandleUseButtonClicked
            );
        }
    }


    private void BindInventory()
    {
        if (ItemManager.Instance == null)
        {
            Debug.LogError(
                $"[ItemSlotUI] ItemManager를 찾을 수 없습니다. " +
                $"Slot: {name}"
            );

            return;
        }


        inventory =
            ItemManager.Instance.Inventory;


        if (inventory == null)
        {
            Debug.LogError(
                $"[ItemSlotUI] ItemInventory를 찾을 수 없습니다. " +
                $"Slot: {name}"
            );

            return;
        }


        inventory.OnItemCountChanged
            += HandleItemCountChanged;


        Refresh();
    }


    private void HandleItemCountChanged(
        GameItemId itemId,
        int newCount)
    {
        if (itemData == null)
            return;


        if (itemData.ItemId != itemId)
            return;


        Refresh(newCount);
    }


    private void Refresh()
    {
        if (itemData == null ||
            inventory == null)
        {
            return;
        }


        int count =
            inventory.GetCount(
                itemData.ItemId
            );


        Refresh(count);
    }


    private void Refresh(int count)
    {
        // --------------------------
        // 수량
        // --------------------------

        if (countText != null)
        {
            countText.text =
                $"{count}";
        }


        // --------------------------
        // 아이콘
        // --------------------------

        if (iconImage != null)
        {
            if (itemData != null)
            {
                iconImage.sprite =
                    itemData.Icon;
            }


            Color color =
                iconImage.color;


            // 아이템 있음 = 0% 투명
            // 아이템 없음 = 50% 투명
            color.a =
                count > 0
                ? ownedAlpha
                : emptyAlpha;


            iconImage.color = color;
        }
    }

    private void HandleUseButtonClicked()
    {
        if (itemData == null)
            return;


        if (inventory == null)
            return;


        // 게임 종료 상태에서는 사용 불가
        if (GameManager.Instance != null &&
            GameManager.Instance.isGameEnded)
        {
            return;
        }


        // 다른 시스템에 의해 Pause 중이면 사용 불가
        if (GameManager.Instance != null &&
            GameManager.Instance.IsGamePaused())
        {
            return;
        }


        // 보유 수량 없음
        if (!inventory.HasItem(
                itemData.ItemId))
        {
            Debug.Log(
                $"[ItemUI] {itemData.ItemName}을(를) " +
                $"보유하고 있지 않습니다."
            );

            return;
        }


        if (ItemManager.Instance == null)
        {
            Debug.LogError(
                "[ItemUI] ItemManager를 찾을 수 없습니다."
            );

            return;
        }


        // ============================
        // 확인창 없이 바로 사용
        // ============================

        ItemUseResult result =
            ItemManager.Instance.TryUseItem(
                itemData.ItemId
            );


        if (result.Success)
        {
            Debug.Log(
                $"[ItemUI] 사용 성공 | " +
                $"{itemData.ItemName} | " +
                $"{result.Message}"
            );
        }
        else
        {
            Debug.LogWarning(
                $"[ItemUI] 사용 실패 | " +
                $"{itemData.ItemName} | " +
                $"Reason: {result.FailureReason} | " +
                $"{result.Message}"
            );
        }
    }
}