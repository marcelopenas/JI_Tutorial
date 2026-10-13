using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2.5f;
    public float attackCooldown = 3f;
    private float detectionRange = 2.5f; // Range within which the enemy detects the player

    // Bounce animation settings (matching the player's interval pattern)
    public float bounceInterval = 0.35f;
    public float stretchHeightMultiplier = 1.15f; // How much it stretches vertically
    private float bounceTimer;
    private bool isStretched = false;
    private Vector3 normalScale;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private float attackCooldownRemaining;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (transform != null)
        {
            normalScale = transform.localScale;
        }
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

            // Bounce animation timer when moving
            bounceTimer += Time.fixedDeltaTime;
            if (bounceTimer >= bounceInterval)
            {
                bounceTimer = 0f;
                isStretched = !isStretched;
            }

            // Apply stretch/shrink effect (squashing horizontally slightly to preserve visual volume)
            Vector3 targetScale = normalScale;
            if (isStretched)
            {
                targetScale.y = normalScale.y * stretchHeightMultiplier;
                targetScale.x = normalScale.x / stretchHeightMultiplier;
            }
            else
            {
                targetScale.y = normalScale.y / stretchHeightMultiplier;
                targetScale.x = normalScale.x * stretchHeightMultiplier;
            }
            transform.localScale = targetScale;

            // Flip the sprite based on movement direction (default is facing right)
            if (movement.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (movement.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            // Reset bounce timer and restore normal scale when stopped or out of range
            bounceTimer = 0f;
            isStretched = false;
            transform.localScale = normalScale;
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