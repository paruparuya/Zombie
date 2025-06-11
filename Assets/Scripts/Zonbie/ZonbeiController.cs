using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrol : MonoBehaviour
{
    [Header("ゾンビ行動範囲の設定")]
    public float chaseRadius = 5f;        // プレイヤーを追う範囲
    public float returnThreshold = 0.5f;  // 元の位置に戻ったと判定する閾値

    private Vector3 initialPosition;      // ゾンビの初期位置
    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        initialPosition = transform.position;     // 初期位置記録
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // SphereColliderを設定（トリガーとして使う）
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
            // プレイヤーを追跡
            agent.SetDestination(player.position);
        }
        else
        {
            // 初期位置に戻る
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

    // プレイヤーが範囲に入ったら追跡開始
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
        }
    }

    // プレイヤーが範囲から出たら追跡終了
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
        }
    }

    // デバッグ用ギズモ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
