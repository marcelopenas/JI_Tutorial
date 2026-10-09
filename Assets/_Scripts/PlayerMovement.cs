using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    [Range(0f, 1f)]
    public float damagedAlpha = 0.35f;
    [SerializeField] private AudioClip damageSound;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private float invulnerabilityVisualRemaining;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            normalColor = spriteRenderer.color;
        }
    }

    private void OnEnable()
    {
        GameController.PlayerDamaged += OnPlayerDamaged;
    }

    private void OnDisable()
    {
        GameController.PlayerDamaged -= OnPlayerDamaged;
        RestoreOpacity();
    }

    private void Update()
    {
        if (invulnerabilityVisualRemaining <= 0f)
        {
            return;
        }

        invulnerabilityVisualRemaining = Mathf.Max(0f, invulnerabilityVisualRemaining - Time.deltaTime);
        float progress = 1f - invulnerabilityVisualRemaining / GameController.PlayerInvulnerabilityDuration;
        SetOpacity(Mathf.Lerp(damagedAlpha, normalColor.a, progress));

        if (invulnerabilityVisualRemaining <= 0f)
        {
            RestoreOpacity();
        }
    }

    void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }

        if (GameController.IsGameOver())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 movement = Vector2.zero;

        // Keyboard input
        if (Keyboard.current != null)
        {
            movement = new Vector2(
                (Keyboard.current.dKey.isPressed ? 1f : 0f) -
                (Keyboard.current.aKey.isPressed ? 1f : 0f),
                (Keyboard.current.wKey.isPressed ? 1f : 0f) -
                (Keyboard.current.sKey.isPressed ? 1f : 0f));

            movement += new Vector2(
                (Keyboard.current.rightArrowKey.isPressed ? 1f : 0f) -
                (Keyboard.current.leftArrowKey.isPressed ? 1f : 0f),
                (Keyboard.current.upArrowKey.isPressed ? 1f : 0f) -
                (Keyboard.current.downArrowKey.isPressed ? 1f : 0f));
        }

        // Gamepad input
        if (Gamepad.current != null && Gamepad.current.leftStick.ReadValue().sqrMagnitude > movement.sqrMagnitude)
        {
            movement = Gamepad.current.leftStick.ReadValue();
        }
        if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().sqrMagnitude > movement.sqrMagnitude)
        {
            movement = Gamepad.current.rightStick.ReadValue();
        }
        if (Gamepad.current != null && Gamepad.current.dpad.ReadValue().sqrMagnitude > movement.sqrMagnitude)
        {
            movement = Gamepad.current.dpad.ReadValue();
        }
        if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().sqrMagnitude > movement.sqrMagnitude)
        {
            movement = Gamepad.current.rightStick.ReadValue();
        }

        movement.Normalize();

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        // Flip the sprite based on movement direction (default is facing right)
        // Also keep last facing direction when not moving
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Player collected a Coin!");
            GameController.AddCoin();
            // Deactivate coin instead of destroying it to allow for game reset
            other.gameObject.SetActive(false);
            // Destroy(other.gameObject);
            audioSource.Play();
        }
    }

    private void OnPlayerDamaged(float duration)
    {
        invulnerabilityVisualRemaining = duration;
        SetOpacity(damagedAlpha);
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    private void SetOpacity(float alpha)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        Color color = normalColor;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private void RestoreOpacity()
    {
        invulnerabilityVisualRemaining = 0f;
        SetOpacity(normalColor.a);
    }
}
