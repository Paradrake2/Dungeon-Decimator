using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    public PlayerStats stats;
    public float playerHealth;
    public void TakeDamage(float damage, AttributeDamage[] attributeDamages)
    {
        float finalDamage;
        float damageMultiplier = 1;
        foreach (var attrDamage in attributeDamages)
        {
            if (attrDamage.damageAmount != 0)
            {
                StatType attributeType = attrDamage.attributeType;
                float attackValue = attrDamage.damageAmount;
                foreach (var resistance in stats.attributeResistances)
                {
                    if (resistance.attributeAttack == attributeType)
                    {
                        damageMultiplier += Mathf.Max(0, attackValue - resistance.resistanceValue);
                        Debug.Log("Attribute: " + attributeType.name + " Attack Value: " + attackValue + " Resistance Value: " + resistance.resistanceValue + " Damage Multiplier: " + damageMultiplier);
                    }
                }
            }

            
        }
        finalDamage = (damage * damageMultiplier) - stats.GetStatValue("Defense");
        Debug.Log("Final damage: " + finalDamage + " Initial damage: " + damage + " Damage Multiplier: " + damageMultiplier);
        UpdateHealth();
        playerHealth -= finalDamage;
        SetHealth(playerHealth);
        if (playerHealth <= 0)
        {
            Die();
        }
        // Implement damage logic here
    }
    public void Die()
    {
        // Implement death logic here
    }

    public void UpdateHealth()
    {
        playerHealth = stats.GetStatValue("Health");
        // Update any UI elements or other game logic related to health here
    }
    public void SetHealth(float health)
    {
        stats.SetStatValue("Health", health);
        UpdateHealth();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
