using UnityEngine;
using UnityEngine.UI; 

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public float spawnDistance = 0.5f;

    [Header("UI")]
    public Slider sizeSlider;  // Slider que controla el tamaño de los cubos

    private float cubeSize = 0.1f; // Tamaño por defecto

    void Start()
    {
        if (sizeSlider != null)
        {
            cubeSize = sizeSlider.value;
            sizeSlider.onValueChanged.AddListener(UpdateCubeSize);
        }
    }

    void UpdateCubeSize(float newSize)
    {
        cubeSize = newSize;
    }

    public void SpawnCube(Color color)
    {
        Camera cam = Camera.main;
        Vector3 spawnPos = cam.transform.position + cam.transform.forward * spawnDistance;

        GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);

        // Ajustar tamany
        cube.transform.localScale = Vector3.one * cubeSize;

        // Configurar color
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
