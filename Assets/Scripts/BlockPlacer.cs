using UnityEngine;
using Oculus.VR;  // For OVRInput and OVRHand

public class BlockPlacer : MonoBehaviour
{
    public ChunkManager chunkManager;

    [SerializeField] private Transform rightHandPointerTransform;  // Drag the pointer pose transform here (see below)

    [SerializeField] private OVRHand rightOVRHand;  // Drag the OVRHand component from right hand

    public float pinchThreshold = 0.9f;     // How strong the pinch needs to be (0-1)
    public float rayReach = 10f;            // Max distance for pointing/placing

    private bool wasPinchingLastFrame = false;

    // Inside BlockPlacer.cs Update()
    private void Update()
    {
        if (rightOVRHand == null || rightHandPointerTransform == null) return;

        float pinchStrength = rightOVRHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = pinchStrength > pinchThreshold;

        // Update preview every frame
        UpdatePreview();

        // Place only when pinch starts
        if (isPinching && !wasPinchingLastFrame)
        {
            chunkManager.PlaceVoxelFromPreview();  // ← Places real block + hides preview
        }

        wasPinchingLastFrame = isPinching;
    }

    private void UpdatePreview()
    {
        Ray ray = new Ray(rightHandPointerTransform.position, rightHandPointerTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, rayReach))
        {
            Vector3 localHitPoint = chunkManager.WorldRoot.InverseTransformPoint(hit.point);
            Vector3 localNormal = chunkManager.WorldRoot.InverseTransformDirection(hit.normal);

            Vector3Int hitVoxel = VoxelGrid.WorldToGrid(localHitPoint - localNormal * 0.01f);
            Vector3Int placePos = hitVoxel + Vector3Int.RoundToInt(localNormal);

            chunkManager.ShowVoxelPreview(placePos);  // ← Shows/updates the ghost block
        }
        else
        {
            chunkManager.HideVoxelPreview();  // ← Hides when no valid surface
        }
    }

    private void TryPlaceBlock()
    {
        Ray ray = new Ray(rightHandPointerTransform.position, rightHandPointerTransform.forward);

        // Optional: Debug.DrawRay for visible laser in Editor/Play mode
        Debug.DrawRay(ray.origin, ray.direction * rayReach, Color.cyan, 0.1f);

        if (Physics.Raycast(ray, out RaycastHit hit, rayReach))
        {
            Vector3 localHitPoint = chunkManager.WorldRoot.InverseTransformPoint(hit.point);
            Vector3 localNormal = chunkManager.WorldRoot.InverseTransformDirection(hit.normal);

            Vector3Int hitVoxel = VoxelGrid.WorldToGrid(localHitPoint - localNormal * 0.01f);
            Vector3Int placePos = hitVoxel + Vector3Int.RoundToInt(localNormal);

            chunkManager.AddVoxel(placePos);
        }
    }
}
/*
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
}*/