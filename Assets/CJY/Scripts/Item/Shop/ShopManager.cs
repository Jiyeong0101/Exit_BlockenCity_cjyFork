using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance
    {
        get;
        private set;
    }


    [Header("References")]
    [SerializeField]
    private ItemInventory inventory;


    [Header("Favor Product")]
    [Min(0)]
    [SerializeField]
    private int favorPrice = 1000;

    [Min(0f)]
    [SerializeField]
    private float favorAmount = 10f;


    public int FavorPrice
        => favorPrice;

    public float FavorAmount
        => favorAmount;


    public int CurrentMoney
    {
        get
        {
            return GameDataManager.Instance
                .GetMoney();
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        FindReferences();
    }


    private void Start()
    {
        FindReferences();
    }


    private void FindReferences()
    {
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
    }


    // =====================================================
    // 일반 아이템 구매
    // =====================================================

    public ShopPurchaseResult TryPurchaseItem(
        ItemData itemData)
    {
        FindReferences();


        if (itemData == null)
        {
            return ShopPurchaseResult.Fail(
                ShopPurchaseFailureReason.InvalidItem,
                "구매할 아이템 정보가 없습니다."
            );
        }


        if (inventory == null)
        {
            return ShopPurchaseResult.Fail(
                ShopPurchaseFailureReason.InvalidState,
                "아이템 인벤토리를 찾을 수 없습니다."
            );
        }


        int price =
            Mathf.Max(
                0,
                itemData.Price);


        // -------------------------
        // 재화 차감
        // -------------------------

        bool spent =
            GameDataManager.Instance
                .TrySpendMoney(price);


        if (!spent)
        {
            return ShopPurchaseResult.Fail(
                ShopPurchaseFailureReason.NotEnoughMoney,
                "보유 재화가 부족합니다."
            );
        }


        // -------------------------
        // 아이템 지급
        // -------------------------

        inventory.AddItem(
            itemData.ItemId,
            1);


        // -------------------------
        // 구매 전체 처리 후 저장 1회
        // -------------------------

        GameDataManager.Instance
            .SaveGameData();


        int newMoney =
            GameDataManager.Instance
                .GetMoney();


        Debug.Log(
            $"[Shop] 아이템 구매 성공 | " +
            $"{itemData.ItemName} | " +
            $"가격: {price} | " +
            $"남은 재화: {newMoney}"
        );


        return ShopPurchaseResult.Succeed(
            $"{itemData.ItemName} 구매 완료"
        );
    }


    // =====================================================
    // 우호도 구매
    // =====================================================

    public ShopPurchaseResult TryPurchaseFavor(
        RelationshipType faction)
    {
        if (!IsValidFaction(faction))
        {
            return ShopPurchaseResult.Fail(
                ShopPurchaseFailureReason.InvalidFaction,
                "잘못된 세력입니다."
            );
        }


        // -------------------------
        // 재화 차감
        // -------------------------

        bool spent =
            GameDataManager.Instance
                .TrySpendMoney(
                    favorPrice);


        if (!spent)
        {
            return ShopPurchaseResult.Fail(
                ShopPurchaseFailureReason.NotEnoughMoney,
                "보유 재화가 부족합니다."
            );
        }


        // -------------------------
        // 우호도 증가
        // -------------------------

        GameDataManager.Instance
            .AddRelationship(
                faction,
                favorAmount);


        // -------------------------
        // 거래 완료 후 저장 1회
        // -------------------------

        GameDataManager.Instance
            .SaveGameData();


        int newMoney =
            GameDataManager.Instance
                .GetMoney();


        float newRelationship =
            GameDataManager.Instance
                .GetRelationship(
                    faction);


        Debug.Log(
            $"[Shop] 우호도 구매 성공 | " +
            $"Faction: {faction} | " +
            $"Favor: {newRelationship} | " +
            $"남은 재화: {newMoney}"
        );


        return ShopPurchaseResult.Succeed(
            "우호도 거래가 완료되었습니다."
        );
    }


    private bool IsValidFaction(
        RelationshipType faction)
    {
        return Enum.IsDefined(
            typeof(RelationshipType),
            faction);
    }
}