using System.Resources;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using System;

public class UpgradeOxygen : MonoBehaviour
{

    public ResourceManager resourceManager;
    public bool inBase = false;

    public UIDocument uiUpgrade;
    private UIDocument uiTimer;
    private UIDocument uiAmmo;
    private UIDocument uiReload;
    private UIDocument uiManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var upgrade = uiUpgrade.rootVisualElement.Q<Button>("upgradeButton");
        upgrade.clicked += upgradeOxygen; 
        var exit = uiUpgrade.rootVisualElement.Q<Button>("exitButton");
        exit.clicked += exitOxygen;
        upgradeText();
        uiUpgrade.rootVisualElement.style.display = DisplayStyle.None;
        uiTimer = GameObject.Find("Player").gameObject.transform.Find("GameTimerUI").gameObject.GetComponent<UIDocument>();
        uiAmmo = GameObject.Find("Player").gameObject.transform.Find("RangedWeaponAmmoUI").gameObject.GetComponent<UIDocument>();
        uiReload = GameObject.Find("Player").gameObject.transform.Find("RangedWeaponReloadingUI").gameObject.GetComponent<UIDocument>();
        uiManager = GameObject.Find("Player").gameObject.transform.Find("UIManager").gameObject.GetComponent<UIDocument>();

    }

    StyleEnum<DisplayStyle> ammoShow;
    StyleEnum<DisplayStyle> reloadShow;
    StyleEnum<DisplayStyle> managerShow;

    private void Update()
    {
        int woodCount = resourceManager.GetResource("Wood");
        int MetalCount = resourceManager.GetResource("Metal");
        int PlasticCount = resourceManager.GetResource("Plastic");
        int ReactorCount = resourceManager.GetResource("Reactor");

        uiUpgrade.rootVisualElement.Q<Label>("currentResources").text = "Wood: " + woodCount +
            "\nMetal: " + MetalCount +
            "\nPlastic: " + PlasticCount +
            "\nReactor: " + ReactorCount;

        GameObject gmObj = GameObject.Find("Player");

        if (gmObj.transform.position.z < -45) {
            inBase = true;
        }
        else {
            inBase = false;
        }

        if (Keyboard.current.uKey.wasPressedThisFrame && inBase)
        {
            PlayerMouseView.ignoreMouse = true;
            uiTimer.rootVisualElement.style.display = DisplayStyle.None;
            ammoShow = uiAmmo.rootVisualElement.style.display;
            reloadShow = uiReload.rootVisualElement.style.display;
            managerShow = uiManager.rootVisualElement.style.display;
            uiAmmo.rootVisualElement.style.display = DisplayStyle.None;
            uiReload.rootVisualElement.style.display = DisplayStyle.None;
            uiManager.rootVisualElement.style.display = DisplayStyle.None;
            uiUpgrade.rootVisualElement.style.display = DisplayStyle.Flex;
            Time.timeScale = 0;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            uiTimer.rootVisualElement.style.display = DisplayStyle.Flex;
            uiAmmo.rootVisualElement.style.display = ammoShow;
            uiReload.rootVisualElement.style.display = reloadShow;
            uiManager.rootVisualElement.style.display = managerShow;
            ammoShow = DisplayStyle.None;
            reloadShow = DisplayStyle.None;
            managerShow = DisplayStyle.None;
            Time.timeScale = 1;
            uiUpgrade.rootVisualElement.style.display = DisplayStyle.None;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            PlayerMouseView.ignoreMouse = false;
        }
    }

    void upgradeOxygen() {

        Debug.Log("Find wood and metal");

        int woodCount = resourceManager.GetResource("Wood");
        int MetalCount = resourceManager.GetResource("Metal");

        Debug.Log("Found wood and metal");

        if ((OxygenManager.Instance.equippedFilterTier < 3) && (woodCount >= 2 && MetalCount >= 2))
        {
            OxygenManager.Instance.equippedFilterTier++;
            resourceManager.SpendResource("Wood", 2);
            resourceManager.SpendResource("Metal", 2);
        }
        upgradeText();
    }

    void exitOxygen()
    {
        uiTimer.rootVisualElement.style.display = DisplayStyle.Flex;
        uiAmmo.rootVisualElement.style.display = ammoShow;
        uiReload.rootVisualElement.style.display = reloadShow;
        uiManager.rootVisualElement.style.display = managerShow;
        ammoShow = DisplayStyle.None;
        reloadShow = DisplayStyle.None;
        managerShow = DisplayStyle.None;
        Time.timeScale = 1;
        uiUpgrade.rootVisualElement.style.display = DisplayStyle.None;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        PlayerMouseView.ignoreMouse = false;
    }

    void upgradeText() {
        Debug.Log("Upgrade text");

        switch (OxygenManager.Instance.equippedFilterTier) {
            case 0:
                uiUpgrade.rootVisualElement.Q<Label>("upgradeRequirements").text = "No Filter -> Level 1 Filter\nNeed to breath?\nRequires:\n\tx2 wood\n\tx2 metal";
                break;
            case 1:
                uiUpgrade.rootVisualElement.Q<Label>("upgradeRequirements").text = "Level 1 Filter -> Level 2 Filter\nLets you breath in deeper areas of the junkyard.\nRequires:\n\tx2 wood\n\tx2 metal";
                break;
            case 2:
                uiUpgrade.rootVisualElement.Q<Label>("upgradeRequirements").text = "Level 2 Filter -> MAX Level Filter\nPurer Air\nRequires:\n\tx2 wood\n\tx2 metal";
                break;
            case 3:
                uiUpgrade.rootVisualElement.Q<Label>("upgradeRequirements").text = "MAX Level Filter\nLungs Of Steel\nCan breath anywhere without difficulty.";
                break;
        }
    }

}
