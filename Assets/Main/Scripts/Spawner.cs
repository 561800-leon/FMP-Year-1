
using UnityEngine;
using UnityEngine.Pool;


public class Spawner : MonoBehaviour
{
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public float timeBetweenSpawns = 20f;
    public float timeSinceLastSpawn;

    [SerializeField] public EnemyMovement enemyPrefab;
    public IObjectPool<EnemyMovement> enemyPool;


    public void Awake()
    {
        enemyPool = new ObjectPool<EnemyMovement>(CreateEnemy);
    }

    private EnemyMovement CreateEnemy()
    {
        EnemyMovement enemy = Instantiate(enemyPrefab);
        return enemy;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeSinceLastSpawn)
        {
            enemyPool.Get();
            timeSinceLastSpawn = Time.time + timeBetweenSpawns;
        }
    }
}

