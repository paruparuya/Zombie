using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float animationSpeedMultiplier = 1.0f; // �A�j���[�V�����p�̃X�s�[�h�W��
    private bool isPlayerControlEnabled = true;

    [SerializeField] private float rayDistance = 2f; // ���C�̒���
    [SerializeField] private float rayHeight = 1.5f; //���C�̍���
    [SerializeField] private LayerMask interactableLayer; // �A�C�e���̃��C���[���w��
    private ItemPickupUI currentUI = null;

    private Rigidbody rb;
    private MyControls controls;
    private Animator animator;

    private Vector2 moveInput;

    private void Awake()
    {
        // Rigidbody�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody>();

        // Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();

        // MyControls�̃C���X�^���X���쐬
        controls = new MyControls();

        // Input Action��o�^
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Interact.performed += OnInteractPerformed;
    }

    private void Update()
    {
        //���C�̐ݒ�
        Vector3 origin = transform.position + Vector3.up * rayHeight;�@//���C�̒��S���߁A�L�����̒��S���獂�����v�Z
        Vector3 direction = transform.forward;�@�@//���C�̌��������߂�A����͑O������

        Debug.DrawRay(origin, direction * rayDistance, Color.red);  //�f�o�b�O���C

        if (Physics.Raycast(origin, direction, out RaycastHit hit, rayDistance, interactableLayer))
        {
            GameObject target = hit.collider.gameObject;

            Debug.Log("Raycast hit: " + target.name);

            // �A�C�e����UI������Ȃ�\��
            ItemPickupUI ui = target.GetComponent<ItemPickupUI>();
            if (ui != null)
            {
                if (currentUI != ui)
                {
                    if (currentUI != null) currentUI.ShowPickupUI(false); // �O��UI���\��
                    currentUI = ui;
                    currentUI.ShowPickupUI(true);
                }
            }

            // �h�A�`�F�b�N
            /*DoorController door = target.GetComponent<DoorController>();
            currentDoor = door; // ���Ă���h�A���L���i����΁j

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

            //currentDoor = null; // �������Ă��Ȃ�
            //currentChest = null;
        }
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


    //Move�̓��͂��󂯎��ARigidbody���g���ă{�[���𓮂���
    private void FixedUpdate()
    {

        if (!isPlayerControlEnabled) return;  // ���얳���Ȃ珈�����X�L�b�v

        // �O�㍶�E�ւ̈ړ�������
        if (rb != null)
        {
            // ���E��]�iA/D�j
            float turn = moveInput.x * turnSpeed * Time.fixedDeltaTime;
            transform.Rotate(0, turn, 0); // Y���ŉ�]

            // �O��ړ��iW/S�j
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

            // WorldItem �X�N���v�g���擾
            WorldItem worldItem = itemObj.GetComponentInParent<WorldItem>();
            if (worldItem != null)
            {
                InventoryItem newItem = worldItem.CreateInventoryItem();
                InventoryManeger.Instance.AddItem(newItem); //  �C���x���g���ɒǉ�
            }
            else
            {
                Debug.LogWarning("WorldItem ��������܂���ł���");
            }

            Destroy(currentUI.gameObject); // �A�C�e�����ƍ폜
            currentUI = null;
        }

        // �h�A���ڂ̑O�ɂ���ΊJ����
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
            Debug.Log("currentChest �� null �ł���");
        }*/
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Move�A�N�V�����̒l���擾
        moveInput = context.ReadValue<Vector2>();
        
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Move�̓��͂������Ȃ�����ړ����~�߂�
        moveInput = Vector2.zero;
    }

    public void SetPlayerControl(bool enabled)
    {
        isPlayerControlEnabled = enabled;
    }
}