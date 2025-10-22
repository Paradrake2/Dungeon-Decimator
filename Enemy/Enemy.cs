using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats stats;
    public EnemyAI ai;

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        ai = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
