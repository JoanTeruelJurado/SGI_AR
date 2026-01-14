using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public GameObject voxelPrefab;
    public GameObject voxelPreviewPrefab;
    public Transform WorldRoot;
    public float currentVoxelSize { get; private set; } = 1f;  // ← this is what BlockPlacer uses for preview/snapping
    private Dictionary<Vector3Int, Chunk> chunks = new();

    // Preview management
    private GameObject currentPreview;
    private Vector3Int lastPreviewGridPos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue); // Invalid initial

    private GameObject currentPreviewVoxel;
    private Vector3Int currentPreviewGridPos = Vector3Int.zero;  // Invalid default

    public void AddVoxel(Vector3Int gridPos)
    {
        Vector3Int chunkCoord = new(
            Mathf.FloorToInt(gridPos.x / 16f),
            Mathf.FloorToInt(gridPos.y / 16f),
            Mathf.FloorToInt(gridPos.z / 16f)
        );
        Vector3Int localVoxel = new(
            gridPos.x - chunkCoord.x * 16,
            gridPos.y - chunkCoord.y * 16,
            gridPos.z - chunkCoord.z * 16
        );
        if (!chunks.TryGetValue(chunkCoord, out Chunk chunk))
        {
            GameObject chunkGO = Instantiate(chunkPrefab, transform);
            chunkGO.transform.localPosition = VoxelGrid.GridToLocal(chunkCoord * 16);
            chunk = chunkGO.GetComponent<Chunk>();
            chunk.chunkCoord = chunkCoord;
            chunks.Add(chunkCoord, chunk);
        }
        chunk.AddVoxel(localVoxel, voxelPrefab);
    }

    public void ShowVoxelPreview(Vector3Int gridPos) {
        // If position hasn't changed, do nothing
        if (gridPos == currentPreviewGridPos) return;

        // Remove old preview if exists
        if (currentPreviewVoxel != null)
        {
            Destroy(currentPreviewVoxel);
            currentPreviewVoxel = null;
        }

        // Only show preview if position is valid (you can add more checks later, e.g., not occupied)
        currentPreviewVoxel = Instantiate(voxelPreviewPrefab, WorldRoot);
        currentPreviewVoxel.transform.localPosition = VoxelGrid.GridToLocal(gridPos);

        currentPreviewGridPos = gridPos;
    }

    public void HideVoxelPreview() {
        if (currentPreviewVoxel != null)
        {
            Destroy(currentPreviewVoxel);
            currentPreviewVoxel = null;
        }
        currentPreviewGridPos = Vector3Int.zero;  // Reset
    }

    public void PlaceVoxelFromPreview() {
        if (currentPreviewGridPos == Vector3Int.zero) return;  // No valid preview

        AddVoxel(currentPreviewGridPos);

        // Clean up preview immediately after placement
        HideVoxelPreview();
    }

    public void PlaceVoxel(RaycastHit hit) {
        Vector3 localHitPoint =
            WorldRoot.InverseTransformPoint(hit.point);
        Vector3 localNormal =
            WorldRoot.InverseTransformDirection(hit.normal);
        Vector3Int hitVoxel =
            VoxelGrid.WorldToGrid(localHitPoint - localNormal * 0.01f);
        Vector3Int placeVoxel =
            hitVoxel + Vector3Int.RoundToInt(localNormal);
        AddVoxel(placeVoxel);
    }

    // Call this every frame from BlockPlacer (pass the calculated place position or RaycastHit)
    public void UpdatePreview(Vector3Int previewGridPos)
    {
        // Only update if position changed
        if (previewGridPos == lastPreviewGridPos) return;

        // Destroy old preview
        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }

        // Create new preview if valid
        if (voxelPreviewPrefab != null && previewGridPos != new Vector3Int(int.MinValue, int.MinValue, int.MinValue))
        {
            currentPreview = Instantiate(voxelPreviewPrefab, WorldRoot);
            currentPreview.transform.localPosition = VoxelGrid.GridToLocal(previewGridPos);
        }

        lastPreviewGridPos = previewGridPos;
    }

    // Call this when placing a real voxel (to clean up preview)
    public void ClearPreview()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
        lastPreviewGridPos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
    }

    private void OnDestroy()
    {
        ClearPreview();
    }
}
