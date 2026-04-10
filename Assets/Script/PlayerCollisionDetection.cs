using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    public HealthSliderManager healthSliderManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }
  

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RadioHead"))
        {

            healthSliderManager.resolveCollision(1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Crawler_Attack"))
        {

            healthSliderManager.resolveCollision(2);
        }
        if (other.CompareTag("Chicky_Attack"))
        {

            healthSliderManager.resolveCollision(1);
        }
        if (other.CompareTag("Chomper_Attack"))
        {

            healthSliderManager.resolveCollision(3);
        }
        if (other.CompareTag("Claze_part_2"))
        {

            healthSliderManager.resolveCollision(10);
        }
        if (other.CompareTag("Claze_part_1_attack"))
        {

            healthSliderManager.resolveCollision(5);
        }
        if (other.CompareTag("FlyingTV_Attack"))
        {

            healthSliderManager.resolveCollision(3);
        }
        if (other.CompareTag("Spinster_Attack"))
        {

            healthSliderManager.resolveCollision(2);
        }
        if (other.CompareTag("WSpider_Attack"))
        {

            healthSliderManager.resolveCollision(4);
        }
        if (other.CompareTag("BossAttack"))
        {

            healthSliderManager.resolveCollision(10);
        }
        
        if (other.CompareTag("Item"))
        {
            itemPickup item = other.GetComponent<itemPickup>();
            if (item != null)
            {
                ResourceManager.Instance.AddResource(item.LootItem, 1);
                Destroy(other.gameObject);
            }

        }

    }
}
