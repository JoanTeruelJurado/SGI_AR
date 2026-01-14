using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    public GameObject prefabToPlace;
    public Camera arCamera;

    public void Place()
    {
        Vector3 position = arCamera.transform.position + arCamera.transform.forward * 1.2f;
        Instantiate(prefabToPlace, position, Quaternion.identity);
    }
}
