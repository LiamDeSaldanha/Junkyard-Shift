using UnityEngine;

public enum EnemyTier { Tier1, Tier2, Tier3, Boss }

[CreateAssetMenu(fileName = "LootItem", menuName = "Loot/LootItem")]
public class LootItem : ScriptableObject
{
    public string resourceName;
    public GameObject itemPrefab;
    public int amount = 0;

    [Range(0, 100)] public float Tier1DropChance;
    [Range(0, 100)] public float Tier2DropChance;
    [Range(0, 100)] public float Tier3DropChance;
    [Range(0, 100)] public float bossDropChance;

    public float GetDropChance(EnemyTier tier)
    {
        switch (tier)
        {
            case EnemyTier.Tier1: return Tier1DropChance;
            case EnemyTier.Tier2: return Tier2DropChance;
            case EnemyTier.Tier3: return Tier3DropChance;
            case EnemyTier.Boss: return bossDropChance;
            default: return 0f;
        }
    }
}