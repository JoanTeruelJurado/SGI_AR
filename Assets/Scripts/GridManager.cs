using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    public static GridManager Instance;
    public float cubeSize = 1f;
    private HashSet<Vector3Int> occupied = new();

    void Awake() { Instance = this; }

    public Vector3 SnapPosition(Vector3 worldPos) {
        return new Vector3(
            Mathf.Round(worldPos.x / cubeSize) * cubeSize,
            Mathf.Round(worldPos.y / cubeSize) * cubeSize,
            Mathf.Round(worldPos.z / cubeSize) * cubeSize
        );
    }

    public Vector3Int GetGridPos(Vector3 worldPos) {
        Vector3 snapped = SnapPosition(worldPos);
        return new Vector3Int(Mathf.RoundToInt(snapped.x / cubeSize), Mathf.RoundToInt(snapped.y / cubeSize), Mathf.RoundToInt(snapped.z / cubeSize));
    }

    public bool IsOccupied(Vector3Int pos) => occupied.Contains(pos);
    public void Occupy(Vector3Int pos) => occupied.Add(pos);
    public void Free(Vector3Int pos) => occupied.Remove(pos);
}