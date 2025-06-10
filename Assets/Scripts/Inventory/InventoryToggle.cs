using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button firstSelectable; // �� �ŏ��ɑI�΂������{�^���iInventoryItemUI�̃{�^���j

    private MyControls controls;

    private bool inventoryOpen = false;
    private bool isSwitchingInventory = false; // ���͂̑��d�Ăяo���h�~�p

    private void Awake()
    {
        controls = new MyControls();
        controls.Player.Inventory.performed += _ => ToggleInventory();
        controls.UI.Inventory.performed += _ => ToggleInventory();
    }

    private void Update()
    {
        if (inventoryOpen && Keyboard.current.eKey.wasPressedThisFrame)�@�@//�����C���v�b�g�A�N�V�����ɕς��ꂽ����
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (selected != null)
            {
                var itemUI = selected.GetComponent<InventoryItemUI>();
                if (itemUI != null)
                {
                    itemUI.OnClick();
                }
            }
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    private void ToggleInventory()
    {
        if (isSwitchingInventory) return;
        StartCoroutine(InventoryToggleRoutine());
    }

    private IEnumerator InventoryToggleRoutine()
    {
        isSwitchingInventory = true;

        inventoryOpen = !inventoryOpen;
        inventoryPanel.SetActive(inventoryOpen);

        Debug.Log("ToggleInventory() �Ă΂ꂽ: " + inventoryOpen);

        if (inventoryOpen)
        {
            Debug.Log("�C���x���g���J���� �� StartCoroutine �Ăяo���\��");

            controls.Player.Disable();
            controls.UI.Enable();

            Object.FindFirstObjectByType<PlayerController>().SetPlayerControl(false);

            yield return null; // 1�t���[���҂��Ă���I��
            Button[] buttons = inventoryPanel.GetComponentsInChildren<Button>();
            if (buttons.Length > 0)
            {
                EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
                Debug.Log("�����ōŏ��̃{�^���I��: " + buttons[0].name);
            }
            else
            {
                Debug.LogWarning("�I���ł���{�^����������܂���ł���");
            }
        }
        else
        {
            controls.UI.Disable();
            controls.Player.Enable();

            Object.FindFirstObjectByType<PlayerController>().SetPlayerControl(true);
            EventSystem.current.SetSelectedGameObject(null);

            SelectedItemDisplay display = Object.FindFirstObjectByType<SelectedItemDisplay>();  //�A�C�e���ڍ׉�ʂ�����
            if (display != null)
            {
                display.Hide();
            }
        }

        yield return new WaitForSeconds(0.2f); // �A�����͖h�~�i0.2�b������t�����j
        isSwitchingInventory = false;
    }
}
