using UnityEngine;
//using UnityEngine.XR.ARFoundation;
//using UnityEngine.XR.ARSubsystems;
//using System.Collections.Generic;

public class ARTapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    //private ARRaycastManager arRaycastManager;
    //private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        //arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
            }
        }*/
    }
}
