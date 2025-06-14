using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float messageDuration = 3f;
    [SerializeField] private TextMeshProUGUI gameOverText;
    private bool isGameOver = false;

    private Coroutine messageCoroutine;
    private MyControls controls;

    private void Awake()
    {
        controls = new MyControls();

        // 最初は非表示
        gameOverText.gameObject.SetActive(false);
        controls.UI.Click.performed += OnClickPerformed;
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        // Inputアクションを有効化
        controls.Enable();
    }

    private void OnDisable()
    {
        // Inputアクションを無効化
        controls.Disable();
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Attack入力検出");

        if (isGameOver)
        {
            Debug.Log("シーンをリスタートします");
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // 外部から呼ばれる公開メソッド
    public void ShowMessage(string message)
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        messageCoroutine = StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        messageText.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverText.text = "GAME OVER";
        gameOverText.gameObject.SetActive(true);
    }
}