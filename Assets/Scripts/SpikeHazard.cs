using UnityEngine;
using Unity.Cinemachine;

public class SpikeHazard : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;

    [Header("Knockback")]
    public float knockbackForce = 10f;
    public Vector2 knockbackDirection = new Vector2(1, 1.2f);

    [Header("Camera Shake")]
    public CinemachineImpulseSource impulseSource;

    private void Reset()
    {
        impulseSource = GetComponentInParent<CinemachineImpulseSource>();
    }

    private void Awake()
    {
        impulseSource = GetComponentInParent<CinemachineImpulseSource>();

        if (impulseSource == null)
            Debug.LogError("Brak CinemachineImpulseSource u parenta!");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // W którą stronę odrzucić
            float dir = (other.transform.position.x > transform.position.x) ? 1f : -1f;

            Vector2 final = new Vector2(
                knockbackDirection.x * dir,
                knockbackDirection.y
            );

            // Reset prędkości (ważne dla spójnego odrzutu)
            rb.linearVelocity = Vector2.zero;

            // Knockback
            rb.AddForce(final.normalized * knockbackForce, ForceMode2D.Impulse);
        }

        // Shake kamery
        if (impulseSource != null)
            impulseSource.GenerateImpulse();
    }
}
