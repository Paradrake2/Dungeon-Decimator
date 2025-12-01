using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    public EnemyStats stats;
    public Transform player;
    public LayerMask obstacleLayer;
    public float pathfindingUpdateInterval = 0.5f;
    public float stoppingDistance = 0f;

    public float attackRange = 0f;
    [SerializeField] float attackCooldown = 1f;
    public float attackSpawnOffset;
    public float angleOffset;

    //float lastAttackTime = 0f;

    [Header("Debug")]
    public bool drawPath = false;

    public bool usePathfinding = false;
    public Vector3[] currentPath;
    public int currentPathIndex;
    public float lastPathfindingTime = 0f;
    public bool isAttacking = false;

    [Header("Anti-Oscillation")]
    public float directionChangeDelay = 0.2f;
    private Vector3 lastMoveDirection;
    private float lastDirectionChangeTime;

    [Header("Animation")]
    public Animator animator;
    [SerializeField] private float enemyRadius;
    public virtual void Start()
    {
        animator.SetBool("facingDown", true);
        BaseStart();
    }
    public void BaseStart()
    {
        stats = GetComponent<EnemyStats>();
        player = FindFirstObjectByType<Player>().transform;
        obstacleLayer = LayerMask.GetMask("Obstacle");
        var collider = this.gameObject.GetComponent<Collider2D>();
        if (collider != null)
        {
            Debug.LogWarning("CALLED");
            enemyRadius = collider.bounds.size.magnitude / 2f;
        }
    }
    public virtual void Update()
    {
        if (Time.time - lastPathfindingTime > pathfindingUpdateInterval)
        {
            CheckIfPathfindingNeeded();
            lastPathfindingTime = Time.time;
        }

        Movement();
    }
    public IEnumerator AttackCooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
    void CheckIfPathfindingNeeded()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized, Vector3.Distance(transform.position, player.position), obstacleLayer);
        usePathfinding = hit.collider != null;
        if (usePathfinding)
        {
            RequestNewPath();
        }
    }
    public void MoveDirectlyTowardsPlayer()
    {
        Vector3 newDirection = (player.position - transform.position).normalized;
    
        if (Time.time - lastDirectionChangeTime > directionChangeDelay || Vector3.Dot(newDirection, lastMoveDirection) < 0.5f) // 60 degree change threshold
        {
            lastMoveDirection = newDirection;
            lastDirectionChangeTime = Time.time;
        }
    
        transform.position += lastMoveDirection * stats.baseMovementSpeed * Time.deltaTime;
    }

    public void RequestNewPath()
    {
        PathRequestManager.RequestPath(transform.position, player.position, OnPathFound, enemyRadius);
    }
    void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            currentPath = newPath;
            currentPathIndex = 0;
        }
        else
        {
            // pathfinding failed, fall back to direct movement
            currentPath = null;
            usePathfinding = false;
        }
    }
    public void FollowPath()
    {
        if (currentPath == null || currentPathIndex >= currentPath.Length) return;

        Vector3 targetPosition = currentPath[currentPathIndex];
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * stats.baseMovementSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPathIndex++;
            if (currentPathIndex >= currentPath.Length)
            {
                CheckIfPathfindingNeeded();
            }
        }

        if (drawPath)
        {
            for (int i = currentPathIndex; i < currentPath.Length - 1; i++)
            {
                Debug.DrawLine(currentPath[i], currentPath[i + 1], Color.red);
            }
        }
    }
    public void BaseMovement()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= stoppingDistance || isAttacking || distanceToPlayer <= attackRange) return;

        if (usePathfinding && currentPath != null && currentPath.Length > 0)
        {
            FollowPath();
        } else
        {
            MoveDirectlyTowardsPlayer();
        }
    }
    public Quaternion GetRotationOffset(Vector3 direction)
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
    public Vector3 GetPositionOffset(Vector3 direction)
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
    public virtual void AnimationDirectionController(Transform target)
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
    public virtual void Movement()
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
    public virtual IEnumerator Attack()
    {
        yield return null;
    }
}
