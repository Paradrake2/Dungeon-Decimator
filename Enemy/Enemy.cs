using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootDrop
{
    public CraftingMaterial material;
    public float dropChance; // 0 to 1
    public int minAmount;
    public int maxAmount;
}

[System.Serializable]
public class RewardTable {
    public CraftingMaterial material;
    public int amount;
}

public class Enemy : MonoBehaviour
{
    public EnemyStats stats;
    public EnemyAI ai;

    public List<LootDrop> lootTable;

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - stats.baseDefense;
        if (finalDamage < 0) finalDamage = 0;
        stats.currentHealth -= finalDamage;
    }

    public void Die()
    {
        List<RewardTable> droppedLoot = LootDrop();
        foreach (var loot in droppedLoot)
        {
            Debug.Log($"Dropped {loot.amount} x {loot.material.materialName}");
        }
        Destroy(gameObject);
    }

    List<RewardTable> LootDrop()
    {
        List<RewardTable> droppedLoot = new List<RewardTable>();
        foreach (var lootDrop in lootTable)
        {
            float dropChance = lootDrop.dropChance;
            if (Random.value <= dropChance)
            {
                int _amount = Random.Range(lootDrop.minAmount, lootDrop.maxAmount);
                droppedLoot.Add(new RewardTable { material = lootDrop.material, amount = _amount });
            }
        }
        return droppedLoot;
    }
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        ai = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
