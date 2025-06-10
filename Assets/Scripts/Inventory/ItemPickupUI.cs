using TMPro;
using UnityEngine;

public class ItemPickupUI : MonoBehaviour
{
    [SerializeField] private Canvas pickupCanvas;
    [SerializeField] private TextMeshProUGUI pickupText;

    private void Start()
    {
        ShowPickupUI(false); // �����͔�\��
    }

    public void ShowPickupUI(bool show)
    {
        if (pickupCanvas != null)
        {
            pickupCanvas.gameObject.SetActive(show);
        }
    }
}