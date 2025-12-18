using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{

    [Header("Vida")]
    public int hitsToExplode = 3;

    [Header("Explosión")]
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float explosionUpwardForce = 2f;
    public GameObject explosionEffect;

    [Header("Sonido")]
    public AudioClip explosionSound;
    public float explosionVolume = 1f;

    [Header("Vibración VR")]
    public float vibrationDuration = 0.2f;
    public float vibrationFrequency = 1f;
    public float vibrationAmplitude = 1f;
    public OVRInput.Controller vibrationController = OVRInput.Controller.RTouch;

    private int _currentHits = 0;
    private bool _exploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_exploded) return;

        if (collision.gameObject.CompareTag("Projectile"))
        {
            _currentHits++;

            Debug.Log($"Barril impactado: {_currentHits}/{hitsToExplode}");

            if (_currentHits >= hitsToExplode)
            {
                Explode();
            }
        }
    }

    void Explode()
    {
        _exploded = true;

        // Sonido
        if (explosionSound) {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionVolume);
        }


        // Vibración
        OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, vibrationController);
        Invoke(nameof(StopVibration), vibrationDuration);


        // Efecto visual
        if (explosionEffect)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Detectar objetos cercanos
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius,
                    explosionUpwardForce,
                    ForceMode.Impulse
                );
            }
        }

        Destroy(gameObject);
    }

    void StopVibration()
    {
        OVRInput.SetControllerVibration(0, 0, vibrationController);
    }
}
