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

        // �ŏ��͔�\��
        gameOverText.gameObject.SetActive(false);
        controls.UI.Click.performed += OnClickPerformed;
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        // Input�A�N�V������L����
        controls.Enable();
    }

    private void OnDisable()
    {
        // Input�A�N�V�����𖳌���
        controls.Disable();
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Attack���͌��o");

        if (isGameOver)
        {
            Debug.Log("�V�[�������X�^�[�g���܂�");
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // �O������Ă΂����J���\�b�h
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