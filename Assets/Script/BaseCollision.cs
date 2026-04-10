using UnityEngine;

public class BaseCollision : MonoBehaviour
{
    public HealthSliderManager healthSliderManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crawler"))
        {
            healthSliderManager.resolveCollision(1);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Chicky"))
        {
            healthSliderManager.resolveCollision(1);
            Destroy(other.gameObject);

        }
        if (other.CompareTag("Chomper"))
        {
            healthSliderManager.resolveCollision(1);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Spinster"))
        {
            healthSliderManager.resolveCollision(3);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("FlyingTV"))
        {
            healthSliderManager.resolveCollision(2);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("W-Spider"))
        {
            healthSliderManager.resolveCollision(2);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("RadioHead"))
        {
            healthSliderManager.resolveCollision(2);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Claze1"))
        {
            healthSliderManager.resolveCollision(3);
            Destroy(other.gameObject);
            if (other.CompareTag("Claze2"))
            {
                healthSliderManager.resolveCollision(5);
                Destroy(other.gameObject);
            }
        }
    }
}
