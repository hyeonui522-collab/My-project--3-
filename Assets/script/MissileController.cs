using UnityEngine;

public class MissileController : MonoBehaviour
{
    [Range(5f, 30f)] public float moveSpeed = 10f;

    private bool fired;
    public bool flying => fired;
    public bool readyToMove => !fired;

    private int damage;
    private Vector3 moveDirection = Vector3.zero;

    private void Update()
    {
        if (!fired) return;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (ScreenWorldBounds.IsOutside(transform.position, 1f))
        {
            ReturnToPool();
        }
    }

    public void Fire(Vector3 firePosition, Vector3 fireDirection, int attackDamage)
    {
        if (flying) return;
        if (fireDirection == Vector3.zero) return;

        gameObject.SetActive(true);
        transform.position = firePosition;
        moveDirection = fireDirection;
        damage = attackDamage;
        fired = true;
    }

    public void ReturnToPool()
    {
        fired = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
        if (enemy == null) return;

        enemy.TakeDamage(damage);
        ReturnToPool();
    }
}
