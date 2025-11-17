using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneratedRoom
{
    public Vector2Int gridPos;
    public RoomData roomData;
}

[System.Serializable]
public class RoomChances
{
    public float treasureChance;
    public float eliteChance;
    public float trapChance;
    public float oreChance;
    public float enemyChance;
}

public class RoomGenerator : MonoBehaviour
{
    // public EnemySpawn spawner;
    public List<RoomData> roomDatas = new List<RoomData>();
    public bool isBossLevel = true;
    public RoomChances roomChances;
    public GameObject testObject;
    private List<RoomData> dungeonRooms = new List<RoomData>();
    private RoomData currentRoom;
    public Tilemap map;
    public GameObject playerObject;

    public void SetIsBoss(bool isBoss)
    {
        isBossLevel = isBoss;
    }

    public void LoadDungeonRooms() // Called when first entering the dungeon
    {
        foreach (var room in Resources.LoadAll<RoomData>("Rooms/Rooms"))
        {
            //if (room.tier <= playerStats.GetStatValue("Level"))
            dungeonRooms.Add(room);
        }
    }
    public void LoadDungeon()
    {
        currentRoom = null;
        map.ClearAllTiles();
        if (dungeonRooms.Count > 0)
        {
            int randomIndex = Random.Range(0, dungeonRooms.Count);
            RoomData selectedRoom = dungeonRooms[randomIndex];
            selectedRoom.GenerateRoom();
            currentRoom = selectedRoom;
        }
        else
        {
            Debug.LogWarning("No rooms available for the player's level.");
        }
    }
    public void ResetDungeon() // Called on death/exiting the dungeon
    {
        dungeonRooms.Clear();
        currentRoom = null;
    }
    public void Start()
    {
        LoadDungeonRooms();
        LoadDungeon();
    }
}
