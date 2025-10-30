using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttributeResistance
{
    public StatType attribute;
    public StatType attributeAttack;
    public float resistanceValue;
}

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    public StatCollection stats = new StatCollection();
    public float baseHealth = 100f;
    public float baseDamage = 10f;
    public float baseDefense = 5f;
    public float baseXpGain = 0;
    public float baseMovementSpeed = 0f;
    public float baseAttackSpeed = 1f;
    public int baseLevel = 1;
    public List<AttributeResistance> attributeResistances = new List<AttributeResistance>();

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
        StatType levelStat = db.GetStat("Level");
        StatType fireResistanceStat = db.GetStat("FireAttributeDefense");
        StatType waterResistanceStat = db.GetStat("WaterAttributeDefense");
        StatType windResistanceStat = db.GetStat("WindAttributeDefense");
        StatType darknessResistanceStat = db.GetStat("DarknessAttributeDefense");
        StatType lightResistanceStat = db.GetStat("LightAttributeDefense");



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
        stats.SetStat(levelStat, baseLevel);
        stats.SetStat(fireResistanceStat, 0f);
        stats.SetStat(waterResistanceStat, 0f);
        stats.SetStat(windResistanceStat, 0f);
        stats.SetStat(darknessResistanceStat, 0f);
        stats.SetStat(lightResistanceStat, 0f);

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
        SetAttributeResistance();
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
        float modifiedDamage = stats.GetStat(damageStat) * modifiers.damageModifier;
        stats.SetStat(damageStat, modifiedDamage);
        StatType defenseStat = StatDatabase.Instance.GetStat("Defense");
        float modifiedDefense = stats.GetStat(defenseStat) * modifiers.defenseModifier;
        stats.SetStat(defenseStat, modifiedDefense);
        StatType movementSpeedStat = StatDatabase.Instance.GetStat("MovementSpeed");
        float modifiedSpeed = stats.GetStat(movementSpeedStat) * modifiers.speedModifier;
        stats.SetStat(movementSpeedStat, modifiedSpeed);
    }
    public void SetAttributeResistance()
    {
        attributeResistances.Clear();
        StatDatabase db = StatDatabase.Instance;
        StatType fireResistanceStat = db.GetStat("FireAttributeDefense");
        StatType waterResistanceStat = db.GetStat("WaterAttributeDefense");
        StatType windResistanceStat = db.GetStat("WindAttributeDefense");
        StatType darknessResistanceStat = db.GetStat("DarknessAttributeDefense");
        StatType lightResistanceStat = db.GetStat("LightAttributeDefense");

        AttributeResistance fireRes = new AttributeResistance
        {
            attribute = db.GetStat("FireAttributeDefense"),
            attributeAttack = db.GetStat("FireAttributeDamage"),
            resistanceValue = stats.GetStat(fireResistanceStat)
        };
        attributeResistances.Add(fireRes);

        AttributeResistance waterRes = new AttributeResistance
        {
            attribute = db.GetStat("WaterAttributeDefense"),
            attributeAttack = db.GetStat("WaterAttributeDamage"),
            resistanceValue = stats.GetStat(waterResistanceStat)
        };
        attributeResistances.Add(waterRes);

        AttributeResistance windRes = new AttributeResistance
        {
            attribute = db.GetStat("WindAttributeDefense"),
            attributeAttack = db.GetStat("WindAttributeDamage"),
            resistanceValue = stats.GetStat(windResistanceStat)
        };
        attributeResistances.Add(windRes);

        AttributeResistance darknessRes = new AttributeResistance
        {
            attribute = db.GetStat("DarknessAttributeDefense"),
            attributeAttack = db.GetStat("DarknessAttributeDamage"),
            resistanceValue = stats.GetStat(darknessResistanceStat)
        };
        attributeResistances.Add(darknessRes);

        AttributeResistance lightRes = new AttributeResistance
        {
            attribute = db.GetStat("LightAttributeDefense"),
            attributeAttack = db.GetStat("LightAttributeDamage"),
            resistanceValue = stats.GetStat(lightResistanceStat)
        };
        attributeResistances.Add(lightRes);
    }
    public List<AttributeResistance> GetAttributeResistances()
    {
        return attributeResistances;
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
        float finalSpeed = speed * (1 + stats.GetStat(attackSpeedModifierStat));
        return finalSpeed;
    }
}
