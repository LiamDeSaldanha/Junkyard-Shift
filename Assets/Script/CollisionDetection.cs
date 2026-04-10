using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    public HealthSliderManager healthSliderManager;


    private void Start()
    {
        
    }



    private void OnTriggerEnter(Collider other) {
                                                                                                                                                            
        if (other.CompareTag("Knife")) {
            healthSliderManager.resolveCollision(1);
        }
        if (other.CompareTag("Machete")) {
            healthSliderManager.resolveCollision(4);
        }
        if (other.CompareTag("PlasmaBlade")) {
            healthSliderManager.resolveCollision(8);
        }
        if (other.CompareTag("Spear")) {
            healthSliderManager.resolveCollision(6);
        }
        if (other.CompareTag("Pistol")) {
            healthSliderManager.resolveCollision(1);
        }
        if (other.CompareTag("Ak47")) {
            healthSliderManager.resolveCollision(2);
        }
        if (other.CompareTag("PlasmaRifle")) {
            healthSliderManager.resolveCollision(3);
        }
        if (other.CompareTag("bazooka")) {
            healthSliderManager.resolveCollision(5);
        }
    }
}
