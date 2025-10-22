using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerStats stats;
    public float moveSpeed = 5f; // placeholder value, will be set from PlayerStats
    public float dashDuration = 0.1f;
    public float deceleration = 8f;
    
    private Vector2 movement;
    Rigidbody2D rb;
    public KeyCode dashKey = KeyCode.Space;

    void Start()
    {
        Initialize();
        rb = GetComponent<Rigidbody2D>();
    }

    void Initialize()
    {
        stats = FindFirstObjectByType<PlayerStats>();
        moveSpeed = stats.GetStatValue("MovementSpeed");
    }

    // in case I need to update speed dynamically
    public void SetSpeed(float value)
    {
        moveSpeed = value;
    }
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(dashKey))
        {
            Dash();
        }
    }
    void FixedUpdate()
    {
        HandleMovement();
    }
    void Dash()
    {
        Vector2 dashDirection = movement.normalized;
        if (dashDirection != Vector2.zero)
        {
            rb.MovePosition(rb.position + dashDirection * moveSpeed * 2 * dashDuration);
        }
    }

    void HandleMovement()
    {
        Vector2 targetVelocity = movement.normalized * moveSpeed;

        if (movement == Vector2.zero)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = targetVelocity;
        }
    }
}
