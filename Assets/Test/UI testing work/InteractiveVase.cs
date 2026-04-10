using UnityEngine;

public class InteractiveVase : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {

        float randomHealth = Random.Range(0f, 100f);
        int oxygenLevel = Random.Range(0, 4);
        PlayerStatsHUD.Instance.SetHealth(randomHealth, 100f);
        PlayerStatsHUD.Instance.SetFuel(oxygenLevel, 3);
    }
}
