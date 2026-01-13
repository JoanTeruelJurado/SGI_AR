using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    public GameObject gunPrefab;
    public Transform gunAnchor;

    GameObject currentGun;

    public void ToggleGun()
    {
        if (currentGun == null)
        {
            currentGun = Instantiate(gunPrefab, gunAnchor);
            currentGun.transform.localPosition = Vector3.zero;
            currentGun.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Destroy(currentGun);
            currentGun = null;
        }
    }
}
