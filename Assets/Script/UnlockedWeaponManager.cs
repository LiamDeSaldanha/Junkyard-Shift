using UnityEngine;

public class UnlockedWeaponManager : MonoBehaviour
{

    public static int unlockedTier = 0;
    public static bool isKnifecrafted = true;
    public static bool isMachetecrafted = false;
    public static bool isPlasmaBladecrafted = false;
    public static bool isSpearcrafted = false;
    public static bool isPistolcrafted = false;
    public static bool isAk47crafted = false;
    public static bool isPlasmaRiflecrafted = false;
    public static bool isBazookacrafted = false;


    public static void clearWeapons()
    {
          unlockedTier = 0;
     isKnifecrafted = true;
     isMachetecrafted = false;
     isPlasmaBladecrafted = false;
    isSpearcrafted = false;
     isPistolcrafted = false;
     isAk47crafted = false;
     isPlasmaRiflecrafted = false;
     isBazookacrafted = false;
}

}
