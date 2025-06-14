using UnityEngine;

public class ZombieKillTrigger : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController playerController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ゲームオーバー: ゾンビがぶつかった！");
            gameManager.GameOver(); 
            playerController.ZonbeiTouch();
        }
    }
}
