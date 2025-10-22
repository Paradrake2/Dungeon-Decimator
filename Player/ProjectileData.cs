using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AttributeDamage
{
    public StatType attributeType;
    public float damageAmount; // percent modifier
}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public GameObject projectilePrefab;
    public float speed;
    public float damage;
    public float lifetime;
    public List<AttributeDamage> damageAttributes;
    public float size = 1;
}
