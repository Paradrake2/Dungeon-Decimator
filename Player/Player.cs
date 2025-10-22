using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStats stats;
    public float playerHealth;
    public void TakeDamage(float damage, AttributeDamage[] attributeDamages)
    {
        UpdateHealth();
        playerHealth -= damage;
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
        playerHealth = stats.stats.GetStat(stats.GetStatValue("Health").ToString());
        // Update any UI elements or other game logic related to health here
    }

    public void GetAttributeResistances()
    {
        // Implement logic to get attribute resistances from stats
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
