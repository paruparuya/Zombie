using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;  //アイコン
    [SerializeField] private TMP_Text nameText;　//名前
    [SerializeField] private TMP_Text countText;  //アイテムアイコンの右下に表示する用

    private InventoryItem storedItem;
    public string ItemName => storedItem.itemName;// ← 比較用のプロパティ

    public string ItemID => storedItem.id;


    public void Setup(InventoryItem item)
    {

        storedItem = item;
        iconImage.sprite = item.icon;
        nameText.text = item.itemName;
        UpdateCount(item.count);
    }

    public void UpdateCount(int count)
    {
        if (count > 1)
            countText.text = "×" + count;
        else
            countText.text = ""; // 1個のときは非表示にする
    }

    public void OnClick()
    {
        Debug.Log("アイテムを選択しました：" + nameText.text);

        // アイテム詳細を表示
        SelectedItemDisplay display = Object.FindFirstObjectByType<SelectedItemDisplay>();
        if (display != null)
        {
            display.ShowItem(storedItem);
        }
    }
}