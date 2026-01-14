
using UnityEngine;
using UnityEngine.UI;
using Oculus.VR;  // For OVRInput and OVRHand


    //generador de bloques/objetos
    public class BlockSpawner : MonoBehaviour
    {
        [Header("Block Prefabs (Grass, Rock, Sand, Wood...)")]
        [SerializeField] public GameObject[] blockPrefabs;

        [Header("Hand reference")]
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private OVRHand rightOVRHand;  // Drag the OVRHand component from right hand

        [SerializeField] private Vector3 localOffset = new Vector3(0f, 0f, 0.1f);

        [Header("Options")]
        [Tooltip("Si es true, solo se permitirán los inputs de spawn/cambio de bloque cuando el mando esté activo (evita triggers desde hand/pinch).")]
        [SerializeField] private bool requireControllerForSpawn = true;
        public float pinchThreshold = 0.9f;    // How strong the pinch needs to be (0-1)

        // Controlador requerido (por defecto, mando izquierdo)
        [SerializeField] private OVRInput.Controller requiredController = OVRInput.Controller.LTouch;
        
  
        
        [Header("Block Settings")]
        [SerializeField] public float blockScale = 0.2f; // Escala uniforme para los bloques (X=Y=Z)
        
        [Header("UI Menu References")]
        [SerializeField] public Toggle applyGravityToggle;
        private bool wasPinchingLastFrame = false;
        public AudioSource PlaceBlockSound;

        
        private int currentIndex = 0;

    
        void Update()
        {
            float pinchStrength = rightOVRHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
            bool isPinching = pinchStrength > pinchThreshold;

            // Place only when pinch starts
            if (isPinching && !wasPinchingLastFrame)
            {
                Debug.Log("Spawning block");
                SpawnBlock();
                PlaceBlockSound.Play();
            }

            wasPinchingLastFrame = isPinching;

            OVRInput.Controller active = OVRInput.GetActiveController();

            bool requiredControllerActive = (active & requiredController) != 0;

            // Si requerimos un mando pero no está activo, no procesamos botones (evita que pinch emule botón)
            if (requireControllerForSpawn && !requiredControllerActive) return;
            


        }

        private void SpawnBlock()
        {
            if (blockPrefabs.Length == 0 || (object)rightHandTransform == null)
                return;

            Vector3 spawnPos = rightHandTransform.TransformPoint(localOffset);
            Quaternion spawnRot = rightHandTransform.rotation;

            GameObject block = Instantiate(blockPrefabs[currentIndex], spawnPos, spawnRot);
    
  
            // Aplica la escala uniforme definida en el inspector
            block.transform.localScale = Vector3.one * blockScale;
        }
    }
