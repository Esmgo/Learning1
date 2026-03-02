using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region 单例实现
    public static EnemyManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Tooltip("关卡配置")]
    [SerializeField] private LevelConfiguration levelConfig;

    [Header("地图范围")]
    [SerializeField] private float mapWidth;
    [SerializeField] private float mapHeight;

    [Header("生成设置")]
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float spawnAreaRadius = 2f;
    [SerializeField] private float spawnIndicatorDuration = 1f;
    [SerializeField] private GameObject spawnIndicatorPrefab;
    [SerializeField] private float waveTime = 60; // 每波时间

    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine _spawnCoroutine;
    private ObjectPool indicatorPool;
    private int currentWave = 1;

    public float waveTimeTimer = 999f;

    public async void Init()
    {
        indicatorPool = ObjectPoolManager.Instance.CreatePool("Indicator", spawnIndicatorPrefab, 10);
        waveTimeTimer = 999;
        foreach (SummonConfiguration config in levelConfig.levelConfig)
        {
            ObjectPoolManager.Instance.CreatePool(config.enemyConfig.enemyName, await ResourceManager.Instance.LoadResourceAsync<GameObject>(config.enemyConfig.address, "EnemyPrefab"));
        }
    }

    private void Update()
    {
        if (waveTimeTimer != 999 && waveTimeTimer <= waveTime)
        {
            waveTimeTimer += Time.deltaTime;

            // 如果计时器超过设定时间，立即清除敌人并停止生成
            if (waveTimeTimer > waveTime)
            {
                waveTimeTimer = 999; // 重置计时器
                ClearAllEnemy();
                StopSpawnEnemy();
                StartCoroutine(EndWaveAfterDelay());
            }
        }
    }

    /// <summary>
    /// 开始生成敌人
    /// </summary>
    public void StartSpawnEnemy()
    {
        if (_spawnCoroutine == null)
        {
            _spawnCoroutine = StartCoroutine(SpawnLoop());
        }
    }

    public void StopSpawnEnemy()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        Debug.Log($"开始第{currentWave}波");
        waveTimeTimer = 0f;
        List<SummonConfiguration> availableEnemies = levelConfig.levelConfig.Where(config => config.spawnWave <= currentWave).ToList();
        while (true)
        {
            SummonConfiguration enemyToSpawn = availableEnemies[Random.Range(0, availableEnemies.Count)];
            int spawnCount = Random.Range(enemyToSpawn.minNumber, enemyToSpawn.maxNumber + 1);

            yield return new WaitForSeconds(spawnInterval);

            if (waveTimeTimer > waveTime) break;

            Vector2 randomCenter = new Vector2(
                Random.Range(-mapWidth / 2 + spawnAreaRadius, mapWidth / 2 - spawnAreaRadius),
                Random.Range(-mapHeight / 2 + spawnAreaRadius, mapHeight / 2 - spawnAreaRadius)
            );

            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 spawnPosition = randomCenter + Random.insideUnitCircle * spawnAreaRadius;
                StartCoroutine(SpawnWithIndicatorRoutine(enemyToSpawn.enemyConfig, spawnPosition));
            }
        }
    }

    private IEnumerator SpawnWithIndicatorRoutine(EnemyConfiguration enemyToSpawn, Vector2 position)
    {
        GameObject indicator = indicatorPool.GetObject();
        if (indicator != null)
        {
            indicator.transform.position = position;
        }
        yield return new WaitForSeconds(spawnIndicatorDuration);

        if (indicator != null)
        {
            indicatorPool.ReturnObject(indicator);
        }

        GameObject enemyObject = ObjectPoolManager.Instance.GetPool(enemyToSpawn.enemyName).GetObject();
        activeEnemies.Add(enemyObject);
        if (enemyObject != null)
        {
            enemyObject.transform.position = position;
            enemyObject.GetComponent<SquareComponent>().Init(enemyToSpawn);
        }
    }

    private IEnumerator EndWaveAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        EndWave();
    }

    private async void EndWave()
    {
        UIManager.Instance.ClosePanel("FightUIPanel");
        await UIManager.Instance.OpenPanelAsync<ShopUIPanel>("ShopUIPanel");
    }

    private void ClearAllEnemy()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<PooledObject>()?.ReturnToPool();
            }
        }
        activeEnemies.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position; // 以EnemyManager的位置为中心
        Vector3 size = new Vector3(mapWidth, mapHeight, 0);
        Gizmos.DrawWireCube(center, size);
    }
}