using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrol : MonoBehaviour
{
    [Header("�]���r�s���͈͂̐ݒ�")]
    public float chaseRadius = 5f;        // �v���C���[��ǂ��͈�
    public float returnThreshold = 0.5f;  // ���̈ʒu�ɖ߂����Ɣ��肷��臒l

    private Vector3 initialPosition;      // �]���r�̏����ʒu
    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        initialPosition = transform.position;     // �����ʒu�L�^
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // SphereCollider��ݒ�i�g���K�[�Ƃ��Ďg���j
        SphereCollider col = GetComponent<SphereCollider>();
        if (col == null)
        {
            col = gameObject.AddComponent<SphereCollider>();
        }
        col.radius = chaseRadius;
        col.isTrigger = true;
    }

    void Update()
    {
        if (isChasing)
        {
            // �v���C���[��ǐ�
            agent.SetDestination(player.position);
        }
        else
        {
            // �����ʒu�ɖ߂�
            if (Vector3.Distance(transform.position, initialPosition) > returnThreshold)
            {
                agent.SetDestination(initialPosition);
            }
            else
            {
                agent.ResetPath();
            }
        }
    }

    // �v���C���[���͈͂ɓ�������ǐՊJ�n
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
        }
    }

    // �v���C���[���͈͂���o����ǐՏI��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
        }
    }

    // �f�o�b�O�p�M�Y��
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
