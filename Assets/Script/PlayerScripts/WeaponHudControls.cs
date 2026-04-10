using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections;
using UnityEngine.UIElements;

public class WeaponHudControls : MonoBehaviour
{
    public UIDocument uiGunDetails;
    private float wepSwapCooldown = 0.2f;
    private Vector2 initialMousePos;
    public int wepNum = 0;

    private Coroutine timeScaleCoroutine;
    public UIDocument uiDoc;
    private InputAction rightMouseClick;

    private void Awake()
    {
        rightMouseClick = new InputAction(binding: "<Mouse>/RightButton");
        rightMouseClick.performed += ctx => onRightMouseClicked();
        rightMouseClick.canceled += ctx => OnRightClickReleased(ctx);
        initialMousePos = Mouse.current.position.ReadValue();
        rightMouseClick.Enable();
        uiDoc.rootVisualElement.style.display = DisplayStyle.None;
        uiGunDetails.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        rightMouseClick.Enable();
        uiDoc = GameObject.Find("Player").gameObject.transform.Find("UIManager").gameObject.GetComponent<UIDocument>();
    }

    private void OnDisable()
    {
        rightMouseClick.Disable();
        StartCoroutine(ReEnable());
    }

    IEnumerator ReEnable() {
        float elapsedTime = 0;
        while (elapsedTime < wepSwapCooldown) { 
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        OnEnable();
    }

    private void OnRightClickReleased(InputAction.CallbackContext context)
    {
        if (PlayerMouseView.ignoreMouse) {
            return;
        }

        // This function is called when the right click is released
        Debug.Log("Right click let go!");

        uiDoc.rootVisualElement.style.display = DisplayStyle.None;
        
        OnDisable();
        RestoreTime();

        Vector2 mousePos = Mouse.current.position.ReadValue() - initialMousePos;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
;
        float radians = Mathf.Atan2(mousePos.y, mousePos.x);
        float degrees = radians * Mathf.Rad2Deg;

        // Convert range from (-180, 180] to [0, 360)
        if (degrees < 0)
        {
            degrees += 360f;
        }

        wepNum = Mathf.CeilToInt((degrees - 45 / 2) / 45);

        wepNum = (wepNum + 6) % 8;

        Debug.Log("Weapon wheel:" + wepNum.ToString());

        toggleActiveWeapon("playerViewCamera/playerHand/Knife", false);
        toggleActiveWeapon("playerViewCamera/playerHand/Machete", false);
        toggleActiveWeapon("playerViewCamera/playerHand/Pistol", false);
        toggleActiveWeapon("playerViewCamera/playerHand/Ak47", false);
        toggleActiveWeapon("playerViewCamera/playerHand/PlasmaRifle", false);
        toggleActiveWeapon("playerViewCamera/playerHand/Bazooka", false);
        toggleActiveWeapon("playerViewCamera/playerHand/Spear", false);
        toggleActiveWeapon("playerViewCamera/playerHand/PlasmaBlade", false);

        Debug.Log("All weapons inactive");

        switch(wepNum) {
            case 0:
                toggleActiveWeapon("playerViewCamera/playerHand/Knife", true);
                uiGunDetails.rootVisualElement.style.display = DisplayStyle.None;
                break;
            case 1:
                if (!UnlockedWeaponManager.isMachetecrafted) { return; }
                toggleActiveWeapon("playerViewCamera/playerHand/Machete", true);
                uiGunDetails.rootVisualElement.style.display = DisplayStyle.None;
                break;
            case 2:
                if (!UnlockedWeaponManager.isPlasmaBladecrafted) { return; }
                toggleActiveWeapon("playerViewCamera/playerHand/PlasmaBlade", true);
                uiGunDetails.rootVisualElement.style.display = DisplayStyle.None;
                break;
            case 3:
                if (!UnlockedWeaponManager.isSpearcrafted) { return; }
                toggleActiveWeapon("playerViewCamera/playerHand/Spear", true);
                uiGunDetails.rootVisualElement.style.display = DisplayStyle.None;
                break;
            case 4:
                if (!UnlockedWeaponManager.isBazookacrafted) { return; }
                toggleActiveWeapon("playerViewCamera/playerHand/Bazooka", true);
                uiGunDetails.rootVisualElement.style.display = DisplayStyle.Flex;
                uiGunDetails.rootVisualElement.Q<Label>("GunName").text = "Bazooka";
                uiGunDetails.rootVisualElement.Q<Label>("AmmoCount").text = "? / ?";
                break;
            case 5:
                if (!UnlockedWeaponManager.isPlasmaRiflecrafted) { return; }
                toggleActiveWeapon("playerViewCamera/playerHand/PlasmaRifle", true);
                uiGunDetails.rootVisualElement.style.display = DisplayStyle.Flex;
                uiGunDetails.rootVisualElement.Q<Label>("GunName").text = "Plasma Rifle";
                uiGunDetails.rootVisualElement.Q<Label>("AmmoCount").text = "? / ?";
                break;
            case 6:
                if (!UnlockedWeaponManager.isAk47crafted) { return; }
                toggleActiveWeapon("playerViewCamera/playerHand/Ak47", true);
                uiGunDetails.rootVisualElement.style.display = DisplayStyle.Flex;
                uiGunDetails.rootVisualElement.Q<Label>("GunName").text = "Ak47";
                uiGunDetails.rootVisualElement.Q<Label>("AmmoCount").text = "? / ?";
                break;
            case 7:
                if (!UnlockedWeaponManager.isPistolcrafted) { return; }
                toggleActiveWeapon("playerViewCamera/playerHand/Pistol", true);
                uiGunDetails.rootVisualElement.style.display = DisplayStyle.Flex;
                uiGunDetails.rootVisualElement.Q<Label>("GunName").text = "Bazooka";
                uiGunDetails.rootVisualElement.Q<Label>("AmmoCount").text = "? / ?";
                break;
        }

        Debug.Log("NewWeaponActive");
    }

    private void toggleActiveWeapon(string path, bool state) {
        Transform trf = transform.Find(path);
        trf.gameObject.SetActive(state);
    }

    private void onRightMouseClicked() {

        if (PlayerMouseView.ignoreMouse)
        {
            return;
        }

        Debug.Log("Left mouse button clicked via the new Input System!");
        // Detect Mouse Click (Left Click)
        uiDoc.rootVisualElement.style.display = DisplayStyle.Flex;

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        initialMousePos = Mouse.current.position.ReadValue();

        if (timeScaleCoroutine != null) StopCoroutine(timeScaleCoroutine);
        timeScaleCoroutine = StartCoroutine(SlowTimeRoutine());
    }

    private IEnumerator SlowTimeRoutine()
    {
        Time.timeScale = 0.1f;
        // This waits for 1 second of REAL time, regardless of Time.timeScale
        yield return new WaitForSecondsRealtime(1);

        RestoreTime();
    }

    // Call this function to restore time immediately
    public void RestoreTime()
    {
        if (timeScaleCoroutine != null)
        {
            StopCoroutine(timeScaleCoroutine);
            timeScaleCoroutine = null;
        }
        Time.timeScale = 1.0f;
        // Adjust fixedDeltaTime for consistent physics during/after slow-mo
        Time.fixedDeltaTime = 0.016f;

    }
}
