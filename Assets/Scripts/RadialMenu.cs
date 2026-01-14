using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    public GameObject[] options;
    public float radius = 150f;

    private bool isOpen = false;

    void Start()
    {
        CloseMenu();
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;

        if (isOpen)
            OpenMenu();
        else
            CloseMenu();
    }

    void OpenMenu()
    {
        float angleStep = 360f / options.Length;

        for (int i = 0; i < options.Length; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );

            options[i].SetActive(true);
            options[i].GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    void CloseMenu()
    {
        foreach (GameObject option in options)
            option.SetActive(false);
    }
}
