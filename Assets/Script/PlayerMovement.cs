
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    private Vector3 movedirection;
    public InputActionReference move;

    private void Update()
    {
        movedirection = move.action.ReadValue<Vector3>();
    }
    void FixedUpdate()
    {
        transform.Translate( 0, movedirection.y * speed, movedirection.z * speed  );
        transform.Rotate(0, movedirection.x * speed, 0);
      //  rb.linearVelocity = new Vector3(movedirection.x*speed, movedirection.y * speed, movedirection.z * speed);
    }
}