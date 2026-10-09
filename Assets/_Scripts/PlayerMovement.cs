using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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

        if (Gamepad.current != null && Gamepad.current.leftStick.ReadValue().sqrMagnitude > movement.sqrMagnitude)
        {
            movement = Gamepad.current.leftStick.ReadValue();
        }

        movement.Normalize();

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
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
}
