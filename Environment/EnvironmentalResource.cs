using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalResource : MonoBehaviour
{
    public Sprite[] durabilityStages;
    public int health;
    public List<CraftingMaterial> drops;
    public int minDrops, maxDrops;
    public Collider2D triggerCollider;
    void DestroyResource()
    {
        foreach (var item in drops)
        {
            // Add resource to inventory
            int dropCount = Random.Range(minDrops, maxDrops + 1);
            MaterialInventory.Instance.AddMaterial(item, dropCount);
        }
        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        ShowDamagePopup(damage);
        int stageIndex = Mathf.Clamp(durabilityStages.Length - 1 - (health * durabilityStages.Length) / 100, 0, durabilityStages.Length - 1);
        GetComponent<SpriteRenderer>().sprite = durabilityStages[stageIndex];
        if (health <= 0)
        {
            DestroyResource();
        }
    }
    void ShowDamagePopup(int damage)
    {
        if (DungeonUIManager.Instance != null)
        {
            DungeonUIManager.Instance.CreateNumberPopup(damage.ToString(), Color.red, transform.position + Vector3.up * 0.5f, 1f);
        }
    }
    void Start()
    {
        if (triggerCollider == null)
        {
            triggerCollider = GetComponent<Collider2D>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
