using UnityEngine;

public class PlayerModifiers : MonoBehaviour
{
    // these are modified during the run and reset at the start of each new run
    public int quantity = 1;
    public float speedModifier = 1;
    public float damageModifier = 1;
    public float healthModifier = 1;
    public float defenseModifier = 1;
    public float projSizeModifier = 1;
    public float attackSpeedModifier = 0;
    

    public void ResetModifiers() // called at start of new run
    {
        quantity = 1;
        speedModifier = 1;
        damageModifier = 1;
        healthModifier = 1;
        defenseModifier = 1;
        projSizeModifier = 1;
        attackSpeedModifier = 0;
    }
    public void SetQuantity(int qty)
    {
        quantity = quantity + qty;
    }
    public void SetSpeedModifier(float speedMod)
    {
        speedModifier = speedModifier + speedMod;
    }
    public void SetDamageModifier(float damageMod)
    {
        damageModifier = damageModifier + damageMod;
    }
    public void SetProjSizeModifier(float sizeMod)
    {
        projSizeModifier = projSizeModifier + sizeMod;
    }
    public void SetHealthModifier(float healthMod)
    {
        healthModifier = healthModifier + healthMod;
    }
    public void SetDefenseModifier(float defMod)
    {
        defenseModifier = defenseModifier + defMod;
    }
    public void SetAttackSpeedModifier(float attackSpeedMod)
    {
        attackSpeedModifier = attackSpeedModifier + attackSpeedMod;
    }

    public int GetQuantity()
    {
        return quantity;
    }
    public float GetSpeedModifier()
    {
        return speedModifier;
    }
    public float GetDamageModifier()
    {
        return damageModifier;
    }
    public float GetProjSizeModifier()
    {
        return projSizeModifier;
    }
    public float GetHealthModifier()
    {
        return healthModifier;
    }
    public float GetDefenseModifier()
    {
        return defenseModifier;
    }
    public float GetAttackSpeedModifier()
    {
        return attackSpeedModifier;
    }
}
