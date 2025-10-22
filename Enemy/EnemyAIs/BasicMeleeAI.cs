using UnityEngine;

public class BasicMeleeAI : EnemyAI
{
    public Transform player;
    public EnemyStats stats;
    public override void Attack()
    {
        // Implement melee attack logic here
    }

    public override void Movement()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * stats.baseMovementSpeed * Time.deltaTime;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>().transform;
        stats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
}
