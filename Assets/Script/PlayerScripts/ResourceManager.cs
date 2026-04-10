using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance;
    public List<RecipeIngredients> resources = new List<RecipeIngredients>();
    void Awake()
    {
        // singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
            Destroy(gameObject);
    }

    public void AddResource(LootItem item, int amount)
    {
        RecipeIngredients existing = resources.FirstOrDefault(r => r.item == item);

        if (existing != null)
            existing.amount += amount;
        else
        {
            if (item.resourceName== "Wood")
            {
                UnlockedWeaponManager.unlockedTier = Mathf.Max(UnlockedWeaponManager.unlockedTier, 0);

            }
            else if (item.resourceName == "Metal")
            {
                UnlockedWeaponManager.unlockedTier = Mathf.Max(UnlockedWeaponManager.unlockedTier,1);

            }
            else if (item.resourceName == "Plastic")
            {
                UnlockedWeaponManager.unlockedTier = Mathf.Max(UnlockedWeaponManager.unlockedTier, 2);


            }
            else if (item.resourceName == "Reactor")
            {
                UnlockedWeaponManager.unlockedTier = Mathf.Max(UnlockedWeaponManager.unlockedTier, 3);


            }
            resources.Add(new RecipeIngredients { item = item, amount = amount });

        }

        Debug.Log(item.resourceName + " " + resources.First(r => r.item == item).amount);
    }

    public int GetResource(LootItem item)
    {
        RecipeIngredients existing = resources.FirstOrDefault(r => r.item == item);
        if (existing != null)
            return existing.amount;
        return 0;
    }
    public int GetResource(string resourceName)
    {
        RecipeIngredients existing = resources.FirstOrDefault(r => r.item.resourceName == resourceName);
        if (existing != null)
            return existing.amount;
        return 0;
    }

    public bool SpendResource(LootItem item, int amount)
    {
        RecipeIngredients existing = resources.FirstOrDefault(r => r.item == item);
        if (existing != null && existing.amount >= amount)
        {
            existing.amount -= amount;
            return true;
        }
        return false;
    }

    public bool SpendResource(string resourceName,int amount)
    {
        RecipeIngredients existing = resources.FirstOrDefault(r => r.item.resourceName == resourceName);
        if (existing != null && existing.amount >= amount)
        {
            existing.amount -= amount;
            return true;
        }
        return false;
    }

    private void Update()
    {
        
    }
}
