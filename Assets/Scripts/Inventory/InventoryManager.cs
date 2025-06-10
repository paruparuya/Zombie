using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryManeger : MonoBehaviour
{
    public static InventoryManeger Instance; // �V���O���g���ɂ��đ��X�N���v�g����Ăт₷��

    public List<InventoryItem> items = new List<InventoryItem>(); // �ۑ�����A�C�e�����X�g

    [Header("UI �֘A")]
    [SerializeField] private Transform itemGrid; // Grid��Transform
    [SerializeField] private GameObject itemUIPrefab; // InventoryItemUI�̃v���n�u

    [HideInInspector] public Button lastCreatedButton;

    private void Awake()
    {
        if (Instance == null)�@�@//�C���x���g�����ЂƂ����ɂ���d�g��
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(InventoryItem newItem)
    {
        // ���łɎ����Ă��邩�`�F�b�N�i���O�Ŕ���j
        InventoryItem existingItem = items.Find(i => i.itemName == newItem.itemName);
        if (existingItem != null)
        {
            existingItem.count++;
            Debug.Log($"{newItem.itemName} ��ǉ��i����: {existingItem.count}�j");

            // UI�X�V�F���X�g����Y��UI��T��
            InventoryItemUI[] uiList = itemGrid.GetComponentsInChildren<InventoryItemUI>();
            foreach (var ui in uiList)
            {
                if (ui.ItemName == newItem.itemName)
                {
                    ui.UpdateCount(existingItem.count); // �� UI���X�V
                    return;
                }
            }
        }
        else
        {
            items.Add(newItem);
            Debug.Log($"{newItem.itemName} ���C���x���g���ɒǉ����܂���");

            // UI�ɐV�����ǉ�
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

            Debug.Log($"{id} ���C���x���g������폜���܂���");
        }
    }
}
