using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    public StatCollection stats = new StatCollection();
    public float baseHealth = 100f;
    public float baseDamage = 10f;
    public float baseDefense = 5f;
    public float baseXpGain = 0;
    public float baseMovementSpeed = 5f;

    void Start()
    {
        SetupPlayerStats();
    }
    void SetupPlayerStats()
    {
        StatDatabase db = StatDatabase.Instance;

        StatType healthStat = db.GetStat("Health");
        StatType damageStat = db.GetStat("Damage");
        StatType defenseStat = db.GetStat("Defense");
        StatType xpGainStat = db.GetStat("XpGain");
        StatType movementSpeedStat = db.GetStat("MovementSpeed");
        StatType critChanceStat = db.GetStat("CriticalChance");
        StatType critDamageStat = db.GetStat("CriticalDamage");
        StatType fireAttackStat = db.GetStat("FireAttributeDamage");
        StatType waterAttackStat = db.GetStat("WaterAttributeDamage");
        StatType windAttackStat = db.GetStat("WindAttributeDamage");
        StatType darknessAttackStat = db.GetStat("DarknessAttributeDamage");
        StatType lightAttackStat = db.GetStat("LightAttributeDamage");

        // Initialize base stats, remember to add equipment stats later
        stats.SetStat(healthStat, baseHealth);
        stats.SetStat(damageStat, baseDamage);
        stats.SetStat(defenseStat, baseDefense);
        stats.SetStat(xpGainStat, baseXpGain);
        stats.SetStat(movementSpeedStat, baseMovementSpeed);
        stats.SetStat(critChanceStat, 0f);
        stats.SetStat(critDamageStat, 0f);
        stats.SetStat(fireAttackStat, 0f);
        stats.SetStat(waterAttackStat, 0f);
        stats.SetStat(windAttackStat, 0f);
        stats.SetStat(darknessAttackStat, 0f);
        stats.SetStat(lightAttackStat, 0f);

        // Add equipment stats
        StatCollection equipmentStats = EquipmentManager.Instance.CalculateEquipmentStats();
        foreach (StatType statType in db.GetAllStatTypes())
        {
            float totalValue = stats.GetStat(statType) + equipmentStats.GetStat(statType);
            stats.SetStat(statType, totalValue);
        }
    }
    public void UpdatePlayerStat(StatType statType, float value)
    {
        stats.SetStat(statType, value);
    }
    public void GetEquipmentStats(StatCollection equipmentStats)
    {
        stats.AddStats(equipmentStats);
    }

    void Update()
    {
        
    }
}
