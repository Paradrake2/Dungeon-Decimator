using System.Collections;
using UnityEngine;

public class BasicSpiderAI : EnemyAI
{
    public GameObject attackPrefab;
    public float windupTime = 0.3f;
    public float cooldownTime = 1f;
    [Header("Attack Prefab Settings")]
    public float angleOffset;
    public float attackSpawnOffset;

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
    Vector3 GetPositionOffset(Vector3 direction)
    {
        string dir = GetFacingDirection(direction);
        if (dir == "facingRight")
        {
            return transform.position + new Vector3(attackSpawnOffset, 0, 0);
        }
        else if (dir == "facingLeft")
        {
            return transform.position + new Vector3(-attackSpawnOffset, 0, 0);
        }
        else if (dir == "facingUp")
        {
            return transform.position + new Vector3(0, attackSpawnOffset, 0);
        }
        else // facingDown
        {
            return transform.position + new Vector3(0, -attackSpawnOffset, 0);
        }

    }

    Quaternion GetRotationOffset(Vector3 direction)
    {
        string dir = GetFacingDirection(direction);
        if (dir == "facingRight")
        {
            return Quaternion.Euler(0, 0, 0 + angleOffset);
        }
        else if (dir == "facingLeft")
        {
            return Quaternion.Euler(0, 0, 180 + angleOffset);
        }
        else if (dir == "facingUp")
        {
            return Quaternion.Euler(0, 0, 90 + angleOffset);
        }
        else // facingDown
        {
            return Quaternion.Euler(0, 0, 270 + angleOffset);
        }

    }
    public override void Movement()
    {
        if (isAttacking)
        {
            animator.SetBool("isWalking", false);
            return;
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
        AnimationDirectionController(player);
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            BaseMovement();
        } else if (!isAttacking)
        {
            animator.ResetTrigger("idleOver");
            StartCoroutine(Attack());
        }
    }

    void Start()
    {
        animator.SetBool("facingDown", true);
        player = FindFirstObjectByType<Player>().transform;
    }


    void AnimationDirectionController(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        string facingDirection = GetFacingDirection(direction);
        if (facingDirection == "facingUp")
        {
            animator.SetBool("facingUp", true);
            animator.SetBool("facingDown", false);
            animator.SetBool("facingLeft", false);
            animator.SetBool("facingRight", false);
        }
        else if (facingDirection == "facingDown")
        {
            animator.SetBool("facingDown", true);
            animator.SetBool("facingUp", false);
            animator.SetBool("facingLeft", false);
            animator.SetBool("facingRight", false);
        }
        else if (facingDirection == "facingLeft")
        {
            animator.SetBool("facingLeft", true);
            animator.SetBool("facingUp", false);
            animator.SetBool("facingDown", false);
            animator.SetBool("facingRight", false);
        }
        else if (facingDirection == "facingRight")
        {
            animator.SetBool("facingRight", true);
            animator.SetBool("facingUp", false);
            animator.SetBool("facingDown", false);
            animator.SetBool("facingLeft", false);
        }
    }
    
    string GetFacingDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? "facingRight" : "facingLeft";
        }
        else
        {
            return direction.y > 0 ? "facingUp" : "facingDown";
        }
    }
}
