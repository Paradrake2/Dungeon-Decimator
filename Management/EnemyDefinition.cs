using UnityEngine;


public enum EnemyType
{
    Melee,
    Ranged,
    Boss,
    MiniBoss
}
public enum EnemyRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Boss
}


[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "Management/EnemyDefinition")]
public class EnemyDefinition : ScriptableObject
{
    public string id;
    public string displayName;
    public EnemyType enemyType;
    public GameObject enemyPrefab;
    public EnemyRarity enemyRarity;

    [Header("Spawn/Balance")]
    public int tier;
    public EnemyAttribute enemyAttribute;
}
