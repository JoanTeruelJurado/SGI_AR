using UnityEngine;

public static class VoxelGrid
{
    public const float VoxelSize = 1f;  // Assuming 1 unit = 1 block

    public static Vector3Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector3Int(
            Mathf.FloorToInt(worldPos.x / VoxelSize),
            Mathf.FloorToInt(worldPos.y / VoxelSize),
            Mathf.FloorToInt(worldPos.z / VoxelSize)
        );
    }

    public static Vector3 GridToLocal(Vector3Int gridPos)
    {
        return new Vector3(gridPos.x * VoxelSize, gridPos.y * VoxelSize, gridPos.z * VoxelSize);
    }

    // For chunk positioning (chunkCoord * 16 * size)
    public static Vector3 GridToWorld(Vector3Int gridPos)
    {
        return GridToLocal(gridPos);  // If size=1
    }
}