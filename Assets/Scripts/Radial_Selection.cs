using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class Radial_Selection : MonoBehaviour
{
    [Range(2,10)] public int NumberOfRadialPart;
    public GameObject radialPartPrefab;
    public Transform radialPartCanvas;
    public OVRInput.Button spawnButton;
    public float AngleBetweenPart;
    private List<GameObject> spawnedParts = new List<GameObject>();
    public Transform handTransform;

    private int currentSelectedRadialPart = -1;

    public UnityEvent<int> OnPartSelected;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(spawnButton)) SpawnRadialPart();
        if (OVRInput.Get(spawnButton)) GetSelectedRadialPart();
        if (OVRInput.GetUp(spawnButton)) HideAndTriggerSelected();
    }

    public void HideAndTriggerSelected() {
        OnPartSelected.Invoke(currentSelectedRadialPart);
        radialPartCanvas.gameObject.SetActive(false);
    }

    public void GetSelectedRadialPart() {
        Vector3 centerToHand = handTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(radialPartCanvas.up, centerToHandProjected, -radialPartCanvas.forward);

        if (angle < 0) angle +=360;

        currentSelectedRadialPart = (int) angle * NumberOfRadialPart / 360;

        for(int i = 0; i < spawnedParts.Count; i++) {
            if(i == currentSelectedRadialPart) {
                spawnedParts[i].GetComponent<Image>().color = Color.yellow;
                spawnedParts[i].transform.localScale = 1.1f * Vector3.one;
            } else {
                spawnedParts[i].GetComponent<Image>().color = Color.white;
                spawnedParts[i].transform.localScale = 1f * Vector3.one;
            }
        }
    }

    public void SpawnRadialPart() {
        radialPartCanvas.gameObject.SetActive(true);
        radialPartCanvas.position = handTransform.position;
        radialPartCanvas.rotation = handTransform.rotation;

        foreach (var item in spawnedParts) {
            Destroy(item);
        }
        spawnedParts.Clear();

        for (int i = 0; i<NumberOfRadialPart; i++) {
            float angle = - i * 360/NumberOfRadialPart - AngleBetweenPart/2;
            Vector3 radiaPartEulerAngle = new Vector3(0,0,angle);

            GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartCanvas);

            spawnedRadialPart.transform.position = radialPartCanvas.position;
            spawnedRadialPart.transform.localEulerAngles = radiaPartEulerAngle;

            spawnedRadialPart.GetComponent<Image>().fillAmount = (1/ (float) NumberOfRadialPart) - (AngleBetweenPart/360);
            spawnedParts.Add(spawnedRadialPart);
        }
    }
}
