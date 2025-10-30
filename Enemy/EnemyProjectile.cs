using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyProjectile : MonoBehaviour
{
    public GameObject prefab;
    public EnemyProjectileData projectileData; // holds damage, speed, size, lifetime, animator
    public Animator animator = null;
    public bool destroyOnImpact = true;
    public float damage;
    public List<AttributeDamage> attributeDamages = new List<AttributeDamage>();
    public virtual void Initialize(EnemyStats stats, EnemyProjectileData data, Vector3 direction)
    {
        damage = projectileData.damage + stats.baseDamage;
        float speed = data.speed;
        float lifetime = data.lifetime;
        float size = data.size;
        if (data.animator != null)
        {
            animator = data.animator;
            Animation();
        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(size, size, size);
        if (speed != 0) rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }
    public abstract void Animation();
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy Projectile hit Player");
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(projectileData.damage, attributeDamages.ToArray());
            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
    }
}
