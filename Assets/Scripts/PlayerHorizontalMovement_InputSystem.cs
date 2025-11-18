using UnityEngine;
using UnityEngine.InputSystem; // NEW Input System

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHorizontalMovement_InputSystem : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;  // DODANE

    [Header("Input (Input System)")]
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

    // State info for animations
    public float Speed { get; private set; }        // DODANE
    public bool IsGrounded => isGrounded;           // DODANE
    public bool faceRight { get; private set; } = true; // DODANE

    // Ground detection updated via GroundCheck
    bool isGrounded = false;
    int availableJumps = 0;

    // Input direction
    float desiredDirection = 0f; // -1..1

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>(); // AUTO przypisanie

        if (rb != null)
            rb.freezeRotation = true;
    }

    void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (rb != null)
            rb.freezeRotation = true;
    }

    void OnEnable()
    {
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
        // ----- INPUT -----
        if (moveAction != null && moveAction.action != null && moveAction.action.enabled)
        {
            Vector2 v = moveAction.action.ReadValue<Vector2>();
            desiredDirection = Mathf.Clamp(v.x, -1f, 1f);
        }
        else
        {
            desiredDirection = 0f;
        }

        // ----- ANIMATION PARAMETERS -----

        // 1. Speed
        Speed = Mathf.Abs(rb.linearVelocity.x);
        if (animator != null)
            animator.SetFloat("Speed", Speed);

        // 2. IsGrounded
        if (animator != null)
            animator.SetBool("IsGrounded", isGrounded);

        // 3. Facing direction
        if (desiredDirection > 0.05f)
            faceRight = true;
        else if (desiredDirection < -0.05f)
            faceRight = false;

        if (animator != null)
            animator.SetBool("FaceRight", faceRight);
    }

    void FixedUpdate()
    {
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
            TryJump();
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
        // reset vertical velocity before jump
        Vector2 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    // Called from GroundCheck script
    public void SetGrounded(bool grounded)
    {
        if (!isGrounded && grounded)
        {
            availableJumps = allowDoubleJump ? 1 : 0;
        }
        isGrounded = grounded;
    }
}
