using UnityEngine;

public class Block : MonoBehaviour
{
    public float gridSize = 1f;

    void Start()
    {
        SnapToGrid();
    }

    void SnapToGrid()
    {
        transform.position = new Vector3(
            Mathf.Round(transform.position.x / gridSize) * gridSize,
            Mathf.Round(transform.position.y / gridSize) * gridSize,
            Mathf.Round(transform.position.z / gridSize) * gridSize
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Block")) return;

        ContactPoint contact = collision.contacts[0];
        Vector3 offset = contact.normal * gridSize;

        transform.position = collision.transform.position + offset;
        SnapToGrid();
    }
}
