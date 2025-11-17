using UnityEngine;


public enum EnemyType
{
    Melee,
    Ranged,
    Boss,
    MiniBoss
}


[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "Management/EnemyDefinition")]
public class EnemyDefinition : ScriptableObject
{
    public string id;
    public string displayName;
    public EnemyType enemyType;
    public GameObject enemyPrefab;

    [Header("Spawn/Balance")]
    public int tier;
    public int threatCost;
    public float baseWeight;
    public EnemyAttribute enemyAttribute;
}
