using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryManeger : MonoBehaviour
{
    public static InventoryManeger Instance; // シングルトンにして他スクリプトから呼びやすく

    public List<InventoryItem> items = new List<InventoryItem>(); // 保存するアイテムリスト

    [Header("UI 関連")]
    [SerializeField] private Transform itemGrid; // GridのTransform
    [SerializeField] private GameObject itemUIPrefab; // InventoryItemUIのプレハブ

    [HideInInspector] public Button lastCreatedButton;

    private void Awake()
    {
        if (Instance == null)　　//インベントリをひとつだけにする仕組み
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(InventoryItem newItem)
    {
        // すでに持っているかチェック（名前で判定）
        InventoryItem existingItem = items.Find(i => i.itemName == newItem.itemName);
        if (existingItem != null)
        {
            existingItem.count++;
            Debug.Log($"{newItem.itemName} を追加（現在: {existingItem.count}個）");

            // UI更新：リストから該当UIを探す
            InventoryItemUI[] uiList = itemGrid.GetComponentsInChildren<InventoryItemUI>();
            foreach (var ui in uiList)
            {
                if (ui.ItemName == newItem.itemName)
                {
                    ui.UpdateCount(existingItem.count); // ← UIを更新
                    return;
                }
            }
        }
        else
        {
            items.Add(newItem);
            Debug.Log($"{newItem.itemName} をインベントリに追加しました");

            // UIに新しく追加
            GameObject uiObj = Instantiate(itemUIPrefab, itemGrid);
            InventoryItemUI ui = uiObj.GetComponent<InventoryItemUI>();
            if (ui != null)
            {
                ui.Setup(newItem);
            }

            Button b = uiObj.GetComponent<Button>();
            if (b != null)
            {
                lastCreatedButton = b;
            }
        }
    }

    public bool HasItemWithID(string id)
    {
        return items.Any(item => item.id == id);
    }
    void Update()
    {

    }

    public void RemoveItemByID(string id)
    {
        InventoryItem itemToRemove = items.Find(item => item.id == id);
        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);

            InventoryItemUI[] uis = itemGrid.GetComponentsInChildren<InventoryItemUI>();
            foreach (InventoryItemUI ui in uis)
            {
                if (ui.ItemID == id)
                {
                    Destroy(ui.gameObject);
                    break;
                }
            }

            Debug.Log($"{id} をインベントリから削除しました");
        }
    }
}
