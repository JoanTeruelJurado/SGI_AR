using UnityEngine;
using Oculus.Platform;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using Meta.XR.BuildingBlocks;

public class WorldAnchorManager : MonoBehaviour
{
    public GameObject worldRootPrefab;
    public OVRInput.RawButton PlacingButton;
    private GameObject worldRootInstance;
    private OVRSpatialAnchor anchor;

    public void CreateAnchor(Vector3 position, Quaternion rotation)
    {
        worldRootInstance = Instantiate(worldRootPrefab, position, rotation);

        anchor = worldRootInstance.AddComponent<OVRSpatialAnchor>();
        anchor.Save();
    }

        // Update is called once per frame
    void Update()
    {
        /*
        if (OVRInput.GetDown(PlacingButton)) {
            CreateAnchor();
        }
        */   
    }
}
