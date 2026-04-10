using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Unity.VisualScripting; // Required namespace

public class craftingScript : MonoBehaviour
{
    public UIDocument uiCrafting;
    public Texture2D KnifeImage;
    public Texture2D PistolImage;
    public Texture2D MacheteImage;
    public Texture2D Ak47Image;
    public Texture2D PlasmaBladeImage;
    public Texture2D PlasmaRifleImage;
    public Texture2D SpearImage;
    public Texture2D BazookaImage;
    public ResourceManager resourceManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private GameObject plyrObj;

    private int currentTier = -1;
    private bool inBase = false;

    private UIDocument uiTimer;
    private UIDocument uiAmmo;
    private UIDocument uiReload;
    private UIDocument uiManager;

    void Start()
    {
        var tier0Button = uiCrafting.rootVisualElement.Q<Button>("Tier0Button");
        tier0Button.clicked += showTier0;
        var tier1Button = uiCrafting.rootVisualElement.Q<Button>("Tier1Button");
        tier1Button.clicked += showTier1;
        var tier2Button = uiCrafting.rootVisualElement.Q<Button>("Tier2Button");
        tier2Button.clicked += showTier2;
        var tier3Button = uiCrafting.rootVisualElement.Q<Button>("Tier3Button");
        tier3Button.clicked += showTier3; 
        var exitButton = uiCrafting.rootVisualElement.Q<Button>("exitButton");
        exitButton.clicked += exitCrafting;

        uiCrafting.rootVisualElement.style.display = DisplayStyle.None;

        plyrObj = GameObject.Find("Player");

        showTier0();

        var meleeCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("MeleeCraftButton");
        meleeCraftBuutton.clicked += craftMelee;

        var rangedCraftButton = uiCrafting.rootVisualElement.Q<Button>("RangedCraftButton");
        rangedCraftButton.clicked += craftRanged;

        uiTimer = GameObject.Find("Player").gameObject.transform.Find("GameTimerUI").gameObject.GetComponent<UIDocument>();
        uiAmmo = GameObject.Find("Player").gameObject.transform.Find("RangedWeaponAmmoUI").gameObject.GetComponent<UIDocument>();
        uiReload = GameObject.Find("Player").gameObject.transform.Find("RangedWeaponReloadingUI").gameObject.GetComponent<UIDocument>();
        uiManager = GameObject.Find("Player").gameObject.transform.Find("UIManager").gameObject.GetComponent<UIDocument>();
    }
    void exitCrafting()
    {
        uiTimer.rootVisualElement.style.display = DisplayStyle.Flex;
        uiAmmo.rootVisualElement.style.display = ammoShow;
        uiReload.rootVisualElement.style.display = reloadShow;
        uiManager.rootVisualElement.style.display = managerShow;
        ammoShow = DisplayStyle.None;
        reloadShow = DisplayStyle.None;
        managerShow = DisplayStyle.None;
        Time.timeScale = 1;
        uiCrafting.rootVisualElement.style.display = DisplayStyle.None;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        PlayerMouseView.ignoreMouse = false;

    }

    StyleEnum<DisplayStyle> ammoShow;
    StyleEnum<DisplayStyle> reloadShow;
    StyleEnum<DisplayStyle> managerShow;

    void Update()
    {

        if (plyrObj.transform.position.z < -45)
        {
            inBase = true;
        }
        else
        {
            inBase = false;
        }
        if (Keyboard.current.cKey.wasPressedThisFrame && inBase)
        {
            PlayerMouseView.ignoreMouse = true;
            Time.timeScale = 0;
            uiTimer.rootVisualElement.style.display = DisplayStyle.None;
            ammoShow = uiAmmo.rootVisualElement.style.display;
            reloadShow = uiReload.rootVisualElement.style.display;
            managerShow = uiManager.rootVisualElement.style.display;
            uiAmmo.rootVisualElement.style.display = DisplayStyle.None;
            uiReload.rootVisualElement.style.display = DisplayStyle.None;
            uiManager.rootVisualElement.style.display = DisplayStyle.None;
            uiCrafting.rootVisualElement.style.display = DisplayStyle.Flex;
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
            uiCrafting.rootVisualElement.style.display = DisplayStyle.None;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            PlayerMouseView.ignoreMouse = false;
        }

        int woodCount = resourceManager.GetResource("Wood");
        int MetalCount = resourceManager.GetResource("Metal");
        int PlasticCount = resourceManager.GetResource("Plastic");
        int ReactorCount = resourceManager.GetResource("Reactor");

        uiCrafting.rootVisualElement.Q<Label>("AvailableResources").text =
            "Wood: " + woodCount +
            "\nMetal: " + MetalCount +
            "\nPlastic: " + PlasticCount +
            "\nReactor: " + ReactorCount;
    }

    void craftMelee()
    {
        int woodCount = resourceManager.GetResource("Wood");
        int MetalCount = resourceManager.GetResource("Metal");
        int PlasticCount = resourceManager.GetResource("Plastic");
        int ReactorCount = resourceManager.GetResource("Reactor");

        if (currentTier == 0)
        {
            if (!UnlockedWeaponManager.isKnifecrafted && (MetalCount >= 1))
            {
                UnlockedWeaponManager.isKnifecrafted = true;
                resourceManager.SpendResource("Metal", 1);
                showTier0();
            }
        }
        if (currentTier == 1)
        {
            if (!UnlockedWeaponManager.isMachetecrafted && (MetalCount >= 3 && woodCount >= 1))
            {
                UnlockedWeaponManager.isMachetecrafted = true;
                resourceManager.SpendResource("Wood", 1);
                resourceManager.SpendResource("Metal", 3);
                showTier1();
            }
        }
        if (currentTier == 2)
        {
            if (!UnlockedWeaponManager.isSpearcrafted && (MetalCount >= 2 && woodCount >= 2 && PlasticCount >= 2))
            {
                UnlockedWeaponManager.isSpearcrafted = true;
                showTier2();
                resourceManager.SpendResource("Wood", 2);
                resourceManager.SpendResource("Metal", 2);
                resourceManager.SpendResource("Plastic", 2);
            }
        }
        if (currentTier == 3)
        {
            if (!UnlockedWeaponManager.isPlasmaBladecrafted && (MetalCount >= 3 && woodCount >= 1 && PlasticCount >= 2 && ReactorCount >= 1))
            {
                UnlockedWeaponManager.isPlasmaBladecrafted = true;
                showTier3();
                resourceManager.SpendResource("Wood", 1);
                resourceManager.SpendResource("Metal", 3);
                resourceManager.SpendResource("Plastic", 2);
                resourceManager.SpendResource("Reactor", 1);
            }
        }
    }

    void craftRanged()
    {
        int woodCount = resourceManager.GetResource("Wood");
        int MetalCount = resourceManager.GetResource("Metal");
        int PlasticCount = resourceManager.GetResource("Plastic");
        int ReactorCount = resourceManager.GetResource("Reactor");

        if (currentTier == 0)
        {
            if (!UnlockedWeaponManager.isPistolcrafted && (woodCount >= 1 && MetalCount >= 1))
            {
                UnlockedWeaponManager.isPistolcrafted = true;
                showTier0();
                resourceManager.SpendResource("Wood", 1);
                resourceManager.SpendResource("Metal", 1);
            }
        }
        if (currentTier == 1)
        {
            if (!UnlockedWeaponManager.isAk47crafted && (MetalCount >= 2 && woodCount >= 2))
            {
                UnlockedWeaponManager.isAk47crafted = true;
                showTier1();
                resourceManager.SpendResource("Wood", 2);
                resourceManager.SpendResource("Metal", 2);
            }
        }
        if (currentTier == 2)
        {
            if (!UnlockedWeaponManager.isBazookacrafted && (MetalCount >= 2 && woodCount >= 2 && PlasticCount >= 2))
            {
                UnlockedWeaponManager.isBazookacrafted = true;
                showTier2();
                resourceManager.SpendResource("Wood", 2);
                resourceManager.SpendResource("Metal", 3);
                resourceManager.SpendResource("Plastic", 2);
            }
        }
        if (currentTier == 3)
        {
            if (!UnlockedWeaponManager.isPlasmaRiflecrafted && (MetalCount >= 3 && PlasticCount >= 3 && ReactorCount >= 1))
            {
                UnlockedWeaponManager.isPlasmaRiflecrafted = true;
                showTier3();
                resourceManager.SpendResource("Wood", 3);
                resourceManager.SpendResource("Metal", 3);
                resourceManager.SpendResource("Plastic", 1);
            }
        }
    }

    void showTier0()
    {
        uiCrafting.rootVisualElement.Q<Label>("WeaponTier").text = "Tier 0";

        uiCrafting.rootVisualElement.Q<Label>("MeleeWeaponName").text = "Survival Knife";
        uiCrafting.rootVisualElement.Q<Image>("MeleeWeaponImage").image = KnifeImage;
        uiCrafting.rootVisualElement.Q<Label>("MeleeWeaponDescription").text = " Basic Knife that anyone \n should have.\n Not suitable to combat.\n Recipe:\n\tx1 metal";

        uiCrafting.rootVisualElement.Q<Label>("RangedWeaponName").text = "Pistol";
        uiCrafting.rootVisualElement.Q<Image>("RangedWeaponImage").image = PistolImage;
        uiCrafting.rootVisualElement.Q<Label>("RangedWeaponDescription").text = " Its a gun!\n Does decent damage.\n Magazine size: 6\n Bullet speed: slow\n Recipe:\n\tx1 wood\n\tx1 metal";

        currentTier = 0;
        if (UnlockedWeaponManager.isKnifecrafted)
        {
            var meleeCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("MeleeCraftButton");
            meleeCraftBuutton.text = "Crafted";
        }
        else
        {
            var meleeCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("MeleeCraftButton");
            meleeCraftBuutton.text = "Craft";
        }
        if (UnlockedWeaponManager.isPistolcrafted)
        {
            var rangedCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("RangedCraftButton");
            rangedCraftBuutton.text = "Crafted";
        }
        else
        {
            var rangedCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("RangedCraftButton");
            rangedCraftBuutton.text = "Craft";
        }
    }
    void showTier1()
    {
        if (UnlockedWeaponManager.unlockedTier < 1) {
            return;
        }

        uiCrafting.rootVisualElement.Q<Label>("WeaponTier").text = "Tier 1";

        uiCrafting.rootVisualElement.Q<Label>("MeleeWeaponName").text = "Machete";
        uiCrafting.rootVisualElement.Q<Image>("MeleeWeaponImage").image = MacheteImage;
        uiCrafting.rootVisualElement.Q<Label>("MeleeWeaponDescription").text = " Sharp Machete.\n Watch your finger.\n Recipe:\n\tx1 wood\n\tx3 metal";

        uiCrafting.rootVisualElement.Q<Label>("RangedWeaponName").text = "Ak47";
        uiCrafting.rootVisualElement.Q<Image>("RangedWeaponImage").image = Ak47Image;
        uiCrafting.rootVisualElement.Q<Label>("RangedWeaponDescription").text = " Its a stronger gun!\n Does good damage.\n Magazine size: 60\n Bullet speed: Medium\n Recipe:\n\tx2 wood\n\tx2 metal";

        currentTier = 1;
        if (UnlockedWeaponManager.isMachetecrafted)
        {
            var meleeCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("MeleeCraftButton");
            meleeCraftBuutton.text = "Crafted";
        }
        else
        {
            var meleeCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("MeleeCraftButton");
            meleeCraftBuutton.text = "Craft";
        }
        if (UnlockedWeaponManager.isAk47crafted)
        {
            var rangedCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("RangedCraftButton");
            rangedCraftBuutton.text = "Crafted";
        }
        else
        {
            var rangedCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("RangedCraftButton");
            rangedCraftBuutton.text = "Craft";
        }
    }
    void showTier2()
    {
        if (UnlockedWeaponManager.unlockedTier < 2)
        {
            return;
        }

        uiCrafting.rootVisualElement.Q<Label>("WeaponTier").text = "Tier 2";

        uiCrafting.rootVisualElement.Q<Label>("MeleeWeaponName").text = "Spear";
        uiCrafting.rootVisualElement.Q<Image>("MeleeWeaponImage").image = SpearImage;
        uiCrafting.rootVisualElement.Q<Label>("MeleeWeaponDescription").text = " Spear fashioned from a \n giant fork.\n Very pointy.\n Recipe:\n\tx2 wood\n\tx2 metal\n\tx2 plastic";

        uiCrafting.rootVisualElement.Q<Label>("RangedWeaponName").text = "Bazooka";
        uiCrafting.rootVisualElement.Q<Image>("RangedWeaponImage").image = BazookaImage;
        uiCrafting.rootVisualElement.Q<Label>("RangedWeaponDescription").text = " Its the big gun!\n Projectiles go boom.\n Magazine size: 1\n Bullet speed: medium\n Recipe:\n\tx2 wood\n\tx2 metal\n\tx2 plastic";

        currentTier = 2;
        if (UnlockedWeaponManager.isSpearcrafted)
        {
            var meleeCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("MeleeCraftButton");
            meleeCraftBuutton.text = "Crafted";
        }
        else
        {
            var meleeCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("MeleeCraftButton");
            meleeCraftBuutton.text = "Craft";
        }
        if (UnlockedWeaponManager.isBazookacrafted)
        {
            var rangedCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("RangedCraftButton");
            rangedCraftBuutton.text = "Crafted";
        }
        else
        {
            var rangedCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("RangedCraftButton");
            rangedCraftBuutton.text = "Craft";
        }
    }
    void showTier3()
    {
        if (UnlockedWeaponManager.unlockedTier < 3)
        {
            return;
        }

        uiCrafting.rootVisualElement.Q<Label>("WeaponTier").text = "Tier 3";

        uiCrafting.rootVisualElement.Q<Label>("MeleeWeaponName").text = "Plasma Blade";
        uiCrafting.rootVisualElement.Q<Image>("MeleeWeaponImage").image = PlasmaBladeImage;
        uiCrafting.rootVisualElement.Q<Label>("MeleeWeaponDescription").text = " Uses a reactor to power \n the sword.\n It burns!!!.\n Recipe:\n\tx1 wood\n\tx3 metal\n\tx2 plastic\n\tx1 Reactor";

        uiCrafting.rootVisualElement.Q<Label>("RangedWeaponName").text = "Bazooka";
        uiCrafting.rootVisualElement.Q<Image>("RangedWeaponImage").image = PlasmaRifleImage;
        uiCrafting.rootVisualElement.Q<Label>("RangedWeaponDescription").text = " The bullets are made\n of plasma.\n Magazine size: 60\n Bullet speed: High\n Recipe:\n\tx3 metal\n\tx3 plastic\n\tx1 Reactor";

        currentTier = 3;
        if (UnlockedWeaponManager.isPlasmaBladecrafted)
        {
            var meleeCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("MeleeCraftButton");
            meleeCraftBuutton.text = "Crafted";
        }
        else
        {
            var meleeCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("MeleeCraftButton");
            meleeCraftBuutton.text = "Craft";
        }
        if (UnlockedWeaponManager.isPlasmaRiflecrafted)
        {
            var rangedCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("RangedCraftButton");
            rangedCraftBuutton.text = "Crafted";
        }
        else
        {
            var rangedCraftBuutton = uiCrafting.rootVisualElement.Q<Button>("RangedCraftButton");
            rangedCraftBuutton.text = "Craft";
        }
    }
}
