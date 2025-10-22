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
    public float baseAttackSpeed = 1f;

    void Start()
    {
        SetupPlayerStats();
    }
    void SetupPlayerStats()
    {
        StatDatabase db = StatDatabase.Instance;

        // Initialize stat types
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
        StatType attackSpeedStat = db.GetStat("AttackSpeed");
        StatType attackSpeedModifierStat = db.GetStat("AttackSpeedModifier");



        // Initialize base stats, remember to add equipment stats later
        stats.SetStat(healthStat, baseHealth);
        stats.SetStat(damageStat, baseDamage);
        stats.SetStat(defenseStat, baseDefense);
        stats.SetStat(xpGainStat, baseXpGain);
        stats.SetStat(movementSpeedStat, baseMovementSpeed);
        stats.SetStat(attackSpeedStat, baseAttackSpeed);
        stats.SetStat(critChanceStat, 0f);
        stats.SetStat(critDamageStat, 0f);
        stats.SetStat(fireAttackStat, 0f);
        stats.SetStat(waterAttackStat, 0f);
        stats.SetStat(windAttackStat, 0f);
        stats.SetStat(darknessAttackStat, 0f);
        stats.SetStat(lightAttackStat, 0f);
        stats.SetStat(attackSpeedModifierStat, 0f); // this will be impacted by equipment and buff items

        // Add equipment stats
        StatCollection equipmentStats = EquipmentManager.Instance.CalculateEquipmentStats();
        foreach (StatType statType in db.GetAllStatTypes())
        {
            Debug.Log("Applying stat");
            float totalValue = stats.GetStat(statType) + equipmentStats.GetStat(statType);
            Debug.Log(statType + "  " + totalValue);
            stats.SetStat(statType, totalValue);
        }

        // add multipliers
        UpdateFromModifiers();
        float healthAfterMult = stats.GetStat(healthStat) * (1 + stats.GetStat(db.GetStat("HealthMult")));
        stats.SetStat(healthStat, healthAfterMult);
        float damageAfterMult = stats.GetStat(damageStat) * (1 + stats.GetStat(db.GetStat("DamageMult")));
        stats.SetStat(damageStat, damageAfterMult);
        float defenseAfterMult = stats.GetStat(defenseStat) * (1 + stats.GetStat(db.GetStat("DefenseMult")));
        stats.SetStat(defenseStat, defenseAfterMult);
    }
    public void UpdatePlayerStat(StatType statType, float value)
    {
        stats.SetStat(statType, value);
    }
    public void GetEquipmentStats(StatCollection equipmentStats)
    {
        stats.AddStats(equipmentStats);
    }

    public void UpdateFromModifiers()
    {
        PlayerModifiers modifiers = FindAnyObjectByType<PlayerModifiers>();
        StatType healthStat = StatDatabase.Instance.GetStat("Health");
        float modifiedHealth = stats.GetStat(healthStat) * modifiers.healthModifier;
        stats.SetStat(healthStat, modifiedHealth);
        StatType damageStat = StatDatabase.Instance.GetStat("Damage");
        float modifiedDamage = stats.GetStat(damageStat) *  modifiers.damageModifier;
        stats.SetStat(damageStat, modifiedDamage);
        StatType defenseStat = StatDatabase.Instance.GetStat("Defense");
        float modifiedDefense = stats.GetStat(defenseStat) * modifiers.defenseModifier;
        stats.SetStat(defenseStat, modifiedDefense);
        StatType movementSpeedStat = StatDatabase.Instance.GetStat("MovementSpeed");
        float modifiedSpeed = stats.GetStat(movementSpeedStat) * modifiers.speedModifier;
        stats.SetStat(movementSpeedStat, modifiedSpeed);
    }

    void Update()
    {

    }

    public float GetStatValue(string statID)
    {
        StatType statType = StatDatabase.Instance.GetStat(statID);
        if (statType != null)
        {
            return stats.GetStat(statType);
        }
        return 0f;
    }

    public void SetStatValue(string statID, float value)
    {
        StatType statType = StatDatabase.Instance.GetStat(statID);
        if (statType != null)
        {
            stats.SetStat(statType, value);
        }
    }
    
    public float GetAttackSpeed()
    {
        StatType attackSpeedStat = StatDatabase.Instance.GetStat("AttackSpeed");
        float speed = stats.GetStat(attackSpeedStat);
        StatType attackSpeedModifierStat = StatDatabase.Instance.GetStat("AttackSpeedModifier");
        return speed;
    }
}
