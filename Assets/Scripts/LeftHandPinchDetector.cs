using UnityEngine;

public class LeftHandPinchDetector : MonoBehaviour
{
    public OVRHand leftHand;

    public bool IsPinching =>
        leftHand != null &&
        leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
}
