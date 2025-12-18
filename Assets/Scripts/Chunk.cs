using UnityEngine;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
    public Vector3Int chunkCoord;
    public Dictionary<Vector3Int, GameObject> voxels = new();

    public void AddVoxel(Vector3Int localVoxelCoord, GameObject voxelPrefab)
    {
        if (voxels.ContainsKey(localVoxelCoord))
            return;

        GameObject voxel = Instantiate(voxelPrefab, transform);
        voxel.transform.localPosition = VoxelGrid.GridToLocal(localVoxelCoord);

        voxels[localVoxelCoord] = voxel;
    }
}
