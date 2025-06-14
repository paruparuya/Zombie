using UnityEngine;

public class ZombieKillTrigger : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController playerController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�Q�[���I�[�o�[: �]���r���Ԃ������I");
            gameManager.GameOver(); 
            playerController.ZonbeiTouch();
        }
    }
}
