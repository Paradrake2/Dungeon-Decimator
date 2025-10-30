using System.Collections;
using UnityEngine;



public class Combat : MonoBehaviour
{
    Vector3 mousePos;
    public Transform player;
    public PlayerStats stats;
    public PlayerModifiers playerModifiers;
    public KeyCode attackKey = KeyCode.Mouse0;
    public float attackCooldown = 1f; // placeholder value, will be set by player's attack speed stat
    private Vector3 attackDirection;
    public GameObject projectilePrefab; // what the player shoots
    void Start()
    {
        player = transform;
        Initialize();
        StartCoroutine(AttackCoroutine());
    }
    void Initialize()
    {
        stats = FindFirstObjectByType<PlayerStats>();
        attackCooldown = stats.GetAttackSpeed();
    }
    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Set the z position to 0
        attackDirection = (mousePos - player.position).normalized;
        // direction will be used for aiming
    }

    public IEnumerator AttackCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(attackKey))
            {
                // Perform attack logic here
                FireProjectile();
                // Wait for the cooldown duration before allowing the next attack
                yield return new WaitForSeconds(attackCooldown);
            }
            else
            {
                yield return null; // Wait until the next frame
            }
        }
    }

    void FireProjectile()
    {
        int quantity = playerModifiers.GetQuantity();

        if (quantity == 1)
        {
            // Single projectile - fire straight
            GameObject projectile = Instantiate(projectilePrefab, player.position, Quaternion.identity);
            PlayerProjectile projScript = projectile.GetComponent<PlayerProjectile>();
            projScript.Initialize(attackDirection, stats, playerModifiers);
        }
        else
        {
            // Multiple projectiles - fire in a cone
            FireProjectileCone(quantity);
        }
    }

    void FireProjectileCone(int quantity)
    {
        float coneAngle = 30f; // Total spread angle in degrees (adjustable)

        if (quantity == 2)
        {
            // Special case for 2 projectiles - symmetric spread
            float halfAngle = coneAngle / 4f; // Smaller spread for just 2

            FireSingleProjectile(attackDirection, -halfAngle);
            FireSingleProjectile(attackDirection, halfAngle);
        }
        else
        {
            // 3 or more projectiles
            float angleStep = coneAngle / (quantity - 1);
            float startAngle = -coneAngle / 2f;

            for (int i = 0; i < quantity; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                FireSingleProjectile(attackDirection, currentAngle);
            }
        }
    }

    void FireSingleProjectile(Vector3 baseDirection, float angleOffset)
    {
        // Rotate the base direction by the angle offset
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, angleOffset) * baseDirection;

        GameObject projectile = Instantiate(projectilePrefab, player.position, Quaternion.identity);
        PlayerProjectile projScript = projectile.GetComponent<PlayerProjectile>();
        projScript.Initialize(rotatedDirection, stats, playerModifiers);
    }
}
