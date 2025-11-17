using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Management/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyDefinition> allEnemies;
}
