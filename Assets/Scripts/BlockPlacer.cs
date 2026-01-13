using UnityEngine;
using Oculus.VR;  // For OVRInput and OVRHand

public class BlockPlacer : MonoBehaviour
{
    public ChunkManager chunkManager;

    [Header("Laser Visuals")]
    [SerializeField] private LineRenderer handLaser;               // ← Drag your LineRenderer component here
    [SerializeField] private Transform rightHandPointerTransform;  // ← Usually index tip or palm forward transform
    [SerializeField] private OVRHand rightOVRHand;                 // Drag the OVRHand (right) here

    [Header("Settings")]
    public float pinchThreshold = 0.9f;
    public float rayReach = 10f;
    public float laserWidth = 0.008f;         // quite thin for immersion
    public Color laserColorValid = new Color(0.0f, 0.7f, 1.0f, 0.9f);    // cyan-ish when hitting
    public Color laserColorNoHit = new Color(0.4f, 0.4f, 0.4f, 0.6f);    // dim gray when no hit

    [Header("Debug & Fixes")]
    [SerializeField] private LayerMask raycastLayerMask = ~0;  // ← Set to ignore hands/player layer in Inspector (e.g., exclude "Ignore Raycast" or custom "Hands" layer)
    public float originOffsetDistance = 0.05f;  // ← Adjust this (0.05-0.1m) to start ray just outside hand collider

    private bool wasPinchingLastFrame = false;

    private void Awake()
    {
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

    private void Update()
    {
        if (rightOVRHand == null || rightHandPointerTransform == null || handLaser == null)
            return;

        float pinchStrength = rightOVRHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = pinchStrength > pinchThreshold;

        // Always update the preview + laser
        UpdatePreviewAndLaser();

        // Place only on pinch start (rising edge)
        if (isPinching && !wasPinchingLastFrame)
        {
            chunkManager.PlaceVoxelFromPreview();
        }

        wasPinchingLastFrame = isPinching;
    }

    private void UpdatePreviewAndLaser()
    {
        // Direction: Use -forward if it was pointing backwards before (as per your last message)
        Vector3 rayDirection = -rightHandPointerTransform.forward;  // ← Flip if needed; try forward if this is wrong now

        // Offset origin slightly forward to avoid self-hit on hand collider
        Vector3 rayOrigin = rightHandPointerTransform.position + rayDirection * originOffsetDistance;

        Ray ray = new Ray(rayOrigin, rayDirection);

        // Debug ray (visible in Scene view / Gizmos)
        Debug.DrawRay(ray.origin, ray.direction * rayReach, Color.cyan, 0.08f);

        // Raycast with layer mask to ignore hands/player
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, rayReach, raycastLayerMask);

        Vector3 endPosition;

        if (hitSomething)
        {
            endPosition = hit.point;

            // NEW: Log the name of the hit object to console (check Unity Console for "Hit: ObjectName")
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
        }
        else
        {
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

    // Optional: cleanup on disable/destroy
    private void OnDisable()
    {
        if (handLaser != null)
            handLaser.enabled = false;
    }
}