using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] floorTile;
    public GameObject[] wallTile;
    public GameObject[] obstacleTile;
    public List<Vector3> enemySpawnPoints = new List<Vector3>();
    // public EnemySpawn spawner;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
