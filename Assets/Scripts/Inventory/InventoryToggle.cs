using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button firstSelectable; // ← 最初に選ばせたいボタン（InventoryItemUIのボタン）

    private MyControls controls;

    private bool inventoryOpen = false;
    private bool isSwitchingInventory = false; // 入力の多重呼び出し防止用

    private void Awake()
    {
        controls = new MyControls();
        controls.Player.Inventory.performed += _ => ToggleInventory();
        controls.UI.Inventory.performed += _ => ToggleInventory();
    }

    private void Update()
    {
        if (inventoryOpen && Keyboard.current.eKey.wasPressedThisFrame)　　//ここインプットアクションに変えれたかな
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

        Debug.Log("ToggleInventory() 呼ばれた: " + inventoryOpen);

        if (inventoryOpen)
        {
            Debug.Log("インベントリ開いた → StartCoroutine 呼び出し予定");

            controls.Player.Disable();
            controls.UI.Enable();

            Object.FindFirstObjectByType<PlayerController>().SetPlayerControl(false);

            yield return null; // 1フレーム待ってから選択
            Button[] buttons = inventoryPanel.GetComponentsInChildren<Button>();
            if (buttons.Length > 0)
            {
                EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
                Debug.Log("自動で最初のボタン選択: " + buttons[0].name);
            }
            else
            {
                Debug.LogWarning("選択できるボタンが見つかりませんでした");
            }
        }
        else
        {
            controls.UI.Disable();
            controls.Player.Enable();

            Object.FindFirstObjectByType<PlayerController>().SetPlayerControl(true);
            EventSystem.current.SetSelectedGameObject(null);

            SelectedItemDisplay display = Object.FindFirstObjectByType<SelectedItemDisplay>();  //アイテム詳細画面を消す
            if (display != null)
            {
                display.Hide();
            }
        }

        yield return new WaitForSeconds(0.2f); // 連続入力防止（0.2秒だけ受付無効）
        isSwitchingInventory = false;
    }
}
