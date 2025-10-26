using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProjectileData", menuName = "Scriptable Objects/EnemyProjectileData")]
public class EnemyProjectileData : ScriptableObject
{
    public bool isHoming = false;
    public float speed = 10f;
    public float damage = 10f;
    public float lifetime = 5f;
    public float size = 1f;
    public Animator animator = null;
}
