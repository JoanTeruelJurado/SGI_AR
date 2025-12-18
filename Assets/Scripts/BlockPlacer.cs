using UnityEngine;
using UnityEngine.XR;
using Unity.XR;
using UnityEditor.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Oculus.Interaction;

public class BlockPlacer : MonoBehaviour
{
    public ChunkManager chunkManager;  // Assign your ChunkManager in Inspector
    public GameObject voxelPrefab;     // Same as in ChunkManager

    [SerializeField] private XRBaseController rightController;  // Assign right controller
    [SerializeField] public OVRInput.RawButton PlacingButton;    // Grip or trigger

    private void Awake()
    {
        // Example: Use trigger press to place (configure in Action-Based Controller)
        if (OVRInput.GetDown(PlacingButton)) {
            TryPlaceBlock();
        }    
    }

    private void TryPlaceBlock()
    {
        if (rightController.TryGetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>(out var rayInteractor))
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                // Transform hit relative to WorldRoot (same as your code)
                Vector3 localHitPoint = chunkManager.WorldRoot.InverseTransformPoint(hit.point);
                Vector3 localNormal = chunkManager.WorldRoot.InverseTransformDirection(hit.normal);

                Vector3Int hitVoxel = VoxelGrid.WorldToGrid(localHitPoint - localNormal * 0.01f);
                Vector3Int placePos = hitVoxel + Vector3Int.RoundToInt(localNormal);

                chunkManager.AddVoxel(placePos);
            }
        }
    }
}