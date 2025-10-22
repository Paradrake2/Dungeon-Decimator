using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats stats;
    public EnemyAI ai;

    public List<CraftingMaterial> lootTable;

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - stats.baseDefense;
        if (finalDamage < 0) finalDamage = 0;
        stats.currentHealth -= finalDamage;
    }

    public void Die()
    {
        // Implement death logic here
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
