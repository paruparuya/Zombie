using UnityEngine;
using UnityEngine.InputSystem;

public class CarInteraction : MonoBehaviour
{
    public string requiredKeyID = "CarKey"; // 鍵のID
    [SerializeField] private GameManager gameManager;

    
    public void TryInteract()
    {
        

        if (InventoryManager.Instance.HasItemWithID(requiredKeyID))
        {
            gameManager.ShowMessage("脱出できた");
            gameManager.TriggerGameClear(); // ゲームクリア処理（UI表示・シーン切り替えなど）
        }
        else
        {
            gameManager.ShowMessage("鍵が必要なようだ");
        }
    }
}
