using UnityEngine;
using UnityEngine.InputSystem; // NEW Input System

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHorizontalMovement_InputSystem : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;

    [Header("Input (Input System)")]
    // Drag your InputActionReference (Move) from the Input Actions asset into this field
    public InputActionReference moveAction;   // expected Vector2 (x = left/right)
    public InputActionReference jumpAction;   // expected Button (performed = jump)

    [Header("Movement Settings")]
    public float maxSpeed = 8f;
    public float accel = 60f;
    public float decel = 80f;
    [Range(0f, 1f)] public float airControlFactor = 0.6f;

    [Header("Jump Settings")]
    public float jumpForce = 14f;
    public bool allowDoubleJump = false;

    // Ground detection is updated externally via GroundCheck (calls SetGrounded)
    bool isGrounded = false;
    int availableJumps = 0;

    // Internal
    float desiredDirection = 0f; // -1..1

    void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        // Enable actions if assigned
        if (moveAction != null) moveAction.action.Enable();
        if (jumpAction != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJumpPerformed;
        }
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (jumpAction != null)
        {
            jumpAction.action.performed -= OnJumpPerformed;
            jumpAction.action.Disable();
        }
    }

    void Update()
    {
        // Read move value (Vector2), but only X axis used
        if (moveAction != null && moveAction.action != null && moveAction.action.enabled)
        {
            Vector2 v = moveAction.action.ReadValue<Vector2>();
            desiredDirection = Mathf.Clamp(v.x, -1f, 1f);
        }
        else
        {
            desiredDirection = 0f;
        }
    }

    void FixedUpdate()
    {
        // Horizontal movement: smooth toward target speed
        float targetSpeed = desiredDirection * maxSpeed;
        float currentSpeed = rb.linearVelocity.x;

        float accelRate = Mathf.Abs(targetSpeed) > 0.001f ? accel : decel;
        float controlFactor = isGrounded ? 1f : airControlFactor;
        float maxDelta = accelRate * controlFactor * Time.fixedDeltaTime;

        float newSpeedX = Mathf.MoveTowards(currentSpeed, targetSpeed, maxDelta);
        rb.linearVelocity = new Vector2(newSpeedX, rb.linearVelocity.y);
    }

    void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            TryJump();
        }
    }

    void TryJump()
    {
        if (isGrounded)
        {
            DoJump();
            availableJumps = allowDoubleJump ? 1 : 0;
        }
        else if (availableJumps > 0)
        {
            DoJump();
            availableJumps--;
        }
    }

    void DoJump()
    {
        // reset vertical velocity before applying jump for consistent height
        Vector2 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // Called from GroundCheck script
    public void SetGrounded(bool grounded)
    {
        // when landing, reset double jump availability
        if (!isGrounded && grounded)
        {
            availableJumps = allowDoubleJump ? 1 : 0;
        }
        isGrounded = grounded;
    }
}
