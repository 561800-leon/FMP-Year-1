using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Pool;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent navMeshAgent;
    public PlayerMovement playerMovement;
    [SerializeField] float damage = 8f;

    public IObjectPool<EnemyMovement> enemyPool;

    public void SetPool(IObjectPool<EnemyMovement> pool)
    {
        enemyPool = pool;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMovement.playerHealth = playerMovement.playerHealth - damage;
        }
    }

}
