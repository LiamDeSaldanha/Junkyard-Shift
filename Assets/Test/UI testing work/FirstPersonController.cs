using UnityEngine;
using UnityEngine.InputSystem; // Requires the Input System package

/// <summary>
/// Attach this to your Player GameObject.
/// The Player should have:
///   - A CharacterController component
///   - A Camera as a child object (assign in Inspector)
///
/// Requires: Unity Input System package (already active in your project).
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Look")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivity = 0.1f; // New Input System delta is in pixels, so this is smaller than before
    [SerializeField] private float verticalLookLimit = 80f;

    [Header("Interaction")]
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayers;

    private CharacterController _controller;
    private Vector3 _velocity;
    private float _verticalRotation = 0f;

    // Cached device references — checked for null so the script
    // won't throw if no keyboard/mouse is connected
    private Keyboard _keyboard;
    private Mouse _mouse;

    // Jump is buffered so a press is never silently dropped if
    // isGrounded happens to flicker false on that exact frame
    private bool _jumpBuffered = false;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        _keyboard = Keyboard.current;
        _mouse = Mouse.current;

        // Lock and hide the cursor for FPS-style look
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        // Re-cache devices whenever the component is enabled (e.g. after scene reload)
        _keyboard = Keyboard.current;
        _mouse = Mouse.current;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
        HandleInteraction();
    }

    // -------------------------------------------------
    // Mouse look — rotates the player body on Y (horizontal)
    // and the camera on X (vertical).
    // Mouse.current.delta gives raw pixel movement per frame.
    // -------------------------------------------------
    private void HandleLook()
    {
        if (_mouse == null) return;

        Vector2 mouseDelta = _mouse.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        // Rotate the player GameObject left/right
        transform.Rotate(Vector3.up * mouseX);

        // Tilt the camera up/down, clamped so you can't flip over
        _verticalRotation -= mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -verticalLookLimit, verticalLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }

    // -------------------------------------------------
    // WASD movement + jump + gravity
    // -------------------------------------------------
    private void HandleMovement()
    {
        if (_keyboard == null) return;

        // isGrounded can flicker on flat surfaces — a small negative Y
        // velocity keeps the controller pressed against the ground
        if (_controller.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;

        // Build a movement vector from WASD / arrow keys
        Vector2 moveInput = Vector2.zero;
        if (_keyboard.wKey.isPressed || _keyboard.upArrowKey.isPressed) moveInput.y += 1f;
        if (_keyboard.sKey.isPressed || _keyboard.downArrowKey.isPressed) moveInput.y -= 1f;
        if (_keyboard.dKey.isPressed || _keyboard.rightArrowKey.isPressed) moveInput.x += 1f;
        if (_keyboard.aKey.isPressed || _keyboard.leftArrowKey.isPressed) moveInput.x -= 1f;

        // Move relative to where the player is facing
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        _controller.Move(move * moveSpeed * Time.deltaTime);

        // Buffer the jump request the moment space is pressed,
        // then consume it as soon as we confirm we're grounded.
        // This prevents presses from being dropped when isGrounded
        // flickers false on the same frame the key goes down.
        if (_keyboard.spaceKey.wasPressedThisFrame)
            _jumpBuffered = true;

        if (_jumpBuffered && _controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _jumpBuffered = false;
        }

        // Apply gravity
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    // -------------------------------------------------
    // Interaction — fires a ray from the camera centre.
    // Any object hit that implements IInteractable will
    // have its Interact() method called on 'E' press.
    // -------------------------------------------------
    private void HandleInteraction()
    {
        if (_keyboard == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayers))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null && _keyboard.eKey.wasPressedThisFrame)
            {
                interactable.Interact();
            }
        }
    }

    // -------------------------------------------------
    // Handy utility — call this to unlock the cursor
    // (e.g. when opening a menu)
    // -------------------------------------------------
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}