using System.Collections;
using UnityEngine;

namespace UI
{
    //activa o desactiva en canvas principal
    public class ToggleBlocksUI : MonoBehaviour
    {
    
        public GameObject blocksUiCanvas;          // Canvas del menu principal
        
        public Transform playerCamera;       // Asigna la cámara del jugador 
        
        public float distanceFromPlayer = 1.5f; // Distancia a la que aparecerá el Canvas delante del jugador
        
        public float timeToShow = 3.0f; // Distancia a la que aparecerá el Canvas delante del jugador

        private Coroutine _hideCoroutine;


        public void Show()
        {
            Vector3 forward = playerCamera.forward;
            forward.y = 0; // evita que aparezca mirando hacia arriba o abajo
            forward.Normalize();

            blocksUiCanvas.transform.position = playerCamera.position + forward * distanceFromPlayer;
            blocksUiCanvas.transform.rotation = Quaternion.LookRotation(forward);
            
            blocksUiCanvas.SetActive(true);
            
            // si ya hay una corutina corriendo, cancelarla
            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);

            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }
        
        
        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(timeToShow);
            blocksUiCanvas.SetActive(false);
            _hideCoroutine = null;
        }
    }
}
