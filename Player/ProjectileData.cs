using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public GameObject projectilePrefab;
    public float speed;
    public float damage;
    public float lifetime;
    public StatType[] damageAttributes;
    public float size = 1;
}
