using UnityEngine;

public class GunController : MonoBehaviour
{
    public float fireDistance = 10f;
    public LayerMask hitLayers;

    void Update()
    {
        // Disparo simple: gesto o botón (por ahora click izquierdo)
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, fireDistance, hitLayers))
        {
            CubeTarget target = hit.collider.GetComponent<CubeTarget>();
            if (target != null)
            {
                target.DestroyCube();
            }
        }
    }
}
