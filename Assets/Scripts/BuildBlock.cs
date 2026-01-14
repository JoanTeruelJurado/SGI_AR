using UnityEngine;

public class BuildBlock : MonoBehaviour
{
    [Header("Configuración de snap")]
    public float snapDistance = 0.15f;      // Distancia máxima para que el snap se active
    public Transform[] snapPoints;          // Puntos de anclaje del cubo

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Inicialmente el cubo está controlado manualmente, no por física
        rb.isKinematic = true;
    }

    /// <summary>
    /// Intenta encajar el cubo con otro cubo cercano
    /// </summary>
    /// <param name="other">Cubo con el que intentar el snap</param>
    /// <returns>True si se encajó</returns>
    public bool TrySnap(BuildBlock other)
    {
        foreach (var myPoint in snapPoints)
        {
            foreach (var otherPoint in other.snapPoints)
            {
                if (Vector3.Distance(myPoint.position, otherPoint.position) < snapDistance)
                {
                    // Ajustamos la posición para que coincida el punto de snap
                    Vector3 offset = myPoint.position - transform.position;
                    transform.position = otherPoint.position - offset;

                    // Opcional: hacer que el cubo quede fijo al encajar
                    rb.isKinematic = true;

                    // Opcional: convertir en hijo para formar estructuras
                    transform.SetParent(other.transform);

                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Permite “liberar” el cubo para moverlo o destruirlo
    /// </summary>
    public void Detach()
    {
        rb.isKinematic = false;
        transform.SetParent(null);
    }

    /// <summary>
    /// Función que se puede llamar al disparar para destruir el cubo
    /// </summary>
    public void Break()
    {
        rb.isKinematic = false; // Activar física
        // Agregar fuerza opcional si quieres que salte al ser disparado
        // rb.AddExplosionForce(50f, transform.position, 2f);
        Destroy(gameObject, 3f); // Destruye después de 3 segundos
    }
}
