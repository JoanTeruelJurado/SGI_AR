using UnityEngine;

public class RightHandMenuController : MonoBehaviour
{
    public GameObject rightHandMenu;      // El panel de UI
    public Transform rightController;      // RightControllerAnchor
    public float palmUpThreshold = 0.8f;  // Cómo de “plana” hacia arriba

    void Update()
    {
        if (rightController == null || rightHandMenu == null)
            return;

        // Calculamos “cuánto apunta la palma hacia arriba”
        float dot = Vector3.Dot(rightController.up, Vector3.up);

        // Si dot > threshold → palma arriba
        rightHandMenu.SetActive(dot > palmUpThreshold);
    }
}
