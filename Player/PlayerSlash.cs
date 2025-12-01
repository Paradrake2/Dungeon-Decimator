using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    public Collider2D slashCollider;
    public float slashDuration = 0.3f;
    public float offset;
    public Animator animator;
    private float damage;
    public void Initialize(Vector3 direction, PlayerStats stats, PlayerModifiers modifiers = null)
    {
        Vector3 rotatedDirection = Quaternion.Euler(0,0, -90) * direction;
        transform.up = rotatedDirection;
        transform.position += direction * offset;
        damage = stats.GetStatValue("Damage") + modifiers.damageModifier;
        animator.SetTrigger("Slash");
        Destroy(gameObject, slashDuration);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
