using UnityEngine;

public class TargetDiana : MonoBehaviour
{

    public int hitPoints = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Diana impactada");
            hitPoints--;

            if (hitPoints <= 0)
            {
                GetComponent<Renderer>().material.color = Color.yellow;
                Destroy(gameObject, 0.2f);
            }
        }
    }
}
