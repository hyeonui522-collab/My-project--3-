using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public MissileManager missileManager;
    [Range(0f, 50f)] public float moveSpeed = 5f;
    [Range(1, 100)] public int maxHP = 10;
    [Range(1, 20)] public int atk = 1;
    [Range(0.1f, 2f)] public float muzzlePositionOffset = 0.6f;

    public int health { get; private set; }

    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        ScreenWorldBounds.Initialize(Camera.main);
        health = maxHP;

        if (missileManager == null)
        {
            Debug.LogError("PlayerController MissileManager is null");
        }
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.position = ScreenWorldBounds.ClampPosition(transform.position, 0.35f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started) return;

        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (missileManager == null) return;

        Vector3 firePosition = transform.position + Vector3.up * muzzlePositionOffset;
        missileManager.FireMissile(firePosition, Vector3.up, atk);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        OnFire(context);
    }

    public void TakeDamage(int damage)
    {
        if (health <= 0) return;

        health = Mathf.Max(health - damage, 0);
        Debug.Log("Player HP: " + health + " / " + maxHP);

        if (health <= 0)
        {
            Debug.Log("Game Over");
            gameObject.SetActive(false);
        }
    }
}
