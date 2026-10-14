using UnityEngine;
public class MissileManager : MonoBehaviour
{
    [Range(1, 30)] public int maxMissileCount = 10;
    public MissileController[] missiles;
    public MissileController missilePrefab;

    private void Awake()
    {
        if (missilePrefab == null)
        {
            Debug.LogError("MissileManager missilePrefab is null");
            return;
        }

        missiles = new MissileController[maxMissileCount];

        for (int i = 0; i < maxMissileCount; i++)
        {
            missiles[i] = Instantiate(missilePrefab);
            missiles[i].transform.position = Vector3.zero;
            missiles[i].transform.SetParent(transform);
            missiles[i].gameObject.SetActive(false);
        }

        missilePrefab.gameObject.SetActive(false);
    }

    public void FireMissile(Vector3 startPosition, Vector3 direction, int damage)
    {
        if (missiles == null) return;

        for (int i = 0; i < maxMissileCount; i++)
        {
            if (missiles[i].flying) continue;

            missiles[i].Fire(startPosition, direction, damage);
            break;
        }
    }
}
