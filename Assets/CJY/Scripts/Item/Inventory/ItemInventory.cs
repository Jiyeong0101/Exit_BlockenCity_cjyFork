using System;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public event Action<GameItemId, int>
        OnItemCountChanged;


    // =====================================================
    // Get
    // =====================================================

    public int GetCount(
        GameItemId itemId)
    {
        return GameDataManager.Instance
            .GetItemCount(itemId);
    }


    public bool HasItem(
        GameItemId itemId,
        int amount = 1)
    {
        if (amount <= 0)
        {
            return true;
        }


        return GetCount(itemId) >= amount;
    }


    // =====================================================
    // Add
    // =====================================================

    public void AddItem(
        GameItemId itemId,
        int amount = 1)
    {
        if (amount <= 0)
        {
            return;
        }


        GameDataManager.Instance.AddItem(
            itemId,
            amount);


        int newCount =
            GetCount(itemId);


        OnItemCountChanged?.Invoke(
            itemId,
            newCount);
    }


    // =====================================================
    // Consume
    // =====================================================

    public bool TryConsume(
        GameItemId itemId,
        int amount = 1)
    {
        bool success =
            GameDataManager.Instance
                .TryConsumeItem(
                    itemId,
                    amount);


        if (!success)
        {
            return false;
        }


        int newCount =
            GetCount(itemId);


        OnItemCountChanged?.Invoke(
            itemId,
            newCount);


        return true;
    }


    // =====================================================
    // Set
    // =====================================================

    public void SetCount(
        GameItemId itemId,
        int count)
    {
        GameDataManager.Instance
            .SetItemCount(
                itemId,
                count);


        int newCount =
            GetCount(itemId);


        OnItemCountChanged?.Invoke(
            itemId,
            newCount);
    }
}