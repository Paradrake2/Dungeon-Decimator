using UnityEngine;

public class BlueSpiderBiteAttack : EnemyProjectile
{
    public override void Animation()
    {

    }
    public override void Initialize(EnemyStats stats, EnemyProjectileData data, Vector3 direction)
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
    void Start()
    {
        
    }
}
