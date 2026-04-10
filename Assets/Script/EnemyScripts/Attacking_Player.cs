using UnityEngine;

public class Attacking_Player : MonoBehaviour
{

    public BoxCollider[] colliders;
    private EnemyAI ai;
    void Start()
    {
        ai = GetComponentInChildren<EnemyAI>();

    }

    void Update()
    {
        if(ai != null)
        {
            if(ai.GetState() == EnemyAI.State.Attack)
            {

                for(int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = true;
                }
            }else 
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = false;
                }
            }

        }



    }
}
