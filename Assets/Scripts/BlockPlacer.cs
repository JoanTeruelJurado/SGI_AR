using UnityEngine;
using Oculus.VR;  // For OVRInput and OVRHand

public class BlockPlacer : MonoBehaviour
{
    public ChunkManager chunkManager;
    
    [Header("Laser Visuals")]
    [SerializeField] private LineRenderer handLaser;  // ← Drag your LineRenderer here
    [SerializeField] private Transform rightHandPointerTransform;  // Drag the pointer pose transform here (see below)
    [SerializeField] private OVRHand rightOVRHand;  // Drag the OVRHand component from right hand

    [Header("Settings")]
    public float pinchThreshold = 0.9f;     // How strong the pinch needs to be (0-1)
    public float rayReach = 10f;            // Max distance for pointing/placing
    public float laserWidth = 0.008f;         // quite thin for immersion
    public Color laserColorValid = new Color(0.0f, 0.7f, 1.0f, 0.9f);    // cyan-ish when hitting
    public Color laserColorNoHit = new Color(0.4f, 0.4f, 0.4f, 0.6f);    // dim gray when no hit
    public AudioSource PlaceBlockSound;
    private bool wasPinchingLastFrame = false;

    [Header("Debug & Fixes")]
    [SerializeField] private LayerMask raycastLayerMask = ~0;  // ← Set to ignore hands/player layer in Inspector (e.g., exclude "Ignore Raycast" or custom "Hands" layer)
    public float originOffsetDistance = 0.05f;  // ← Adjust this (0.05-0.1m) to start ray just outside hand collider

    private void Awake() {
        if (handLaser != null)
        {
            // Basic LineRenderer setup (do once)
            handLaser.positionCount = 2;
            handLaser.startWidth = laserWidth;
            handLaser.endWidth = laserWidth * 0.6f;   // optional slight taper
            handLaser.material = new Material(Shader.Find("Sprites/Default")); // simple unlit
            handLaser.useWorldSpace = true;
        }
    }

    // Inside BlockPlacer.cs Update()
    private void Update()
    {
        if (rightOVRHand == null || rightHandPointerTransform == null || handLaser == null) return;

        float pinchStrength = rightOVRHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = pinchStrength > pinchThreshold;

        // Update preview every frame
        UpdatePreviewAndLaser();

        // Place only when pinch starts
        if (isPinching && !wasPinchingLastFrame)
        {
            chunkManager.PlaceVoxelFromPreview();  // ← Places real block + hides preview
            PlaceBlockSound.Play();
        }

        wasPinchingLastFrame = isPinching;
    }

private void UpdatePreviewAndLaser()
    {
        Ray ray = new Ray(rightHandPointerTransform.position, rightHandPointerTransform.forward);

        // Raycast with layer mask to ignore hands/player
        //bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, rayReach, raycastLayerMask);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, rayReach, raycastLayerMask);

        Vector3 endPosition;

        if (hitSomething) {
            endPosition = hit.point;

            // Optional: little offset so laser stops just before surface
            // endPosition = hit.point - ray.direction * 0.02f;
            Debug.Log($"Hit: {hit.collider.gameObject.name} (Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)})");

            // Update preview (your existing logic)
            Vector3 localHitPoint = chunkManager.WorldRoot.InverseTransformPoint(hit.point);
            Vector3 localNormal = chunkManager.WorldRoot.InverseTransformDirection(hit.normal);
            Vector3Int hitVoxel = VoxelGrid.WorldToGrid(localHitPoint - localNormal * 0.01f);
            Vector3Int placePos = hitVoxel + Vector3Int.RoundToInt(localNormal);

            chunkManager.ShowVoxelPreview(placePos);

            // Visual feedback → strong/clear color when valid placement surface
            handLaser.startColor = laserColorValid;
            handLaser.endColor = laserColorValid;
        } else {
            endPosition = ray.origin + ray.direction * rayReach;

            chunkManager.HideVoxelPreview();

            // Dimmer color when pointing at nothing
            handLaser.startColor = laserColorNoHit;
            handLaser.endColor = laserColorNoHit;
        }

        // Update LineRenderer (always visible)
        handLaser.SetPosition(0, ray.origin);
        handLaser.SetPosition(1, endPosition);
    }

    private void OnDisable()
    {
        if (handLaser != null)
            handLaser.enabled = false;
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