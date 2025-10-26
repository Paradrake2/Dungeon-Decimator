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
    float attackCooldown = 1f;
    float lastAttackTime = 0f;

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
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        player = FindFirstObjectByType<Player>().transform;
        obstacleLayer = LayerMask.GetMask("Obstacle");
    }

    void Update()
    {
        if (Time.time - lastPathfindingTime > pathfindingUpdateInterval)
        {
            CheckIfPathfindingNeeded();
            lastPathfindingTime = Time.time;
        }

        Movement();
        if (Vector3.Distance(transform.position, player.position) <= attackRange && Time.time - lastAttackTime >= attackCooldown && !isAttacking)
        {
            Attack();
            lastAttackTime = Time.time;
            isAttacking = true;
            StartCoroutine(AttackCooldownCoroutine());
        }
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
        PathRequestManager.RequestPath(transform.position, player.position, OnPathFound);
        Debug.Log("Path requested");
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
    public abstract void Movement();
    public abstract IEnumerator Attack();
}
