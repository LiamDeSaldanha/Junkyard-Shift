using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform player;
    public float rotateSpeed = 5f;
    public bool lockYAxis = true; // keeps upright, only rotates horizontally

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }
    private void OnTriggerStay(Collider other)
    {
        if (player == null || !other.CompareTag("Player")) return;

        Vector3 direction = player.position - transform.position;

        if (lockYAxis)
            direction.y = 0; // ignore height difference

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.parent.transform.rotation = Quaternion.Slerp(transform.parent.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
    void Update()
    {
      
    }
}