using System.Linq;
using UnityEngine;

public class Boss_AI : MonoBehaviour
{
    [SerializeField] private float attack2Range = 40f;
    [SerializeField] private float twirlRange = 15f;
    [SerializeField] private float followRange = 100; 
    [SerializeField] private float idleRange = 150;
    [SerializeField] private float SummonRange = 60;

    private Transform player;
    private Transform enemy;
    private float distPlayer;
    private float distEnemy;


    // Inside Boss_AI.cs

    private bool isBossMusicPlaying = false; // Add this variable at the top

    public enum State { Idle, Attack1, Follow, Attack2, Summon ,Twirl}
    private State currentState = State.Idle;
    public string[] mergebles;

    
    public State GetState()
    {

        return currentState;

    }


    private void SetState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (currentState)
        {
            case State.Follow:
                Debug.Log("Following player");
                // Start moving toward player
                break;
            case State.Attack1:
                Debug.Log("Attacking1 player");
                // Trigger attack logic
                break;
            case State.Attack2:
                Debug.Log("Attacking2 player");
                // Trigger attack logic
                break;
            case State.Idle:
                Debug.Log("Idle");
                // Stop moving
                break;
            case State.Twirl:
                Debug.Log("Twirl");
                // Stop moving
                break;
            case State.Summon:
                Debug.Log("Summoning");
                // 
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            player = other.transform;

            // Trigger the Singleton!
            if (!isBossMusicPlaying)
            {
                isBossMusicPlaying = true;
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {


        if (!(other.CompareTag("Player") )) return;

        if (other.CompareTag("Player"))
            player = other.transform;

        distPlayer = Vector3.Distance(transform.position, player.position);

        if (twirlRange >= distPlayer)
        {
            SetState(State.Twirl);

        }
       else if (attack2Range >= distPlayer)
        {
            SetState(State.Attack2);

        }

        
        else if (followRange >= distPlayer)
        {
            SetState(State.Follow);



        }
        else if (SummonRange >= distPlayer)

        {
            SetState(State.Summon);



        }
        else if (idleRange >= distPlayer)
            {
            SetState(State.Idle);



        }




    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            currentState = State.Idle;

            // Switch back to chill vibes
            if (isBossMusicPlaying)
            {
                MusicManager.Instance.SwitchToBackgroundMusic();
                isBossMusicPlaying = false;
            }
        }
    }


}