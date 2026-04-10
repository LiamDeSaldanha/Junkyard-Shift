using UnityEngine;
using UnityEngine.InputSystem;

public class playrJump : MonoBehaviour
{
    public float jumpForce = 12.0f;
    public bool isGrounded;
    public AudioSource jumpSound; 

    private Rigidbody rb;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int groundLayer = LayerMask.NameToLayer("Ground");
        int DefualtLayer = LayerMask.NameToLayer("Default");

        if (collision.gameObject.layer == groundLayer || collision.gameObject.layer == DefualtLayer)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        int groundLayer = LayerMask.NameToLayer("Ground");
        int DefualtLayer = LayerMask.NameToLayer("Default");

        if (collision.gameObject.layer == groundLayer || collision.gameObject.layer == DefualtLayer)
        {
            isGrounded = false;
        }
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (jumpSound != null)
                jumpSound.Play();
        }
        if (isGrounded == false && transform.position.y < 0) {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}