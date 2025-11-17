using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawnHolder
{
    public List<GameObject> enemyPrefabs;
}

[System.Serializable]
public class RoomContents
{
    public List<GameObject> potentialResources;
    public List<GameObject> potentialObstacles;
    public List<GameObject> potentialTraps;
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
    public RoomAttribute roomAttribute;
    public RoomBiome roomBiome;
    public GenerationAlgorithm generationAlgorithm;
    public int width = 1;
    public int height = 1;
    public int enemyCapacity;
    public float obstacleDensity;

    public abstract void GenerateRoom();
}
