using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
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
    private EnemyAI ai;
    public Transform enemy;
    public Vector3 Base;

    public AudioSource audioSource;
    public AudioClip footstepClip;

    private float stepTimer = 0f;
    public float stepInterval = 0.5f;

    void Start()
    {
         ai = GetComponent<EnemyAI>();
        player = GameObject.Find("Player");
        Base = new Vector3(27.6907f, -0.06f, -58.422f);
    }

    void Update()


    {
        if(ai == null) return;

        if(ai.GetState() == EnemyAI.State.Follow)
        {
            agent.isStopped = false;
            animator.SetBool("Running", true);
            animator.SetBool("Attacking", false);

            agent.SetDestination(player.transform.position);

        }

        if (ai.GetState() == EnemyAI.State.Idle)
        {
            agent.isStopped = false;
            animator.SetBool("Attacking", false);
            animator.SetBool("Running", true);
            string currentScene = SceneManager.GetActiveScene().name;
            if (!(currentScene == "Boss Chamber"))
            {
                agent.SetDestination(Base);

            }
        }

        if (ai.GetState() == EnemyAI.State.Attack)
        {
            agent.isStopped = true;

            animator.SetBool("Attacking", true);
            animator.SetBool("Running", false);

        }

        if (ai.GetState() == EnemyAI.State.Merge)
        {
            animator.SetBool("Running", true);
            animator.SetBool("Attacking", false);
        }

        bool isMoving = agent.velocity.magnitude > 0.1f;

        if (isMoving && !agent.isStopped)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                //audioSource.PlayOneShot(footstepClip);
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }





    }
    
    

    private void OnTriggerStay(Collider other)
    {
        if (ai.GetState() == EnemyAI.State.Merge)
        {
            if (mergebles.Contains(other.tag))
            {

                agent.isStopped = false;
                agent.SetDestination(other.transform.position);

            }
        }


     
        
    }



}
