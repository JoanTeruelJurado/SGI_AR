 using UnityEngine;
using Meta.XR.MRUtilityKit;

public class ObjectSpawner : MonoBehaviour
{
    public float spawnTimer = 1f;
    public GameObject prefabToSpawn;
    private float timer;

    public float minEdgeDistance = 0.3f;
    public MRUKAnchor.SceneLabels spawnLabels;
    public float normalOffset = -1f;

    public int SpawnTry = 1000;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MRUK.Instance && !MRUK.Instance.IsInitialized) return;
        timer += Time.deltaTime;
        if (timer > spawnTimer) {
            SpawnObject();
            timer -= spawnTimer;
        }
        
    }

    public void SpawnObject() {

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        
        int currentTry = 0;

        while(currentTry < SpawnTry) {
            bool thereIsPosition = room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);

            if (thereIsPosition) {
                Vector3 randomPosition = pos+ norm * normalOffset;
                randomPosition.y = 0;

                Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
            } else {
                currentTry++;
            }
        }
    }
}
