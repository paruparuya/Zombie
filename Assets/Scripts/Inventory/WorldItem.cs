using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public string category;
    public string id;

    public InventoryItem CreateInventoryItem()
    {
        InventoryItem item = new InventoryItem();
        item.itemName = itemName;
        item.description = description;
        item.icon = icon;
        item.category = category;
        item.id = id;
        return item;
    }
}