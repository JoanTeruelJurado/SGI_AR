using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public GameObject voxelPrefab;
    public Transform WorldRoot;

    private Dictionary<Vector3Int, Chunk> chunks = new();

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

}
