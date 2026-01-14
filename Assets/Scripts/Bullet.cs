using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifetime = 5f;           // How long bullet lives if no hit
    [SerializeField] private LayerMask hitLayer;            // Layer(s) we want to hit (assign in inspector)
    
    [Header("Effects")]
    [SerializeField] private AudioClip hitSound;            // Sound when hitting target
    [SerializeField] [Range(0f, 2f)] private float volume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        // Optional: Add AudioSource if we want sound from bullet itself
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D sound
    }

    void Start()
    {
        // Destroy bullet after some time if it never hits anything
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object we hit is on our target layer
        if (((1 << collision.gameObject.layer) & hitLayer) != 0)
        {
            // Play hit sound (if we have one)
            if (hitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitSound, volume);
            }

            // Destroy both objects
            Destroy(collision.gameObject);      // The box/target
            Destroy(gameObject);                // The bullet itself

            // Optional: you could add particle effect here
            // Example: Instantiate(hitParticles, transform.position, Quaternion.identity);
        }
        else
        {
            // Optional: If you want bullet to disappear on any other collision
            // Destroy(gameObject);
        }
    }

    // Alternative version using trigger (if your bullet uses trigger collider)
    /*
    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hitLayer) != 0)
        {
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position, volume);
            }
            
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    */
}