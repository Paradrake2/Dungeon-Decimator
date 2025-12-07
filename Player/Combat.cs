using System.Collections;
using UnityEngine;



public class Combat : MonoBehaviour
{
    Vector3 mousePos;
    public Transform player;
    public PlayerStats stats;
    public PlayerModifiers playerModifiers;
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode secondaryAttackKey = KeyCode.Mouse1;
    public float attackCooldown = 1f; // placeholder value, will be set by player's attack speed stat
    private Vector3 attackDirection;
    public GameObject projectilePrefab; // what the player shoots
    public GameObject playerSlash;
    public Equipment selectedEquipment;
    public bool canAttack = true;
    public int selectedSlotIndex;
    public Equipment basicEquipment; // fallback equipment if none selected
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
    public void ChangeSelectedSlotIndex(int index)
    {
        selectedSlotIndex = index;
        if (Hotbar.Instance != null)
        {
            Hotbar.Instance.selectedIndex = index;
            selectedEquipment = Hotbar.Instance.GetHeldItem();
        }
    }
    public void StartAttackRoutine()
    {
        StartCoroutine(AttackCoroutine());
    }
    public IEnumerator AttackCoroutine()
    {
        while (canAttack)
        {
            if (Input.GetKeyDown(attackKey) && stats.canAttack)
            {
                if (selectedEquipment != null)
                {
                    FireProjectile();
                    // Wait for the cooldown duration before allowing the next attack
                    yield return new WaitForSeconds(attackCooldown);
                }
                else
                {
                    yield return null; // No equipment selected, wait until the next frame
                }
            }
            else
            {
                yield return null; // Wait until the next frame
            }
            /**
            if (Input.GetKeyDown(secondaryAttackKey) && secondaryAttackCooldown + lastSecondaryAttackTime <= Time.time)
            {
                // Perform secondary attack logic here
                SpawnSlash();
                lastSecondaryAttackTime = Time.time;
                // Wait for the cooldown duration before allowing the next attack
                yield return new WaitForSeconds(secondaryAttackCooldown);
            }
            **/
        }
    }
    void FireProjectile(Vector3 baseDirection = default, float angleOffset = 0f)
    {
        selectedEquipment = Hotbar.Instance.GetHeldItem();
        switch (selectedEquipment.equipmentType)
        {
            case EquipmentType.Melee:
                // Melee attack logic (e.g., spawn a slash effect)
                MeleeAttack();
                break;
            case EquipmentType.Ranged:
                RangedAttack(baseDirection, angleOffset);
                break;
            default:
                Debug.LogWarning("Selected equipment is neither Melee nor Ranged.");
                return;
        }
        if (selectedEquipment.equipmentType == EquipmentType.Ranged)
        {
            
        }
    }
    void RangedAttack(Vector3 baseDirection = default, float angleOffset = 0f)
    {
        int quantity = playerModifiers.GetQuantity();
        if (quantity == 1)
        {
            // Single projectile - fire straight
            GameObject projectile = Instantiate(projectilePrefab, player.position, Quaternion.identity);
            PlayerProjectile projScript = projectile.GetComponent<PlayerProjectile>();
            projScript.Initialize(baseDirection == default ? attackDirection : baseDirection, stats, playerModifiers);
        }
        else
        {
            // Multiple projectiles - fire in a cone
            FireProjectileCone(quantity);
        }
    }
    void MeleeAttack()
    {
        GameObject slash = Instantiate(playerSlash, player.position, Quaternion.identity);
        PlayerSlash slashScript = slash.GetComponent<PlayerSlash>();
        slashScript.Initialize(attackDirection, stats, playerModifiers);
    }
    void FireProjectileCone(int quantity)
    {
        float coneAngle = 30f; // Total spread angle in degrees (adjustable)

        if (quantity == 2)
        {
            // Special case for 2 projectiles - symmetric spread
            float halfAngle = coneAngle / 4f; // Smaller spread for just 2

            FireProjectile(attackDirection, -halfAngle);
            FireProjectile(attackDirection, halfAngle);
        }
        else
        {
            // 3 or more projectiles
            float angleStep = coneAngle / (quantity - 1);
            float startAngle = -coneAngle / 2f;

            for (int i = 0; i < quantity; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                FireProjectile(attackDirection, currentAngle);
            }
        }
    }
    // Deprecated, kept for reference
    /**
    void FireSingleProjectile(Vector3 baseDirection, float angleOffset)
    {
        // Rotate the base direction by the angle offset
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, angleOffset) * baseDirection;

        GameObject projectile = Instantiate(projectilePrefab, player.position, Quaternion.identity);
        PlayerProjectile projScript = projectile.GetComponent<PlayerProjectile>();
        projScript.Initialize(rotatedDirection, stats, playerModifiers);
    }
    **/
}
