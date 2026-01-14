using UnityEngine;
using UnityEngine.InputSystem;


public class GunRayShooter : MonoBehaviour
{
    public Transform shootOrigin;
    public float range = 10f;
    public LayerMask hitLayers;

    // Referencia al script que controla si el arma está activa
    public GunSpawner gunSpawner; 
    
    void Update()
    {
        if (!gunSpawner.IsGunActive)
            return;

        // Gatillo del controller izquierdo
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Shoot();
        }
    }

    bool IsShootInput()
    {
        // AJUSTA ESTO a tu sistema de input:
        // - gesto de pinza
        // - botón XR
        // - Input System
        //return Input.GetMouseButtonDown(0); // placeholder
        return Keyboard.current != null && 
                Keyboard.current.spaceKey.wasPressedThisFrame;

    }

    void Shoot()
    {

        Ray ray = new Ray(shootOrigin.position, shootOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, hitLayers))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // Aquí puedes:
            // - destruir objetos
            // - aplicar daño
            // - cambiar material
            CubeTarget target = hit.collider.GetComponent<CubeTarget>();
            if (target != null)
            {
                target.DestroyCube();
            }
        }

        Debug.DrawRay(shootOrigin.position, shootOrigin.forward * range, Color.red, 0.1f);
    }
}
