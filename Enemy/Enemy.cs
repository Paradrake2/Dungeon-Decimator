using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats stats;
    public EnemyAI ai;

    public List<CraftingMaterial> lootTable;

    public void TakeDamage(float damage)
    {
        // Implement damage logic here
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
