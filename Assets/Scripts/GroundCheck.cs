using UnityEngine;

public class GroundCheck_ByTag : MonoBehaviour
{
    [Header("Ground Check (by Tag)")]
    public Transform checkPoint; // pusty obiekt przy stopach
    public float checkRadius = 0.12f;
    public string groundTag = "Ground"; // tag przypisany platformom
    public PlayerHorizontalMovement_InputSystem playerMovement;

    void Reset()
    {
        // automatyczne przypisanie referencji
        if (checkPoint == null) checkPoint = transform;
        if (playerMovement == null) playerMovement = GetComponentInParent<PlayerHorizontalMovement_InputSystem>();
    }

    void FixedUpdate()
    {
        bool grounded = false;
        Collider2D[] hits = Physics2D.OverlapCircleAll(checkPoint.position, checkRadius);

        foreach (var hit in hits)
        {
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
        if (checkPoint == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(checkPoint.position, checkRadius);
    }
}
