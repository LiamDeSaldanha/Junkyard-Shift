using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody rb;
    public Transform cameraT;
    public float walkSpeed = 35f;
    public float runSpeedUp = 1.5f;
    public float gravityStrength = 10f;
    public float terminalVelocity = 54f;
    [Header("Footsteps")]
    public AudioSource audioSource;
    public AudioClip leftFootstep;
    public AudioClip rightFootstep;
    public float stepInterval = 0.5f;
    [Header("Sprint Sound")]
    public AudioSource sprintAudioSource; // Separate AudioSource for looping sprint sound
    [Header("Camera Bob")]
    public float bobFrequency = 6f;
    public float bobAmplitude = 0.05f;
    // Private
    private float movementX, movementY;
    private float stepTimer;
    private bool isLeftFoot = true;
    private float bobTimer;
    private Vector3 cameraStartLocalPos;
    private playrJump jumpScript;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraStartLocalPos = cameraT.localPosition;
        rb.useGravity = false;
        jumpScript = GetComponent<playrJump>();
        // Ensure the sprint source doesn't auto-play and is set to loop
        if (sprintAudioSource != null)
        {
            sprintAudioSource.loop = true;
            sprintAudioSource.playOnAwake = false;
        }
        Debug.Log("PlayerMovement initialized");
    }
    void FixedUpdate()
    {
        float speedMult = Keyboard.current.leftShiftKey.isPressed ? runSpeedUp : 1f;
        Vector3 moveDir = (transform.right * movementX + transform.forward * movementY).normalized * speedMult;
        Vector3 horizontalVelocity = moveDir * walkSpeed;
        // === GRAVITY THAT PRESERVES JUMPS ===
        float yVel = rb.linearVelocity.y;
        yVel -= gravityStrength * Time.fixedDeltaTime;
        yVel = Mathf.Max(yVel, -terminalVelocity);
        yVel = Mathf.Min(yVel, terminalVelocity);
        rb.linearVelocity = new Vector3(horizontalVelocity.x, yVel, horizontalVelocity.z);
        // === FOOTSTEPS ===
        HandleFootsteps(speedMult);
    }
    private void HandleFootsteps(float speedMult)
    {
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        bool isMoving = horizontalVel.magnitude > 0.1f;
        if (isMoving && jumpScript.isGrounded)
        {
            stepTimer += Time.fixedDeltaTime;
            float currentStepInterval = stepInterval / speedMult;
            if (stepTimer >= currentStepInterval)
            {
                AudioClip clip = isLeftFoot ? leftFootstep : rightFootstep;
                audioSource.PlayOneShot(clip);
                isLeftFoot = !isLeftFoot;
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
    // === SPRINT SOUND ===
    private void HandleSprintSound()
    {
        if (sprintAudioSource == null) return;
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        bool isSprinting = Keyboard.current.leftShiftKey.isPressed
                           && horizontalVel.magnitude > 0.1f
                           && jumpScript.isGrounded;
        if (isSprinting && !sprintAudioSource.isPlaying)
            sprintAudioSource.Play();
        else if (!isSprinting && sprintAudioSource.isPlaying)
            sprintAudioSource.Stop();
    }
    void LateUpdate()
    {
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        bool isMoving = horizontalVel.magnitude > 0.1f;
        // Handle sprint sound here so it runs every frame
        HandleSprintSound();
        if (isMoving && jumpScript.isGrounded)
        {
            bobTimer += Time.deltaTime * bobFrequency * (Keyboard.current.leftShiftKey.isPressed ? runSpeedUp : 1f);
            float bobY = Mathf.Sin(bobTimer) * bobAmplitude;
            float bobX = Mathf.Cos(bobTimer * 0.5f) * bobAmplitude * 0.5f;
            cameraT.localPosition = cameraStartLocalPos + new Vector3(bobX, bobY, 0f);
        }
        else
        {
            bobTimer = 0f;
            cameraT.localPosition = Vector3.Lerp(cameraT.localPosition, cameraStartLocalPos, Time.deltaTime * 8f);
        }
    }
    void OnMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        movementX = v.x;
        movementY = v.y;
    }
}