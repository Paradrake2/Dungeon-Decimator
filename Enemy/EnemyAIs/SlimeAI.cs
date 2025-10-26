using System.Collections;
using UnityEngine;

public class SlimeAI : EnemyAI
{
    public float windupTime = 1f;
    public float cooldownTime = 0.5f;
    public GameObject attackPrefab;
    public override IEnumerator Attack()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(windupTime); // Attack wind-up time
        EnemyProjectileData data = attackPrefab.GetComponent<EnemyProjectile>().projectileData;
        Vector3 direction = Vector2.zero;
        GameObject projectileInstance = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        projectileInstance.GetComponent<EnemyProjectile>().Initialize(stats, data, direction);

        animator.SetBool("isAttacking", false);
        animator.SetBool("isIdle", true);
        yield return new WaitForSeconds(cooldownTime);
        isAttacking = false;
    }
    void SlimeMovement()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isIdle", false);
            animator.SetBool("isMoving", false);
            StartCoroutine(Attack());
        }

        if (usePathfinding && currentPath != null && currentPath.Length > 0)
        {
            FollowPath();
            animator.SetBool("isMoving", true);
        }
        else
        {
            MoveDirectlyTowardsPlayer();
            animator.SetBool("isMoving", true);
        }
    }
    public override void Movement()
    {
        SlimeMovement();
    }
}
