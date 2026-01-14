using UnityEngine;


namespace Blocks
{
    //forzar la alineacion de los bloques en el grid. Nos sirve para poder construir cosas al estilo minecraft
    public class SnapToGrid : MonoBehaviour
    {
        [SerializeField] bool snapY = true;
        [SerializeField] float yOffset = 0f; // p.ej. 0 si el cubo mide 1 y se apoya en suelo Y=0
        
        
        public void SnapNow()
        {
            float cellSize = transform.localScale.x;
            Vector3 p = transform.position;
            float x = Mathf.Round(p.x / cellSize) * cellSize;
            float y = snapY ? Mathf.Round((p.y - yOffset) / cellSize) * cellSize + yOffset : p.y;
            float z = Mathf.Round(p.z / cellSize) * cellSize;
            transform.position = new Vector3(x, y, z);
        }
    

        // Si usas Meta Interaction SDK y tienes eventos:
        // En el GrabInteractable, vincula este método al evento OnSelectExited.
        void FixedUpdate()
        {
            SnapNow();
        }
    }
}