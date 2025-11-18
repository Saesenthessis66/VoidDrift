using UnityEngine;

public class GroundCheck_ByTag : MonoBehaviour
{
    [Header("Ground Check (by Tag)")]
    public string groundTag = "Ground"; // tag przypisany platformom
    public PlayerHorizontalMovement_InputSystem playerMovement;

    private CircleCollider2D groundCheckCollider;

    void Reset()
    {
        // automatyczne przypisanie referencji
        if (playerMovement == null) playerMovement = GetComponentInParent<PlayerHorizontalMovement_InputSystem>();

        // automatyczne znalezienie collidera
        if (groundCheckCollider == null) groundCheckCollider = GetComponent<CircleCollider2D>();
    }

    void Awake()
    {
        // upewnij się, że collider jest znaleziony
        if (groundCheckCollider == null)
            groundCheckCollider = GetComponent<CircleCollider2D>();

        if (groundCheckCollider == null)
            Debug.LogError("CircleCollider2D not found on this GameObject!", this);
    }

    void FixedUpdate()
    {
        if (groundCheckCollider == null) return;

        bool grounded = false;
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            (Vector2)transform.position + groundCheckCollider.offset,
            groundCheckCollider.radius
        );

        foreach (var hit in hits)
        {
            // ignoruj kolizje z samym sobą
            if (hit == groundCheckCollider) continue;

            if (hit.CompareTag(groundTag))
            {
                grounded = true;
                break;
            }
        }

        if (playerMovement != null)
            playerMovement.SetGrounded(grounded);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckCollider == null) return;

        Gizmos.color = Color.cyan;
        Vector2 center = (Vector2)transform.position + groundCheckCollider.offset;
        Gizmos.DrawWireSphere(center, groundCheckCollider.radius);
    }
}