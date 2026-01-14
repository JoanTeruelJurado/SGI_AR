using UnityEngine;

public class BlockPlacerVR : MonoBehaviour
{
    public GameObject blockPrefab;
    public float maxDistance = 10f;
    public LayerMask placementMask;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, placementMask))
            {
                Instantiate(blockPrefab, hit.point, Quaternion.identity);
            }
        }
    }
}
