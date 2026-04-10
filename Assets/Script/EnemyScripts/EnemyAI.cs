using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float followRange = 50f;

    private Transform player;
    private Transform enemy;
    private float distPlayer;
    private float distEnemy;

    public enum State { Idle,Wander, Follow,Merge , Attack }
    private State currentState = State.Idle;
        public string[] mergebles;

  
    public State GetState() {

        return currentState;

    }


    private void SetState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (currentState)
        {
            case State.Follow:
              //  Debug.Log("Following player");
                // Start moving toward player
                break;
            case State.Attack:
               // Debug.Log("Attacking player");
                // Trigger attack logic
                break;
            case State.Idle:
               // Debug.Log("Idle");
                // Stop moving
                break;
            case State.Merge:
               // Debug.Log("Merge");
                // 
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            player = other.transform;
        if (mergebles.Contains(other.tag))
            enemy = other.transform;
    }




    private void OnTriggerStay(Collider other)
    {


        if (!(other.CompareTag("Player") || mergebles.Contains(other.tag))) return;
        
        if (player == null)
        {



            SetState(State.Merge);
        }
        else
        {
            if (mergebles.Contains(other.tag))//remove this if enemy ai breaks
                enemy = other.transform;




            distPlayer = Vector3.Distance(transform.position, player.position);

            if (enemy != null)
            {
                distEnemy = Vector3.Distance(transform.position, enemy.position);
            }
            else
            {
                distEnemy = 1000;
            }

            if (distPlayer < distEnemy)
            {
                if (distPlayer <= attackRange)
                    SetState(State.Attack);
                else
                    SetState(State.Follow);
            }
            else { 
            SetState(State.Idle);

            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            currentState = State.Idle;
        }
    }

    
}