using System.Collections;
using UnityEngine;

public class AttributeDamage
{
    public StatType attributeType;
    public float damageAmount;
}

public class Combat : MonoBehaviour
{
    Vector3 mousePos;
    public Transform player;
    public PlayerStats stats;
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
                Debug.Log("Player attacked!");

                // Wait for the cooldown duration before allowing the next attack
                yield return new WaitForSeconds(attackCooldown);
            }
            else
            {
                yield return null; // Wait until the next frame
            }
        }
    }
}
