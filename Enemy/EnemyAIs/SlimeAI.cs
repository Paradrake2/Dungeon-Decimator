using System.Collections;
using UnityEngine;

public class SlimeAI : EnemyAI
{
    public float windupTime = 1f;
    public float cooldownTime = 0.5f;
    public GameObject attackPrefab;
    public Vector3 attackSpawnLocationOffset;
    public override IEnumerator Attack()
    {
        // Phase 1: Start attack sequence
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        animator.SetBool("isMoving", false);
        animator.SetBool("isIdle", false);

        Debug.Log("Slime: Starting attack windup");

        // Phase 2: Windup phase
        yield return new WaitForSeconds(windupTime);

        // Phase 3: Fire the shockwave
        Debug.Log("Slime: Firing shockwave");
        EnemyProjectileData data = attackPrefab.GetComponent<EnemyProjectile>().projectileData;
        Vector3 direction = Vector2.zero;
        GameObject projectileInstance = Instantiate(attackPrefab, transform.position + attackSpawnLocationOffset, Quaternion.identity);
        projectileInstance.GetComponent<EnemyProjectile>().Initialize(stats, data, direction);

        // Phase 4: End attack animation
        animator.SetBool("isAttacking", false);

        // Phase 5: Cooldown phase
        Debug.Log("Slime: Starting cooldown");
        yield return new WaitForSeconds(cooldownTime);

        // Phase 6: Ready for next attack
        Debug.Log("Slime: Attack sequence complete");
        StartCoroutine(AttackCooldownCoroutine());
    }
    void SlimeMovement()
    {
        // Don't do any movement or animation changes while attacking
        if (isAttacking)
        {
            return; // Let Attack() coroutine handle all animation states
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Check if should attack (when close to player AND not already attacking)
        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Slime: Player in range, starting attack!");
            StartCoroutine(Attack());
            return; // Exit immediately - Attack() coroutine handles all animation states
        }
        

        // Normal movement and animation when not attacking
        if (distanceToPlayer > stoppingDistance)
        {
            // Moving toward player
            if (usePathfinding && currentPath != null && currentPath.Length > 0)
            {
                FollowPath();
            }
            else
            {
                MoveDirectlyTowardsPlayer();
            }
            
            // Set movement animation
            animator.SetBool("isMoving", true);
            animator.SetBool("isIdle", false);
        }
        else
        {
            // Close to player but not in attack range - idle
            animator.SetBool("isMoving", false);
            animator.SetBool("isIdle", true);
        }
    }
    public override void Movement()
    {
        SlimeMovement();
    }

}
