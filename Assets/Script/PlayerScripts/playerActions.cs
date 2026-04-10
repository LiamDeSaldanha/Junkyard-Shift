using BigRookGames.Weapons;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class playerActions : MonoBehaviour
{
    public Transform handT;
    public UIDocument uiReload;
    public UIDocument uiGunDetails;

    private bool isReloading = false;

    // melee weapon rotation
    private Vector3 startRot = new Vector3(180, 180, 540);
    private Vector3 startPos = new Vector3(1.25f, -0.25f, 0.9f);
    private Vector3 windupPos = new Vector3(1.5f, 1f, 1f);
    private Vector3 windupRot = new Vector3(209, 180, 549);
    private Vector3 endRot = new Vector3(121.5f, 415.6f, 797.8f);
    private Vector3 endPos = new Vector3(-0.22f, -0.4f, 0.86f);
    private float windUpDuration = 0.3f;
    private float attackDuration = 0.2f;
    private float impactWaitDuration = 0.2f;
    private float attackRecoveryDuration = 0.2f;

    private bool currentMeleeAttack = false;
    private bool currentRangedAttack = false;

    // ranged weapon details
    private int pistolShotsFired = 0;
    private int Ak47ShotsFired = 0;
    private int PlasmaGUnShotsFired = 0;
    private int bazookaShotsFired = 0;

    private int pistolCapacity = 6;
    private int Ak47Capacity = 60;
    private int plasmaGunCapacity = 60;
    private int bazookaCapacity = 1;

    private float pistolReloadTime = 1.0f;
    private float Ak47ReloadTime = 2.0f;
    private float plasmaGunReloadTime = 4.0f;
    private float bazookaReloadTime = 10.0f;
    private float maxFireRate = 0.2f; // per 5 per s

    // --- Bazooka Audio ---
    public AudioClip bazookaShotClip;
    public AudioClip bazookaReloadClip;
    public AudioSource bazookaSource;
    public AudioSource bazookaReloadSource;
    public Vector2 bazookaAudioPitch = new Vector2(.9f, 1.1f);

    // --- Pistol Audio ---
    public AudioClip pistolShotClip;
    public AudioClip pistolReloadClip;
    public AudioSource pistolSource;
    public AudioSource pistolReloadSource;
    public Vector2 pistolAudioPitch = new Vector2(.9f, 1.1f);

    // --- AK47 Audio ---
    public AudioClip ak47ShotClip;
    public AudioClip ak47ReloadClip;
    public AudioSource ak47Source;
    public AudioSource ak47ReloadSource;
    public Vector2 ak47AudioPitch = new Vector2(.9f, 1.1f);

    // --- Plasma Rifle Audio ---
    public AudioClip plasmaRifleShotClip;
    public AudioClip plasmaRifleReloadClip;
    public AudioSource plasmaRifleSource;
    public AudioSource plasmaRifleReloadSource;
    public Vector2 plasmaRifleAudioPitch = new Vector2(.9f, 1.1f);

    // --- Melee Audio ---
    public AudioClip knifeSwingClip;
    public AudioSource knifeSource;
    public AudioClip macheteSwingClip;
    public AudioSource macheteSource;
    public AudioClip plasmaBladeSwingClip;
    public AudioSource plasmaBladeSource;
    public AudioClip spearSwingClip;
    public AudioSource spearSource;

    // --- Muzzle ---
    public GameObject bazookaMuzzlePrefab;
    public GameObject bazookaMuzzlePosition;
    public GameObject plasmaRifleBulletPosition;
    public GameObject Ak47BulletPosition;
    public GameObject pistolBulletPosition;

    // projectiles
    public GameObject bazookaProjectilePrefab;
    public GameObject plasmaRifleProjectilePrefab;
    public GameObject Ak47ProjectilePrefab;
    public GameObject pistolProjectilePrefab;

    private InputAction leftMouseClick;

    private void Awake()
    {
        leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
        leftMouseClick.performed += ctx => LeftMouseClicked();
        leftMouseClick.canceled += ctx => LeftMouseReleased(ctx);
        leftMouseClick.Enable();
        currentMeleeAttack = false;
        currentRangedAttack = false;
        transform.Find("playerViewCamera/playerHand/PlasmaBlade/Collider").gameObject.GetComponent<BoxCollider>().enabled = false;
        uiReload.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void OnDestroy()
    {
        leftMouseClick.performed -= ctx => LeftMouseClicked();
        leftMouseClick.Disable();
    }

    /// <summary>
    /// Plays a weapon sound using the fire-and-forget pattern:
    /// if the AudioSource is a child of this object, play it directly;
    /// otherwise instantiate a temporary clone, randomise its pitch, play
    /// the clip once, then destroy the clone after 4 seconds.
    /// </summary>
    private void PlayWeaponSound(AudioSource source, AudioClip clip, Vector2 pitchRange)
    {
        if (source == null || clip == null) return;

        if (source.transform.IsChildOf(transform))
        {
            source.Play();
        }
        else
        {
            AudioSource newAS = Instantiate(source);
            if (newAS != null)
            {
                newAS.pitch = Random.Range(pitchRange.x, pitchRange.y);
                newAS.PlayOneShot(clip);
                Destroy(newAS.gameObject, 4);
            }
        }
    }

    private void LeftMouseReleased(InputAction.CallbackContext context)
    {
        if (PlayerMouseView.ignoreMouse)
        {
            return;
        }

        Transform trfKnife = transform.Find("playerViewCamera/playerHand/Knife");
        Transform trfMachete = transform.Find("playerViewCamera/playerHand/Machete");
        Transform trfPlasmaBlade = transform.Find("playerViewCamera/playerHand/PlasmaBlade");
        Transform trfSpear = transform.Find("playerViewCamera/playerHand/Spear");
        Transform trfPistol = transform.Find("playerViewCamera/playerHand/Pistol");
        Transform trfAk47 = transform.Find("playerViewCamera/playerHand/Ak47");
        Transform trfPlasmaRifle = transform.Find("playerViewCamera/playerHand/PlasmaRifle");
        Transform trfBazooka = transform.Find("playerViewCamera/playerHand/Bazooka");

        Debug.Log("found all weapons transfrom on attack");

        GameObject objKnife = trfKnife.gameObject;
        GameObject objMachete = trfMachete.gameObject;
        GameObject objPlasmaBlade = trfPlasmaBlade.gameObject;
        GameObject objSpear = trfSpear.gameObject;

        GameObject objPistol = trfPistol.gameObject;
        GameObject objAk47 = trfAk47.gameObject;
        GameObject objPlasmaRifle = trfPlasmaRifle.gameObject;
        GameObject objBazooka = trfBazooka.gameObject;

        bool melee = false;
        bool ranged = false;

        if (objKnife.activeInHierarchy)
        {
            objKnife.GetComponent<MeshCollider>().convex = true;
            melee = true;
        }
        if (objMachete.activeInHierarchy)
        {
            objMachete.GetComponent<MeshCollider>().convex = true;
            melee = true;
        }
        if (objPlasmaBlade.activeInHierarchy)
        {
            objPlasmaBlade.transform.Find("Collider").GetComponent<BoxCollider>().enabled = true;
            melee = true;
        }
        if (objSpear.activeInHierarchy)
        {
            objSpear.GetComponent<MeshCollider>().convex = true;
            melee = true;
        }
        int weaponVal = 0;
        if (objPistol.activeInHierarchy)
        {
            weaponVal = 0;
            ranged = true;
        }
        if (objAk47.activeInHierarchy)
        {
            weaponVal = 1;
            ranged = true;
        }
        if (objPlasmaRifle.activeInHierarchy)
        {
            weaponVal = 2;
            ranged = true;
        }
        if (objBazooka.activeInHierarchy)
        {
            weaponVal = 3;
            ranged = true;
        }

        if (ranged && !melee)
        {
            currentRangedAttack = false;
        }

    }

    private void LeftMouseClicked()
    {
        if (PlayerMouseView.ignoreMouse)
        {
            return;
        }

        Debug.Log("Left mouse button clicked via the new Input System!");
        // Detect Mouse Click (Left Click)
        if (!currentMeleeAttack && !currentRangedAttack)
        {

            Transform trfKnife = transform.Find("playerViewCamera/playerHand/Knife");
            Transform trfMachete = transform.Find("playerViewCamera/playerHand/Machete");
            Transform trfPlasmaBlade = transform.Find("playerViewCamera/playerHand/PlasmaBlade");
            Transform trfSpear = transform.Find("playerViewCamera/playerHand/Spear");
            Transform trfPistol = transform.Find("playerViewCamera/playerHand/Pistol");
            Transform trfAk47 = transform.Find("playerViewCamera/playerHand/Ak47");
            Transform trfPlasmaRifle = transform.Find("playerViewCamera/playerHand/PlasmaRifle");
            Transform trfBazooka = transform.Find("playerViewCamera/playerHand/Bazooka");

            Debug.Log("found all weapons transfrom on attack");

            GameObject objKnife = trfKnife.gameObject;
            GameObject objMachete = trfMachete.gameObject;
            GameObject objPlasmaBlade = trfPlasmaBlade.gameObject;
            GameObject objSpear = trfSpear.gameObject;

            GameObject objPistol = trfPistol.gameObject;
            GameObject objAk47 = trfAk47.gameObject;
            GameObject objPlasmaRifle = trfPlasmaRifle.gameObject;
            GameObject objBazooka = trfBazooka.gameObject;

            bool melee = false;
            bool ranged = false;

            if (objKnife.activeInHierarchy)
            {
                objKnife.GetComponent<MeshCollider>().convex = true;
                melee = true;
            }
            if (objMachete.activeInHierarchy)
            {
                objMachete.GetComponent<MeshCollider>().convex = true;
                melee = true;
            }
            if (objPlasmaBlade.activeInHierarchy)
            {
                objPlasmaBlade.transform.Find("Collider").GetComponent<BoxCollider>().enabled = true;
                melee = true;
            }
            if (objSpear.activeInHierarchy)
            {
                objSpear.GetComponent<MeshCollider>().convex = true;
                melee = true;
            }
            int weaponVal = 0;
            if (objPistol.activeInHierarchy)
            {
                weaponVal = 0;
                ranged = true;
            }
            if (objAk47.activeInHierarchy)
            {
                weaponVal = 1;
                ranged = true;
            }
            if (objPlasmaRifle.activeInHierarchy)
            {
                weaponVal = 2;
                ranged = true;
            }
            if (objBazooka.activeInHierarchy)
            {
                weaponVal = 3;
                ranged = true;
            }

            if (melee)
            {
                currentMeleeAttack = true;
                StartCoroutine(attackWithMeleeWeapon());
            }
            if (ranged)
            {
                currentRangedAttack = true;
                if (weaponVal == 0)
                {
                    if (pistolShotsFired < pistolCapacity)
                    {
                        StartCoroutine(startFireRangedWeapon(weaponVal));
                    }
                    else
                    {
                        isReloading = true;
                        pistolShotsFired = 0;
                        StartCoroutine(reloadRangedWeapon(weaponVal));
                    }
                }
                if (weaponVal == 1)
                {
                    if (Ak47ShotsFired < Ak47Capacity)
                    {
                        StartCoroutine(startFireRangedWeapon(weaponVal));
                    }
                    else
                    {
                        isReloading = true;
                        Ak47ShotsFired = 0;
                        StartCoroutine(reloadRangedWeapon(weaponVal));
                    }
                }
                if (weaponVal == 2)
                {
                    if (PlasmaGUnShotsFired < plasmaGunCapacity)
                    {
                        StartCoroutine(startFireRangedWeapon(weaponVal));
                    }
                    else
                    {
                        isReloading = true;
                        PlasmaGUnShotsFired = 0;
                        StartCoroutine(reloadRangedWeapon(weaponVal));
                    }
                }
                if (weaponVal == 3)
                {
                    if (bazookaShotsFired < bazookaCapacity)
                    {
                        StartCoroutine(startFireRangedWeapon(weaponVal));

                    }
                    else
                    {
                        isReloading = true;
                        bazookaShotsFired = 0;
                        StartCoroutine(reloadRangedWeapon(weaponVal));
                    }
                }
            }
            if (!melee && !ranged)
            {
                currentMeleeAttack = false;
                currentRangedAttack = false;
            }
            Debug.Log("Finish Attack");
        }
    }

    private void fireRangedWeapon(int val)
    {
        Debug.Log("Fire weapon");
        GameObject obj = GameObject.Find("ProjectileManager");
        if (val == 3)
        {
            Debug.Log("Fire bazooka");
            var flash = Instantiate(bazookaMuzzlePrefab, bazookaMuzzlePosition.transform);
            Destroy(flash, 1);

            GameObject newProjectile = Instantiate(bazookaProjectilePrefab, bazookaMuzzlePosition.transform.position, bazookaMuzzlePosition.transform.rotation, transform);
            newProjectile.transform.SetParent(obj.transform);

            // --- Bazooka Shot Sound ---
            PlayWeaponSound(bazookaSource, bazookaShotClip, bazookaAudioPitch);

            uiGunDetails.rootVisualElement.Q<Label>("GunName").text = "Bazooka";
            uiGunDetails.rootVisualElement.Q<Label>("AmmoCount").text = (bazookaCapacity - bazookaShotsFired) + " / " + bazookaCapacity;

        }
        else if (val == 2)
        {
            Debug.Log("plasma gun fired");

            uiGunDetails.rootVisualElement.Q<Label>("GunName").text = "Plasma Gun";
            uiGunDetails.rootVisualElement.Q<Label>("AmmoCount").text = (plasmaGunCapacity - PlasmaGUnShotsFired) + " / " + plasmaGunCapacity;

            GameObject newProjectile = Instantiate(plasmaRifleProjectilePrefab, plasmaRifleBulletPosition.transform.position, plasmaRifleBulletPosition.transform.rotation, transform);
            newProjectile.transform.SetParent(obj.transform);

            // --- Plasma Rifle Shot Sound ---
            PlayWeaponSound(plasmaRifleSource, plasmaRifleShotClip, plasmaRifleAudioPitch);
        }
        else if (val == 1)
        {
            Debug.Log("Ak47 fired");

            uiGunDetails.rootVisualElement.Q<Label>("GunName").text = "Ak47";
            uiGunDetails.rootVisualElement.Q<Label>("AmmoCount").text = (Ak47Capacity - Ak47ShotsFired) + " / " + Ak47Capacity;

            GameObject newProjectile = Instantiate(Ak47ProjectilePrefab, Ak47BulletPosition.transform.position, Ak47BulletPosition.transform.rotation, transform);
            newProjectile.transform.SetParent(obj.transform);

            // --- AK47 Shot Sound ---
            PlayWeaponSound(ak47Source, ak47ShotClip, ak47AudioPitch);
        }
        else if (val == 0)
        {
            Debug.Log("pistol fired");

            uiGunDetails.rootVisualElement.Q<Label>("GunName").text = "Pistol";
            uiGunDetails.rootVisualElement.Q<Label>("AmmoCount").text = (pistolCapacity - pistolShotsFired) + " / " + pistolCapacity;

            GameObject newProjectile = Instantiate(pistolProjectilePrefab, pistolBulletPosition.transform.position, pistolBulletPosition.transform.rotation, obj.transform);

            // --- Pistol Shot Sound ---
            PlayWeaponSound(pistolSource, pistolShotClip, pistolAudioPitch);
        }
        Debug.Log("ranged weapon fired");
    }

    IEnumerator startFireRangedWeapon(int val)
    {
        float currentShots = 0;
        float capacity = 0;
        if (val == 0)
        { currentShots = pistolShotsFired; capacity = pistolCapacity; }
        if (val == 1)
        { currentShots = Ak47ShotsFired; capacity = Ak47Capacity; }
        if (val == 2)
        { currentShots = PlasmaGUnShotsFired; capacity = plasmaGunCapacity; }
        if (val == 3)
        { currentShots = bazookaShotsFired; capacity = bazookaCapacity; }

        while (currentRangedAttack && currentShots < capacity + 1 && !isReloading)
        {
            float elapsed = 0;
            while (elapsed < maxFireRate)
            {
                elapsed += Time.deltaTime;
                yield return null; // Wait for next frame
            }
            fireRangedWeapon(val);

            if (val == 0)
            { currentShots = ++pistolShotsFired; capacity = pistolCapacity; }
            if (val == 1)
            { currentShots = ++Ak47ShotsFired; capacity = Ak47Capacity; }
            if (val == 2)
            { currentShots = ++PlasmaGUnShotsFired; capacity = plasmaGunCapacity; }
            if (val == 3)
            { currentShots = ++bazookaShotsFired; capacity = bazookaCapacity; }
        }
    }

    IEnumerator reloadRangedWeapon(int val)
    {

        uiReload.rootVisualElement.style.display = DisplayStyle.Flex;

        float elapsed = 0.0f;
        float reloadTime = 1.0f;
        if (val == 0)
        {
            reloadTime = pistolReloadTime;

            PlayWeaponSound(pistolReloadSource, pistolReloadClip, pistolAudioPitch);
            // --- Pistol Reload Sound ---
            // TODO: assign pistolReloadClip and pistolReloadSource in the Inspector
            // PlayWeaponSound(pistolReloadSource, pistolReloadClip, pistolAudioPitch);
        }
        if (val == 1)
        {
            reloadTime = Ak47ReloadTime;
            PlayWeaponSound(ak47ReloadSource, ak47ReloadClip, ak47AudioPitch);
            // --- AK47 Reload Sound ---
            // TODO: assign ak47ReloadClip and ak47ReloadSource in the Inspector
            // PlayWeaponSound(ak47ReloadSource, ak47ReloadClip, ak47AudioPitch);
        }
        if (val == 2)
        {
            reloadTime = plasmaGunReloadTime;
            PlayWeaponSound(plasmaRifleReloadSource, plasmaRifleReloadClip, plasmaRifleAudioPitch);
            // --- Plasma Rifle Reload Sound ---
            // TODO: assign plasmaRifleReloadClip and plasmaRifleReloadSource in the Inspector
            // PlayWeaponSound(plasmaRifleReloadSource, plasmaRifleReloadClip, plasmaRifleAudioPitch);
        }
        if (val == 3) // Note: was incorrectly val == 4 in original
        {
            reloadTime = bazookaReloadTime;
            PlayWeaponSound(bazookaReloadSource, bazookaReloadClip, bazookaAudioPitch);
            // --- Bazooka Reload Sound ---
            // TODO: assign bazookaReloadClip and bazookaReloadSource in the Inspector
            // PlayWeaponSound(bazookaReloadSource, bazookaReloadClip, bazookaAudioPitch);
        }

        while (elapsed < reloadTime)
        {

            elapsed += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        uiReload.rootVisualElement.style.display = DisplayStyle.None;
        currentRangedAttack = false;
        isReloading = false;
    }

    IEnumerator attackWithMeleeWeapon()
    {

        Debug.Log("Rotating hand object");
        Quaternion startRotation = Quaternion.Euler(startRot);
        Vector3 startPosition = startPos;
        Quaternion endRotation = Quaternion.Euler(windupRot);
        Vector3 endPosition = windupPos;
        float elapsed = 0.0f;

        while (elapsed < windUpDuration)
        {
            // Smoothly interpolate between start and end rotation
            handT.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsed / (windUpDuration));
            handT.localPosition = Vector3.Slerp(startPosition, endPosition, elapsed / (windUpDuration));

            elapsed += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure final rotation is exact
        handT.localRotation = endRotation;
        handT.localPosition = endPosition;

        // --- Melee Swing Sound ---
        // Triggered here, at the moment the wind-up finishes and the actual
        // swing begins, so the sound lands in sync with the strike animation.
        {
            Transform trfKnife = transform.Find("playerViewCamera/playerHand/Knife");
            Transform trfMachete = transform.Find("playerViewCamera/playerHand/Machete");
            Transform trfPlasmaBlade = transform.Find("playerViewCamera/playerHand/PlasmaBlade");
            Transform trfSpear = transform.Find("playerViewCamera/playerHand/Spear");

            if (trfKnife.gameObject.activeInHierarchy)
                PlayWeaponSound(knifeSource, knifeSwingClip, new Vector2(0.9f, 1.1f));

            if (trfMachete.gameObject.activeInHierarchy)
                PlayWeaponSound(macheteSource, macheteSwingClip, new Vector2(0.9f, 1.1f));

            if (trfPlasmaBlade.gameObject.activeInHierarchy)
                PlayWeaponSound(plasmaBladeSource, plasmaBladeSwingClip, new Vector2(0.9f, 1.1f));

            if (trfSpear.gameObject.activeInHierarchy)
                PlayWeaponSound(spearSource, spearSwingClip, new Vector2(0.9f, 1.1f));
        }

        startRotation = Quaternion.Euler(windupRot);
        startPosition = windupPos;
        endRotation = Quaternion.Euler(endRot);
        endPosition = endPos;
        elapsed = 0.0f;

        while (elapsed < attackDuration)
        {
            // Smoothly interpolate between start and end rotation
            handT.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsed / (attackDuration));
            handT.localPosition = Vector3.Slerp(startPosition, endPosition, elapsed / (attackDuration));

            elapsed += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure final rotation is exact
        handT.localRotation = endRotation;
        handT.localPosition = endPosition;

        elapsed = 0.0f;
        while (elapsed < impactWaitDuration)
        {
            elapsed += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        startRotation = Quaternion.Euler(endRot);
        startPosition = endPosition;
        endRotation = Quaternion.Euler(startRot);
        endPosition = startPos;
        elapsed = 0.0f;

        while (elapsed < attackRecoveryDuration)
        {
            // Smoothly interpolate between start and end rotation
            handT.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsed / (attackRecoveryDuration));
            handT.localPosition = Vector3.Slerp(startPosition, endPosition, elapsed / (attackRecoveryDuration));

            elapsed += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure final rotation is exact
        handT.localRotation = endRotation;
        handT.localPosition = endPosition;

        Transform trfKnifeFinal = transform.Find("playerViewCamera/playerHand/Knife");
        Transform trfMacheteFinal = transform.Find("playerViewCamera/playerHand/Machete");
        Transform trfPlasmaBladeFinal = transform.Find("playerViewCamera/playerHand/PlasmaBlade");
        Transform trfSpearFinal = transform.Find("playerViewCamera/playerHand/Spear");
        GameObject objKnife = trfKnifeFinal.gameObject;
        GameObject objMachete = trfMacheteFinal.gameObject;
        GameObject objPlasmaBlade = trfPlasmaBladeFinal.gameObject;
        GameObject objSpear = trfSpearFinal.gameObject;

        if (objKnife.activeInHierarchy)
        {
            objKnife.GetComponent<MeshCollider>().convex = false;
        }
        if (objMachete.activeInHierarchy)
        {
            objMachete.GetComponent<MeshCollider>().convex = false;

        }
        if (objPlasmaBlade.activeInHierarchy)
        {
            objPlasmaBlade.transform.Find("Collider").GetComponent<BoxCollider>().enabled = false;
        }
        if (objSpear.activeInHierarchy)
        {
            objSpear.GetComponent<MeshCollider>().convex = false;
        }

        currentMeleeAttack = false;

    }
}