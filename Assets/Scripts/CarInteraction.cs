using UnityEngine;
using UnityEngine.InputSystem;

public class CarInteraction : MonoBehaviour
{
    public string requiredKeyID = "CarKey"; // ����ID
    [SerializeField] private GameManager gameManager;

    
    public void TryInteract()
    {
        

        if (InventoryManager.Instance.HasItemWithID(requiredKeyID))
        {
            gameManager.ShowMessage("�E�o�ł���");
            gameManager.TriggerGameClear(); // �Q�[���N���A�����iUI�\���E�V�[���؂�ւ��Ȃǁj
        }
        else
        {
            gameManager.ShowMessage("�����K�v�Ȃ悤��");
        }
    }
}
