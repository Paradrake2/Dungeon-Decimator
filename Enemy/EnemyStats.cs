using System.Collections.Generic;
using UnityEngine;

public enum EnemyAttribute
{
    None,
    Normal,
    Fire,
    Water,
    Wind,
    Darkness,
    Light
}


public class EnemyStats : MonoBehaviour
{
    StatCollection stats = new StatCollection();

    public float baseHealth = 50f;
    public float currentHealth = 50f;
    public float baseDamage = 15f;
    public float baseDefense = 3f;
    public float baseMovementSpeed = 4f;
    public float attackSpeed = 1f; // used for ranged enemies

    public EnemyAttribute enemyAttribute = EnemyAttribute.None;
    public float fireWeaknessStat = 1f;
    public float waterWeaknessStat = 1f;
    public float windWeaknessStat = 1f;
    public float darknessWeaknessStat = 1f;
    public float lightWeaknessStat = 1f;
    public float fireDamageStat = 0f;
    public float waterDamageStat = 0f;
    public float windDamageStat = 0f;
    public float darknessDamageStat = 0f;
    public float lightDamageStat = 0f;
    void Start()
    {
        SetupEnemyStats();
        currentHealth = baseHealth;
    }
    void SetupEnemyStats()
    {
        StatDatabase db = StatDatabase.Instance;

        StatType healthStat = db.GetStat("Health");
        StatType damageStat = db.GetStat("Damage");
        StatType defenseStat = db.GetStat("Defense");
        StatType movementSpeedStat = db.GetStat("MovementSpeed");
        StatType attackSpeedStat = db.GetStat("AttackSpeed");
        StatType fireWeaknessStat = db.GetStat("FireAttributeWeakness");
        StatType fireDamageStat = db.GetStat("FireAttributeDamage");
        StatType waterWeaknessStat = db.GetStat("WaterAttributeWeakness");
        StatType waterDamageStat = db.GetStat("WaterAttributeDamage");
        StatType windWeaknessStat = db.GetStat("WindAttributeWeakness");
        StatType windDamageStat = db.GetStat("WindAttributeDamage");
        StatType darknessWeaknessStat = db.GetStat("DarknessAttributeWeakness");
        StatType darknessDamageStat = db.GetStat("DarknessAttributeDamage");
        StatType lightWeaknessStat = db.GetStat("LightAttributeWeakness");
        StatType lightDamageStat = db.GetStat("LightAttributeDamage");


        // Initialize base stats
        stats.SetStat(healthStat, baseHealth);
        stats.SetStat(damageStat, baseDamage);
        stats.SetStat(defenseStat, baseDefense);
        stats.SetStat(movementSpeedStat, baseMovementSpeed);
        stats.SetStat(attackSpeedStat, attackSpeed);
        stats.SetStat(fireWeaknessStat, this.fireWeaknessStat);
        stats.SetStat(fireDamageStat, this.fireDamageStat);
        stats.SetStat(waterWeaknessStat, this.waterWeaknessStat);
        stats.SetStat(waterDamageStat, this.waterDamageStat);
        stats.SetStat(windWeaknessStat, this.windWeaknessStat);
        stats.SetStat(windDamageStat, this.windDamageStat);
        stats.SetStat(darknessWeaknessStat, this.darknessWeaknessStat);
        stats.SetStat(darknessDamageStat, this.darknessDamageStat);
        stats.SetStat(lightWeaknessStat, this.lightWeaknessStat);
        stats.SetStat(lightDamageStat, this.lightDamageStat);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
