using UnityEngine;
using Oculus.VR;  // For OVRInput and OVRHand
public class Gun : MonoBehaviour
{
    public LayerMask layerMask;
    public OVRInput.RawButton shootingButton;
    public LineRenderer linePrefab;
    public Transform shootingPoint;
    public float maxLineDistance = 5f;
    public float lineShowtimer = 0.3f;
    public AudioSource source;
    public AudioClip shootingAudioClip;
    public GameObject rayImpactPrefab;
     [SerializeField] private OVRHand leftOVRHand;  // Drag the OVRHand component from right hand
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float pinchThreshold = 0.9f;
    
    private bool wasPinchingLastFrame = false;
    public float timeBetweenShots = 0.5f;
    private float lastShotTime = -Mathf.Infinity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float pinchStrength = leftOVRHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = pinchStrength > pinchThreshold;

        // Place only when pinch starts
        if (isPinching && !wasPinchingLastFrame && Time.time - lastShotTime >= timeBetweenShots)
        {
            Debug.Log("Shoot block");
            Shoot();
            lastShotTime = Time.time;
        }  
    }

    public void Shoot() {
        /*
        source.PlayOneShot(shootingAudioClip);

        Ray ray = new Ray(shootingPoint.position, shootingPoint.forward);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxLineDistance, layerMask);

        Vector3 endPoint = Vector3.zero; 

        if (hasHit) {
            endPoint = hit.point;
            Quaternion rayImpactRotation = Quaternion.LookRotation(-hit.normal);
            GameObject rayImpact = Instantiate(rayImpactPrefab, hit.point, rayImpactRotation);
            Destroy(rayImpact, 3);
        } else {
            endPoint = shootingPoint.position + shootingPoint.forward * maxLineDistance;
        }

        LineRenderer line = Instantiate(linePrefab);
        line.positionCount = 2;
        line.SetPosition(0, shootingPoint.position);

       

        line.SetPosition(1, endPoint);
        Destroy(line.gameObject, lineShowtimer);*/

        source.PlayOneShot(shootingAudioClip);

        GameObject bullet = Instantiate(
            bulletPrefab,
            shootingPoint.position,
            shootingPoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = shootingPoint.forward * bulletSpeed;
    }
}
