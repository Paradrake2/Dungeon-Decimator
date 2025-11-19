using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawnHolder
{
    public List<GameObject> enemyPrefabs;
}
[System.Serializable]
public class ResourceWeights
{
    public GameObject resourcePrefab;
    public int weight = 1;
}
[System.Serializable]
public class RoomContents
{
    public List<ResourceWeights> potentialResources;
    public List<GameObject> potentialObstacles;
    public List<GameObject> potentialTraps;
    public PotentialEnemiesHolder potentialEnemies;
}
[System.Serializable]
public class EnemyRarityWeight
{
    public EnemyRarity enemyRarity;
    public int weight = 1;
}
[System.Serializable]
public class TileContentWeights
{
    public TileContentType tileContentType;
    public int weight = 1;
}

public enum RoomBiome
{
    None,
    Forest,
    Cave,
    Volcano,
    Water,

}

public enum RoomAttribute
{
    None,
    Normal,
    Fire,
    Water,
    Wind,
    Light,
    Dark
}

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/RoomData")]
public abstract class RoomData : ScriptableObject
{
    public string roomName;
    public string ID;
    public int tier;
    public RoomEnvironment roomEnv;
    public RoomContents roomContents;
    public EnemyRarityWeight[] enemyRarityWeights;
    public TileContentWeights[] tileContentWeights;
    public RoomAttribute roomAttribute;
    public RoomBiome roomBiome;
    public GenerationAlgorithm generationAlgorithm;
    public int width = 1;
    public int height = 1;
    public int enemyCapacity;
    public float obstacleDensity;
    public bool useRuleTile = true;
    public NodeGrid nodeGridPrefab;
    public abstract void GenerateRoom();
    public int GetEnemyRarityWeight(EnemyRarity rarity)
    {
        foreach (var erw in enemyRarityWeights)
        {
            if (erw.enemyRarity == rarity)
            {
                return erw.weight;
            }
        }
        return 0;
    }
    public int GetTileContentWeight(TileContentType tileContentType)
    {
        foreach (var tcw in tileContentWeights)
        {
            if (tcw.tileContentType == tileContentType)
            {
                return tcw.weight;
            }
        }
        return 0;
    }
    public List<ResourceWeights> GetPotentialResources()
    {
        return roomContents.potentialResources;
    }
    public List<GameObject> GetPotentialObstacles()
    {
        return roomContents.potentialObstacles;
    }
    public List<GameObject> GetPotentialTraps()
    {
        return roomContents.potentialTraps;
    }
    public PotentialEnemiesHolder GetPotentialEnemies()
    {
        return roomContents.potentialEnemies;
    }
    public void SetNodeGrid()
    {
        nodeGridPrefab = FindAnyObjectByType<NodeGrid>();
    }
}