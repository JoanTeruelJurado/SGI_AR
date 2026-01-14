using UnityEngine;

namespace Blocks
{
    //cuando tienes un bloque enfocado y pulsas la B se borra. 
    //este escript mira si tienes el bloque en el camino del rayo cuando pulsas la B, y si es asi lo elimina
    public class DeleteBlock : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string blockTag = "Block";
        [SerializeField] private OVRInput.RawButton deleteButton = OVRInput.RawButton.B;
        [SerializeField] private float maxDistance = 10f;

        [Header("Referencias")]
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private LineRenderer rightRay;
        [SerializeField] private LineRenderer leftRay;

        void Start()
        {
            //Parte de debug para ver a donde apuntaba el rayo, ya que la referencia right hand transform y left hand
            //transform no eran correctas. Dibujamos los rayos para debugar. Se queda aqui por si es util mas adelante, 
            //aunque ya no lo usemos en el juego
            if (rightRay)
            {
                rightRay.positionCount = 2;
                rightRay.startWidth = 0.01f;
                rightRay.endWidth = 0.005f;
                rightRay.material = new Material(Shader.Find("Unlit/Color"));
                rightRay.material.color = Color.red;
            }

            if (leftRay)
            {
                leftRay.positionCount = 2;
                leftRay.startWidth = 0.01f;
                leftRay.endWidth = 0.005f;
                leftRay.material = new Material(Shader.Find("Unlit/Color"));
                leftRay.material.color = Color.blue;
            }
        }

        void Update()
        {
            UpdateRayVisuals();

            //si el boton B esta apretado, mirar si se puede borrar algun bloque
            if (OVRInput.GetDown(deleteButton))
            {
                TryDeleteBlock();
            }
        }

        //Si estan puesto que se vean los rayos actualizarlos
        private void UpdateRayVisuals()
        {
            if (rightHandTransform && rightRay)
            {
                rightRay.SetPosition(0, rightHandTransform.position);
                rightRay.SetPosition(1, rightHandTransform.position + rightHandTransform.forward * maxDistance);
            }

            if (leftHandTransform && leftRay)
            {
                leftRay.SetPosition(0, leftHandTransform.position);
                leftRay.SetPosition(1, leftHandTransform.position + leftHandTransform.forward * maxDistance);
            }
        }

        private void TryDeleteBlock()
        {
            //para cada rayo ver si estan enfocando a un objeto que se pueda eliminar (tenga el tag blockPrefab) 
            //y si es asi eliminarlo. Si el primer rayo elimina un objeto volvemos, ya que puede ser 
            //que tengas un objeto detras de otro y el primer rayo te eliminaria uno y el segundo rayo otro y no 
            //queremos eso
            if (rightHandTransform)
            {
                Ray ray = new Ray(rightHandTransform.position, rightHandTransform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
                {
                    GameObject target = hit.collider.gameObject;
                    if (target.CompareTag(blockTag))
                    {
                        Destroy(target);
                        return;
                    }
                }
            }

            if (leftHandTransform)
            {
                Ray ray = new Ray(leftHandTransform.position, leftHandTransform.forward);
                if (Physics.Raycast(ray, out RaycastHit leftHit, maxDistance))
                {
                    GameObject target = leftHit.collider.gameObject;
                    if (target.CompareTag(blockTag))
                    {
                        Destroy(target);
                    }
                }
            }
        }
    }
}
