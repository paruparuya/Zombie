using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float animationSpeedMultiplier = 1.0f; // アニメーション用のスピード係数
    private bool isPlayerControlEnabled = true;

    [SerializeField] private float rayDistance = 2f; // レイの長さ
    [SerializeField] private float rayHeight = 1.5f; //レイの高さ
    [SerializeField] private LayerMask interactableLayer; // アイテムのレイヤーを指定
    private ItemPickupUI currentUI = null;

    private Rigidbody rb;
    private MyControls controls;
    private Animator animator;

    private Vector2 moveInput;

    private void Awake()
    {
        // Rigidbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();

        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();

        // MyControlsのインスタンスを作成
        controls = new MyControls();

        // Input Actionを登録
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Interact.performed += OnInteractPerformed;
    }

    private void Update()
    {
        //レイの設定
        Vector3 origin = transform.position + Vector3.up * rayHeight;　//レイの中心決め、キャラの中心から高さを計算
        Vector3 direction = transform.forward;　　//レイの向きを決める、今回は前方向に

        Debug.DrawRay(origin, direction * rayDistance, Color.red);  //デバッグレイ

        if (Physics.Raycast(origin, direction, out RaycastHit hit, rayDistance, interactableLayer))
        {
            GameObject target = hit.collider.gameObject;

            Debug.Log("Raycast hit: " + target.name);

            // アイテムにUIがあるなら表示
            ItemPickupUI ui = target.GetComponent<ItemPickupUI>();
            if (ui != null)
            {
                if (currentUI != ui)
                {
                    if (currentUI != null) currentUI.ShowPickupUI(false); // 前のUIを非表示
                    currentUI = ui;
                    currentUI.ShowPickupUI(true);
                }
            }

            // ドアチェック
            /*DoorController door = target.GetComponent<DoorController>();
            currentDoor = door; // 見ているドアを記憶（あれば）

            ChestOpen chest = target.GetComponent<ChestOpen>();
            currentChest = chest;*/
        }
        else
        {
            if (currentUI != null)
            {
                currentUI.ShowPickupUI(false);
                currentUI = null;
            }

            //currentDoor = null; // 何も見ていない
            //currentChest = null;
        }
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


    //Moveの入力を受け取り、Rigidbodyを使ってボールを動かす
    private void FixedUpdate()
    {

        if (!isPlayerControlEnabled) return;  // 操作無効なら処理をスキップ

        // 前後左右への移動を処理
        if (rb != null)
        {
            // 左右回転（A/D）
            float turn = moveInput.x * turnSpeed * Time.fixedDeltaTime;
            transform.Rotate(0, turn, 0); // Y軸で回転

            // 前後移動（W/S）
            Vector3 move = transform.forward * moveInput.y * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);

            float animationSpeed = Mathf.Abs(moveInput.y) * animationSpeedMultiplier;
            animator.SetFloat("Speed", animationSpeed);
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (currentUI != null)
        {
            GameObject itemObj = currentUI.gameObject;

            // WorldItem スクリプトを取得
            WorldItem worldItem = itemObj.GetComponentInParent<WorldItem>();
            if (worldItem != null)
            {
                InventoryItem newItem = worldItem.CreateInventoryItem();
                InventoryManeger.Instance.AddItem(newItem); //  インベントリに追加
            }
            else
            {
                Debug.LogWarning("WorldItem が見つかりませんでした");
            }

            Destroy(currentUI.gameObject); // アイテムごと削除
            currentUI = null;
        }

        // ドアが目の前にあれば開け閉め
        /*if (currentDoor != null)
        {
            currentDoor.ToggleDoor();
        }
        if (currentChest != null)
        {
            currentChest.TryOpenChest();
        }
        else
        {
            Debug.Log("currentChest は null でした");
        }*/
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Moveアクションの値を取得
        moveInput = context.ReadValue<Vector2>();
        
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Moveの入力が無くなったら移動を止める
        moveInput = Vector2.zero;
    }

    public void SetPlayerControl(bool enabled)
    {
        isPlayerControlEnabled = enabled;
    }
}