using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Follow_Player : MonoBehaviour
{

    private GameObject player;
    public static GameObject parent; // Should have a CharacterController

    public Animator animator;

    public NavMeshAgent agent;
    public float moveSpeed;
    public float turnSpeed = 180f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;

    private Vector3 velocity;
    private bool isGrounded;
    public bool isMerging;
    public Transform groundCheck;
    public LayerMask groundMask;
    public string nameOf;
    public string[] mergebles;
    private Boss_AI ai;
    public Transform enemy;


    void Start()
    {
        ai = GetComponent<Boss_AI>();
        player = GameObject.Find("Player");

    }

    void Update()
    {

        if (ai == null) return;
        clearAnimation();
        if (ai.GetState() == Boss_AI.State.Follow)
        {
            agent.isStopped = false;

            animator.SetBool("Walking", true);

            agent.SetDestination(player.transform.position);

        }
        if (ai.GetState() == Boss_AI.State.Twirl)
        {
            agent.isStopped = true;

            animator.SetBool("Twirl", true);

            


        }
        if (ai.GetState() == Boss_AI.State.Attack1)
        {
            agent.isStopped = true;
            animator.SetBool("Attacking1", true);


        }
        if (ai.GetState() == Boss_AI.State.Attack2)
        {
            agent.isStopped = true;
            animator.SetBool("Attacking2", true);


        }
        if (ai.GetState() == Boss_AI.State.Summon)
        {
            agent.isStopped = true;
            animator.SetBool("Summoning", true);


        }
        if (ai.GetState() == Boss_AI.State.Idle)
        {
            agent.isStopped = true;
            clearAnimation();


        }






    }

    private void clearAnimation()
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Attacking1", false);
        animator.SetBool("Attacking2", false);
            animator.SetBool("Twirl", false);
        animator.SetBool("Summoning", false);
    }


    private void OnTriggerStay(Collider other)
    {
       




    }
}
