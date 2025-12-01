using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public ProjectileData projectileData;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    [SerializeField] private float size = 1;
    [SerializeField] private List<AttributeDamage> damageAttributes;
    private float damageMult = 1f;
    void Start()
    {
        
    }
    public void Initialize(Vector3 direction, PlayerStats stats, PlayerModifiers modifiers = null)
    {
        transform.up = direction;
        speed = projectileData.speed;
        damage = projectileData.damage + stats.GetStatValue("Damage");
        damageMult = Mathf.Max(1f, stats.GetStatValue("EquipmentDamageMultiplier"));
        Debug.Log(damageMult);
        lifetime = projectileData.lifetime;
        size = projectileData.size * modifiers.GetProjSizeModifier();
        transform.localScale = Vector3.one * size;
        damageAttributes = new List<AttributeDamage>();
        if (projectileData.damageAttributes != null)
        {
            foreach (var attr in projectileData.damageAttributes)
            {
                damageAttributes.Add(new AttributeDamage
                {
                    attributeType = attr.attributeType,
                    damageAmount = attr.damageAmount
                });
            }
        }
        SetDamageAttributes(stats);
        Destroy(gameObject, lifetime);
    }

    void SetDamageAttributes(PlayerStats stats)
    {
        AddOrCombineAttribute("FireAttributeDamage", stats.GetStatValue("FireAttributeDamage"), stats);
        AddOrCombineAttribute("WaterAttributeDamage", stats.GetStatValue("WaterAttributeDamage"), stats);
        AddOrCombineAttribute("WindAttributeDamage", stats.GetStatValue("WindAttributeDamage"), stats);
        AddOrCombineAttribute("DarknessAttributeDamage", stats.GetStatValue("DarknessAttributeDamage"), stats);
        AddOrCombineAttribute("LightAttributeDamage", stats.GetStatValue("LightAttributeDamage"), stats);
    }

    void AddOrCombineAttribute(string attributeName, float attributeValue, PlayerStats stats)
    {
        if (attributeValue == 0) return; // Skip if no damage

        AttributeDamage existing = damageAttributes.Find(attr => attr.attributeType.name == attributeName);

        if (existing != null)
        {
            existing.damageAmount += attributeValue;
        }
        else
        {
            StatType statType = stats.stats.GetStatTypeByName(attributeName);
            if (statType != null)
            {
                damageAttributes.Add(new AttributeDamage
                {
                    attributeType = statType,
                    damageAmount = attributeValue
                });
            }
        }
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                float damage = CalculateDamage(enemy.stats, this.damage);
                enemy.TakeDamage(damage);
                Debug.Log("Enemy hit for " + damage + " damage!");
            }
            Destroy(gameObject);
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
    // Damage calculation with attribute interactions
    // Returns final damage after applying attribute multipliers
    float CalculateDamage(EnemyStats stats, float damage)
    {
        float damageAfterMult = damage;
        if (damageAttributes.Count == 0)
        {
            return damage;
        }
        float finalDamage = 0;
        foreach (AttributeDamage attr in damageAttributes)
        {
            if (attr.attributeType.name == "FireAttributeDamage")
            {
                float resistance = stats.fireWeaknessStat;
                float _damage = attr.damageAmount * damage;
                Debug.Log(_damage);
                damageAfterMult = _damage * resistance;
                if (stats.enemyAttribute == EnemyAttribute.Fire)
                {
                    finalDamage += damageAfterMult / 3f;
                    
                } else
                {
                    finalDamage += damageAfterMult;
                }
                damageAfterMult = 0;
            }
            if (attr.attributeType.name == "WaterAttributeDamage")
            {
                float resistance = stats.waterWeaknessStat;
                float _damage = attr.damageAmount * damage;
                damageAfterMult = _damage * resistance;
                if (stats.enemyAttribute == EnemyAttribute.Water)
                {
                    finalDamage += damageAfterMult / 3f;
                }
                else
                {
                    finalDamage += damageAfterMult;
                }
                damageAfterMult = 0;
            }
            if (attr.attributeType.name == "WindAttributeDamage")
            {
                float resistance = stats.windWeaknessStat;
                float _damage = (1 + attr.damageAmount ) *  damage;
                damageAfterMult = _damage * resistance;
                if (stats.enemyAttribute == EnemyAttribute.Wind)
                {
                    finalDamage += damageAfterMult / 3f;
                }
                else
                {
                    finalDamage += damageAfterMult;
                }
                damageAfterMult = 0;
            }
            if (attr.attributeType.name == "DarknessAttributeDamage")
            {
                float resistance = stats.darknessWeaknessStat;
                float _damage = attr.damageAmount * damage;
                damageAfterMult = _damage * resistance;
                if (stats.enemyAttribute == EnemyAttribute.Darkness)
                {
                    finalDamage += damageAfterMult / 3f;
                }
                else
                {
                    finalDamage += damageAfterMult;
                }
                damageAfterMult = 0;

            }
            if (attr.attributeType.name == "LightAttributeDamage")
            {
                float resistance = stats.lightWeaknessStat;
                float _damage = attr.damageAmount * damage;
                damageAfterMult = _damage * resistance;
                if (stats.enemyAttribute == EnemyAttribute.Light)
                {
                    finalDamage += damageAfterMult / 3f;
                } else
                {
                    finalDamage += damageAfterMult;
                }
                damageAfterMult = 0;
            }
        }
        return finalDamage * damageMult;
    }
}
