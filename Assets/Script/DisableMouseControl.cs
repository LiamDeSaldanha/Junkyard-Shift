using UnityEngine;
using UnityEngine.UIElements;

public class DisableMouseControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        PlayerMouseView.ignoreMouse = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
