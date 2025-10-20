using UnityEngine;

/// <summary>
/// Example usage of the new stat system
/// This shows how much cleaner and more flexible the new approach is
/// </summary>
public class StatSystemExample : MonoBehaviour
{
    [Header("Example Stat Usage")]
    public StatCollection playerStats = new StatCollection();
    public StatCollection weaponStats = new StatCollection();
    
    [Header("Example Material")]
    public CraftingMaterial exampleMaterial;
    
    private void Start()
    {
        DemonstrateStatSystem();
    }
    
    private void DemonstrateStatSystem()
    {
        // Get stat database (you'll need to create one in Resources folder)
        StatDatabase db = StatDatabase.Instance;
        if (db == null)
        {
            Debug.LogWarning("No StatDatabase found. Create one in Resources folder.");
            return;
        }
        
        // Example: Setting player stats dynamically
        StatType healthStat = db.GetStat("Health");
        StatType damageStat = db.GetStat("Damage");
        StatType critChanceStat = db.GetStat("CriticalChance");
        
        if (healthStat != null) playerStats.SetStat(healthStat, 100f);
        if (damageStat != null) playerStats.SetStat(damageStat, 25f);
        if (critChanceStat != null) playerStats.SetStat(critChanceStat, 5f);
        
        // Example: Reading stats
        float playerHealth = playerStats.GetStat(healthStat);
        float playerDamage = playerStats.GetStat(damageStat);
        
        Debug.Log($"Player Health: {playerHealth}");
        Debug.Log($"Player Damage: {playerDamage}");
        
        // Example: Combining stats from equipment
        if (weaponStats.Stats.Count > 0)
        {
            StatCollection totalStats = new StatCollection();
            totalStats.AddStats(playerStats);
            totalStats.AddStats(weaponStats);
            
            Debug.Log($"Total damage with weapon: {totalStats.GetStat(damageStat)}");
        }
        
        // Example: Working with crafting materials
        if (exampleMaterial != null && exampleMaterial.stats != null)
        {
            Debug.Log($"Material {exampleMaterial.materialName} stats:");
            foreach (var stat in exampleMaterial.stats.stats.Stats)
            {
                Debug.Log($"- {stat.GetDisplayText()}");
            }
        }
    }
    
    // Example: Method to apply material stats to player
    public void ApplyMaterialBonus(CraftingMaterial material)
    {
        if (material?.stats?.stats != null)
        {
            playerStats.AddStats(material.stats.stats);
            Debug.Log($"Applied bonuses from {material.materialName}");
        }
    }
    
    // Example: Get formatted stat display for UI
    public string GetStatDisplayText(StatType statType)
    {
        float value = playerStats.GetStat(statType);
        return statType.FormatValue(value);
    }
}