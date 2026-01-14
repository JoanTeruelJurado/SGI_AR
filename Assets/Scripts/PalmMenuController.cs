using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class PalmMenuController : MonoBehaviour
{
    public GameObject palmMenu;
    public float showThreshold = 0.6f;

    XRHandSubsystem handSubsystem;

    void Start()
    {
        //var xrManager = XRGeneralSettings.Instance.Manager;
        //handSubsystem = xrManager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
        if (XRGeneralSettings.Instance == null)
        {
            Debug.Log("XR no inicializado (Editor sin visor)");
            return;
        }

        var xrManager = XRGeneralSettings.Instance.Manager;

        if (xrManager == null || xrManager.activeLoader == null)
        {
            Debug.Log("XR Loader no activo (Editor sin Quest)");
            return;
        }

        handSubsystem = xrManager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();

        if (handSubsystem == null)
        {
            Debug.Log("Hand Tracking no disponible");
        }
    }

    void Update()
    {
        if (handSubsystem == null)
            return;

        XRHand rightHand = handSubsystem.rightHand;

        if (!rightHand.isTracked)
        {
            palmMenu.SetActive(false);
            return;
        }

        var palmJoint = rightHand.GetJoint(XRHandJointID.Palm);

        if (!palmJoint.TryGetPose(out Pose palmPose))
        {
            palmMenu.SetActive(false);
            return;
        }

        Vector3 palmUp = palmPose.rotation * Vector3.up;
        Vector3 toCamera = Camera.main.transform.position - palmPose.position;

        float dot = Vector3.Dot(palmUp.normalized, toCamera.normalized);

        bool show = dot > showThreshold;

        palmMenu.SetActive(show);
    }
}
