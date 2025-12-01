using System.Collections;
using UnityEngine;

public class BasicGoblinAI : EnemyAI
{
    public GameObject attackPrefab;
    public float windupTime = 0.3f;
    public float cooldownTime = 1f;
    private Vector3 lockedAttackDirection;
    public override IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        lockedAttackDirection = (transform.position - player.position).normalized;
        yield return new WaitForSeconds(windupTime);
        HandleAttack();
        animator.SetTrigger("attackDone");
        yield return new WaitForSeconds(cooldownTime);
        animator.ResetTrigger("attackDone");
        animator.SetTrigger("idleOver");
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }
    void HandleAttack()
    {
        EnemyProjectileData data = attackPrefab.GetComponent<EnemyProjectile>().projectileData;
        Quaternion rotation = GetRotationOffset(lockedAttackDirection);
        GameObject attack = Instantiate(attackPrefab, GetPositionOffset(lockedAttackDirection), rotation);

        attack.GetComponent<EnemyProjectile>().Initialize(stats, data, lockedAttackDirection);
    }
}
