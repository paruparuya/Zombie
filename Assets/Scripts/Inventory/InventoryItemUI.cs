using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;  //�A�C�R��
    [SerializeField] private TMP_Text nameText;�@//���O
    [SerializeField] private TMP_Text countText;  //�A�C�e���A�C�R���̉E���ɕ\������p

    private InventoryItem storedItem;
    public string ItemName => storedItem.itemName;// �� ��r�p�̃v���p�e�B

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
            countText.text = "�~" + count;
        else
            countText.text = ""; // 1�̂Ƃ��͔�\���ɂ���
    }

    public void OnClick()
    {
        Debug.Log("�A�C�e����I�����܂����F" + nameText.text);

        // �A�C�e���ڍׂ�\��
        SelectedItemDisplay display = Object.FindFirstObjectByType<SelectedItemDisplay>();
        if (display != null)
        {
            display.ShowItem(storedItem);
        }
    }
}