using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2.5f;
    public float attackCooldown = 3f;

    private Rigidbody2D rb;
    private float detectionRange = 2.5f; // Range within which the enemy detects the player
    private GameObject player;
    private float attackCooldownRemaining;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (attackCooldownRemaining > 0f)
        {
            attackCooldownRemaining = Mathf.Max(0f, attackCooldownRemaining - Time.deltaTime);
        }

    }

    void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }
        if (player == null)
        {
            Debug.LogWarning("Player object not found. Ensure the player has the 'Player' tag.");
            return;
        }

        if (GameController.IsGameOver())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Move the enemy towards the player, only if within detection range
        Vector2 direction = (player.transform.position - transform.position);
        if (direction.magnitude <= detectionRange)
        {
            Vector2 movement = direction;
            movement.Normalize();
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") &&
            !GameController.IsGameOver() &&
            !GameController.IsPlayerInvulnerable &&
            attackCooldownRemaining <= 0f)
        {
            Debug.Log("Enemy attacked Player!");
            GameController.DamagePlayer();
            attackCooldownRemaining = attackCooldown;
        }
    }
}
