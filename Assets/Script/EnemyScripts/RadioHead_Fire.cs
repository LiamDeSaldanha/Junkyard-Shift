using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayer : MonoBehaviour
{

    private EnemyAI ai;
    public ParticleSystem ps1Prefab;
    public ParticleSystem ps2Prefab;
    public ParticleSystem ps3Prefab;
    public ParticleSystem ps4Prefab;

    private ParticleSystem ps1Intstance;
    private ParticleSystem ps2Intstance;
    private ParticleSystem ps3Intstance;
    private ParticleSystem ps4Intstance;
    private bool timerRunning = false;
    private GameObject player;
    private HealthSliderManager playerHealthManager;
    void Start()
    {
        ai = GetComponentInChildren<EnemyAI>();
         player = GameObject.Find("Player");
        playerHealthManager = player.GetComponentInChildren<HealthSliderManager>();

    }


    private void Update()
    {

        if (ai.GetState() == EnemyAI.State.Attack)
        {
            if (!PS_exists())
            {
                makePartilce();

            }
            else
            {
               // if (!timerRunning)
               // {
               //     timerRunning = true;
               //     playerHealthManager.resolveCollision(1);
               // }
                
            }
            

        }
        else
        {
            if (PS_exists()) {
            StopAll();
        }
        }
        
        



    }
    private IEnumerator timerWait()
    {
       
        yield return new WaitForSeconds(2f);
        timerRunning = false;
    }

    public bool PS_exists() { 
    
    

        return (ps1Intstance != null&& ps2Intstance != null && ps3Intstance != null && ps4Intstance != null);
    }

    public void PlayAll()
    {
        ps1Intstance.Play();
        ps2Intstance.Play();
        ps3Intstance.Play();
        ps4Intstance.Play();
}

    public void StopAll()
    {
        ps1Intstance.Stop();
        ps2Intstance.Stop();
        ps3Intstance.Stop();
        ps4Intstance.Stop();
    }

   

    public void makePartilce()
    {

        ps1Intstance = Instantiate(ps1Prefab, transform);
        ps2Intstance = Instantiate(ps2Prefab, transform);
        ps3Intstance = Instantiate(ps3Prefab, transform);
        ps4Intstance = Instantiate(ps4Prefab, transform);

    }
}
