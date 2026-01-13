using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public float spawnDistance = 0.5f;

    public void SpawnCube(Color color)
    {
        Camera cam = Camera.main;
        Vector3 spawnPos = cam.transform.position + cam.transform.forward * spawnDistance;

        GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);

        Renderer r = cube.GetComponent<Renderer>();
        if (r != null)
        {
            r.material = new Material(r.material);
            r.material.color = color;
        }
    }

    // Métodos para botones (Unity UI)
    public void SpawnRedCube()
    {
        SpawnCube(Color.red);
    }

    public void SpawnGreenCube()
    {
        SpawnCube(Color.green);
    }

    public void SpawnBlueCube()
    {
        SpawnCube(Color.blue);
    }
}
