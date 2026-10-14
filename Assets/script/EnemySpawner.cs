using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pool")]
    [Range(3, 30)] public int maxEnemyCount = 12;
    public EnemyController enemyPrefab;
    public Transform parentCleaner;
    public Transform player;

    [Header("Spawn")]
    [Range(0.5f, 10f)] public float moveSpeed = 1.4f;
    [Range(0.2f, 3f)] public float enemyPositionArrangeOffset = 1.2f;
    [Range(1f, 10f)] public float spawnInterval = 5f;

    private EnemyController[] enemies;
    private float time;

    private void Start()
    {
        ScreenWorldBounds.Initialize(Camera.main);

        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner enemyPrefab is null");
            return;
        }

        if (parentCleaner == null)
        {
            Debug.LogError("EnemySpawner parentCleaner is null");
            return;
        }

        if (player == null)
        {
            Debug.LogError("EnemySpawner player is null");
            return;
        }

        enemies = new EnemyController[maxEnemyCount];

        for (int i = 0; i < maxEnemyCount; i++)
        {
            enemies[i] = Instantiate(enemyPrefab);
            enemies[i].transform.SetParent(parentCleaner);
            enemies[i].SetTarget(player);
            enemies[i].SetMoveSpeed(moveSpeed);
            enemies[i].gameObject.SetActive(false);
        }

        enemyPrefab.gameObject.SetActive(false);
        OnArrangeAllEnemiesPosition();
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time < spawnInterval) return;

        time = 0f;
        OnArrangeAllEnemiesPosition();
    }

    // 0: 가로 배열, 1: 격자 배열, 2: V자 배열
    public void OnArrangeAllEnemiesPosition()
    {
        if (enemies == null) return;

        int arrangeType = Random.Range(0, 3);
        float centerX = ScreenWorldBounds.Center.x;
        float topY = ScreenWorldBounds.MaxY - 0.7f;

        for (int i = 0; i < maxEnemyCount; i++)
        {
            if (enemies[i].gameObject.activeSelf) continue;

            Vector3 position = Vector3.zero;

            if (arrangeType == 0)
            {
                position = new Vector3(
                    centerX + (i - maxEnemyCount / 2) * enemyPositionArrangeOffset,
                    topY,
                    0f
                );
            }
            else if (arrangeType == 1)
            {
                int columnCount = 4;
                position = new Vector3(
                    centerX + (i % columnCount - 1.5f) * enemyPositionArrangeOffset,
                    topY + (i / columnCount) * enemyPositionArrangeOffset,
                    0f
                );
            }
            else
            {
                float x = (i - maxEnemyCount / 2) * enemyPositionArrangeOffset;
                float y = Mathf.Abs(x) * 0.5f;
                position = new Vector3(centerX + x, topY + y, 0f);
            }

            enemies[i].transform.position = position;
            enemies[i].SetTarget(player);
            enemies[i].SetMoveSpeed(moveSpeed);
            enemies[i].SetColor(new Color(
                Random.Range(0.5f, 1f),
                Random.Range(0.5f, 1f),
                Random.Range(0.5f, 1f)
            ));
            enemies[i].gameObject.SetActive(true);
        }
    }
}
