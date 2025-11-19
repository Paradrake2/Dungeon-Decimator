using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public abstract class GenerationAlgorithm : ScriptableObject
{
    public static GenerationAlgorithm Instance;
    public abstract void GenerateMap(RoomData roomData);
    protected Vector2Int startPos = new Vector2Int(0, 0);
    public HashSet<Vector2Int> floorPosTracker = new HashSet<Vector2Int>();
    public static Tilemap floorMap;
    public static Tilemap wallMap;
    public void GetMaps()
    {
        floorMap = FindObjectsByType<Tilemap>(FindObjectsSortMode.None).Where(t => t.gameObject.name == "FloorTilemap").FirstOrDefault();
        wallMap = FindObjectsByType<Tilemap>(FindObjectsSortMode.None).Where(t => t.gameObject.name == "WallTilemap").FirstOrDefault();
    }
    public void ClearMaps()
    {
        floorMap.ClearAllTiles();
        wallMap.ClearAllTiles();
    }
    
}
