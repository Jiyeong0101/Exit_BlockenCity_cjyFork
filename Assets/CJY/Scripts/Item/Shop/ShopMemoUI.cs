using UnityEngine;
using TMPro;

public class ShopMemoUI : MonoBehaviour
{
    [Header("Money")]
    [SerializeField]
    private TMP_Text moneyText;


    [Header("Item Count")]
    [SerializeField]
    private TMP_Text undoCountText;

    [SerializeField]
    private TMP_Text bottomCountText;

    [SerializeField]
    private TMP_Text bombCountText;


    [Header("References")]
    [SerializeField]
    private ItemInventory inventory;


    private bool moneyBound = false;
    private bool inventoryBound = false;


    private void OnEnable()
    {
        TryBind();

        RefreshAll();
    }


    private void Start()
    {
        TryBind();

        RefreshAll();
    }


    private void OnDisable()
    {
        Unbind();
    }


    private void TryBind()
    {
        // =========================================
        // 재화 이벤트 연결
        // =========================================

        if (!moneyBound)
        {
            GameDataManager.Instance.OnMoneyChanged
                += HandleMoneyChanged;

            moneyBound = true;
        }


        // =========================================
        // ItemInventory 찾기
        // =========================================

        if (inventory == null &&
            ItemManager.Instance != null)
        {
            inventory =
                ItemManager.Instance.Inventory;
        }


        if (inventory == null)
        {
            inventory =
                FindObjectOfType<ItemInventory>();
        }


        // =========================================
        // 아이템 이벤트 연결
        // =========================================

        if (!inventoryBound &&
            inventory != null)
        {
            inventory.OnItemCountChanged
                += HandleItemCountChanged;

            inventoryBound = true;
        }
    }


    private void Unbind()
    {
        // =========================================
        // 재화 이벤트 해제
        // =========================================

        if (moneyBound)
        {
            GameDataManager.Instance.OnMoneyChanged
                -= HandleMoneyChanged;

            moneyBound = false;
        }


        // =========================================
        // 아이템 이벤트 해제
        // =========================================

        if (inventoryBound &&
            inventory != null)
        {
            inventory.OnItemCountChanged
                -= HandleItemCountChanged;

            inventoryBound = false;
        }
    }


    // =============================================
    // 재화 변경
    // =============================================

    private void HandleMoneyChanged(
        int newMoney)
    {
        RefreshMoney(
            newMoney
        );
    }


    // =============================================
    // 아이템 수량 변경
    // =============================================

    private void HandleItemCountChanged(
        GameItemId itemId,
        int newCount)
    {
        switch (itemId)
        {
            case GameItemId.UndoLastBlock:

                SetCountText(
                    undoCountText,
                    newCount
                );

                break;


            case GameItemId.BottomLayerClear:

                SetCountText(
                    bottomCountText,
                    newCount
                );

                break;


            case GameItemId.Bomb3x3:

                SetCountText(
                    bombCountText,
                    newCount
                );

                break;
        }
    }


    // =============================================
    // 전체 UI 갱신
    // =============================================

    public void RefreshAll()
    {
        // -----------------------------------------
        // 재화
        // -----------------------------------------

        int money =
            GameDataManager.Instance.GetMoney();


        RefreshMoney(
            money
        );


        // -----------------------------------------
        // 아이템
        // -----------------------------------------

        if (inventory == null)
        {
            TryBind();
        }


        if (inventory == null)
            return;


        SetCountText(
            undoCountText,
            inventory.GetCount(
                GameItemId.UndoLastBlock
            )
        );


        SetCountText(
            bottomCountText,
            inventory.GetCount(
                GameItemId.BottomLayerClear
            )
        );


        SetCountText(
            bombCountText,
            inventory.GetCount(
                GameItemId.Bomb3x3
            )
        );
    }


    // =============================================
    // 재화 UI 갱신
    // =============================================

    private void RefreshMoney(
        int money)
    {
        if (moneyText == null)
            return;


        moneyText.text =
            money.ToString("N0");
    }


    // =============================================
    // 아이템 수량 UI 갱신
    // =============================================

    private void SetCountText(
        TMP_Text text,
        int count)
    {
        if (text == null)
            return;


        text.text =
            count.ToString();
    }
}