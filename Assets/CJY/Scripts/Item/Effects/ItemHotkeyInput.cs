using UnityEngine;

public class ItemHotkeyInput : MonoBehaviour
{
    [Header("Items")]
    [SerializeField]
    private ItemData undoItem;

    [SerializeField]
    private ItemData bottomClearItem;

    [SerializeField]
    private ItemData bombItem;


    private void Update()
    {
        // 게임 종료
        if (GameManager.Instance != null &&
            GameManager.Instance.isGameEnded)
        {
            return;
        }


        // 확인창이 열려있다면
        // 5 / 6 / 7 추가 입력 차단
        //if (itemuseconfirmui.instance != null &&
        //    itemuseconfirmui.instance.isconfirmopen)
        //{
        //    return;
        //}


        // 다른 시스템의 Pause 중에는
        // 아이템 단축키 사용 차단
        if (GameManager.Instance != null &&
            GameManager.Instance.IsGamePaused())
        {
            return;
        }


        // --------------------------
        // 5 : Undo
        // --------------------------

        if (Input.GetKeyDown(KeyCode.Alpha5) ||
            Input.GetKeyDown(KeyCode.Keypad5))
        {
            RequestItem(undoItem);
        }


        // --------------------------
        // 6 : Bottom Clear
        // --------------------------

        if (Input.GetKeyDown(KeyCode.Alpha6) ||
            Input.GetKeyDown(KeyCode.Keypad6))
        {
            RequestItem(bottomClearItem);
        }


        // --------------------------
        // 7 : Bomb
        // --------------------------

        if (Input.GetKeyDown(KeyCode.Alpha7) ||
            Input.GetKeyDown(KeyCode.Keypad7))
        {
            RequestItem(bombItem);
        }
    }


    private void RequestItem(
    ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogWarning(
                "[ItemHotkeyInput] ItemData가 연결되지 않았습니다."
            );

            return;
        }


        if (ItemManager.Instance == null)
        {
            Debug.LogError(
                "[ItemHotkeyInput] ItemManager가 없습니다."
            );

            return;
        }


        // 보유하고 있지 않으면 사용하지 않음
        if (ItemManager.Instance.GetItemCount(
                itemData.ItemId) <= 0)
        {
            Debug.Log(
                $"[ItemHotkeyInput] " +
                $"{itemData.ItemName}을(를) 보유하고 있지 않습니다."
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
                $"[ItemHotkeyInput] 사용 성공 | " +
                $"{itemData.ItemName} | " +
                $"{result.Message}"
            );
        }
        else
        {
            Debug.LogWarning(
                $"[ItemHotkeyInput] 사용 실패 | " +
                $"{itemData.ItemName} | " +
                $"Reason: {result.FailureReason} | " +
                $"{result.Message}"
            );
        }
    }
}