using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Range(1, 20)] public int maxHealth = 3;
    [Range(1, 10)] public int attack = 1;

    private int health;
    private float moveSpeed;
    private Vector3 moveDirection;
    private Rigidbody2D body;
    private Transform target;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetColor(Color color)
    {
        if (spriteRenderer == null) return;
        spriteRenderer.color = color;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void FixedUpdate()
    {
        if (target == null) return;
        if (body == null) return;

        moveDirection = target.position - transform.position;
        moveDirection.Normalize();

        body.MovePosition(
            transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime
        );
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player == null) return;

        player.TakeDamage(attack);
        gameObject.SetActive(false);
    }
}
