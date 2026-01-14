using UnityEngine;

namespace Blocks
{
    //Si esta activado bloquea la rotacion. Nos sirve en los bloques por si queremos que queden alienados
    //en el grid y asi poder construir cosas al estilo minecraft
    [RequireComponent(typeof(Rigidbody))]
    public class FreezeRotationAlways : MonoBehaviour
    {
        private Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();

            // Asegura que la rotación física esté bloqueada
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        void FixedUpdate()
        {
            // Evita cualquier rotación numérica o por el SDK
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            // Mantiene la orientación original exacta
            transform.rotation =  Quaternion.Euler(0f, 0f, 0f);
        }

        private void Update()
        {
            // Evita cualquier rotación numérica o por el SDK
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            // Mantiene la orientación original exacta
            transform.rotation =  Quaternion.Euler(0f, 0f, 0f);
        }
    }
}