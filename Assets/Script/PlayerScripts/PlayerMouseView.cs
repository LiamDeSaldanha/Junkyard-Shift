using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseView : MonoBehaviour
{

    public Transform cameraT;

    public float maxNeckAngle = 75;

    private bool mouseShown = false;
    private float mouseDelay = 1;
    private float lastMouseVisibiltyChanged = 0;

    public float horizontalSensitivty = 180;
    public float verticalSensitivty = 120;

    public static bool ignoreMouse = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // locks mouse so player can move mouse without cursor hitting screen edge, and hides cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lastMouseVisibiltyChanged = Time.time;
    }

    // Update is called once per frame
    void Update() {
        // reads the current mouse value and extact the change in x and y pos for this frame.
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        if (ignoreMouse) {
            mouseDelta *= 0;
        }

        

        if (ignoreMouse)
        {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mouseShown = true;
            
        }
        else
        {
            
        }


        // converts mouse sensetivity to rotate the player
        float rotY = mouseDelta.x / Screen.width * horizontalSensitivty;
        float rotX = mouseDelta.y / Screen.height * verticalSensitivty;

        float val = cameraT.rotation.eulerAngles.x;
        // clamp rotation about x-axis so player can only look up or down up to a set angle
        // looking up clamp
        if (val - rotX < 360 - maxNeckAngle && val - rotX > 180)
        {
            rotX = val + maxNeckAngle;
        }
        // looking down clamp
        if (val - rotX > maxNeckAngle && val - rotX < 180)
        {
            rotX = val - maxNeckAngle;
        }

        // rotate the player to face the correct way and rotate the camera to face up or down
        cameraT.Rotate(-rotX, 0, 0);
        transform.Rotate(0, rotY, 0);

        // enables/disables the cursor and toggle between the 2
        if (Keyboard.current.leftAltKey.isPressed)
        {
            if (Time.time - lastMouseVisibiltyChanged > mouseDelay)
            {
                lastMouseVisibiltyChanged = Time.time;
                if (!mouseShown)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    mouseShown = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    mouseShown = false;
                }
            }
        }
    }
}