using UnityEngine;

public class GroundCheck_ByTag : MonoBehaviour
{
    [Header("Ground Check (by Tag)")]
    public string groundTag = "Ground"; // tag przypisany platformom
    public PlayerHorizontalMovement_InputSystem playerMovement;

    private CircleCollider2D groundCheckCollider;

    void Reset()
    {
        if (playerMovement == null)
            playerMovement = GetComponentInParent<PlayerHorizontalMovement_InputSystem>();

        if (groundCheckCollider == null)
            groundCheckCollider = GetComponent<CircleCollider2D>();
    }

    void Awake()
    {
        if (groundCheckCollider == null)
            groundCheckCollider = GetComponent<CircleCollider2D>();

        if (groundCheckCollider == null)
            Debug.LogError("CircleCollider2D not found on this GameObject!", this);
    }

    void FixedUpdate()
    {
        if (groundCheckCollider == null) return;

        // --- GLOBALNE centrum koła (offset lokalny → globalna pozycja)
        Vector2 center = groundCheckCollider.transform.TransformPoint(groundCheckCollider.offset);

        // --- GLOBALNY promień (uwzględnia skalowanie)
        float radius = groundCheckCollider.radius *
                       Mathf.Max(
                           groundCheckCollider.transform.lossyScale.x,
                           groundCheckCollider.transform.lossyScale.y
                       );

        bool grounded = false;

        // --- DETEKCJA ZIEMI ---
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);

        foreach (var hit in hits)
        {
            if (hit == groundCheckCollider) continue;  // pomiń siebie

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
        if (groundCheckCollider == null)
            groundCheckCollider = GetComponent<CircleCollider2D>();

        if (groundCheckCollider == null) return;

        Gizmos.color = Color.cyan;

        Vector2 center = groundCheckCollider.transform.TransformPoint(groundCheckCollider.offset);

        float radius = groundCheckCollider.radius *
                       Mathf.Max(
                           groundCheckCollider.transform.lossyScale.x,
                           groundCheckCollider.transform.lossyScale.y
                       );

        Gizmos.DrawWireSphere(center, radius);
    }
}
