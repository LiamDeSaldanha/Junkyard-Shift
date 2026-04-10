using UnityEngine;

public class Attack_FlyingTV : MonoBehaviour
{
    public GameObject parent;
    public float speedRelease = 5f;
    public GameObject AmmoPrefab;
    public Transform FirePoint; // assign an empty GameObject at the barrel/spawn point
    public float fireRate = 1f; // seconds between shots

    private EnemyAI ai;
    private float nextFireTime = 0f;

    private void Start()
    {
        ai = GetComponentInChildren<EnemyAI>();
    }

    void Update()
    {
        if (ai != null && ai.GetState() == EnemyAI.State.Attack)
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void Fire()
    {
        // Spawn at FirePoint position and rotation, fallback to self if not assigned
        Vector3 spawnPos = FirePoint != null ? FirePoint.position : transform.position;
        Quaternion spawnRot = FirePoint != null ? FirePoint.rotation : transform.rotation;

        GameObject AmmoInstance = Instantiate(AmmoPrefab, spawnPos, spawnRot);

        Rigidbody rb = AmmoInstance.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(FirePoint.transform.forward * speedRelease, ForceMode.Impulse);
        else
            Debug.LogWarning("Ammo prefab has no Rigidbody!");
    }
}