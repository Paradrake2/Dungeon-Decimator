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
        animator.SetTrigger("isAttacking");
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

        // Phase 5: Cooldown phase
        Debug.Log("Slime: Starting cooldown");
        yield return new WaitForSeconds(cooldownTime);

        // Phase 6: Ready for next attack
        Debug.Log("Slime: Attack sequence complete");
        animator.SetTrigger("attackOver");
        StartCoroutine(AttackCooldownCoroutine());
    }
    void SlimeMovement()
    {
        if (isAttacking)
        {
            return;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Slime: Player in range, starting attack!");
            StartCoroutine(Attack());
            return;
        }
        

        if (distanceToPlayer > stoppingDistance)
        {
            if (usePathfinding && currentPath != null && currentPath.Length > 0)
            {
                FollowPath();
            }
            else
            {
                MoveDirectlyTowardsPlayer();
            }
            
            animator.SetBool("isMoving", true);
            animator.SetBool("isIdle", false);
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isIdle", true);
        }
    }
    public override void Movement()
    {
        SlimeMovement();
    }

}
