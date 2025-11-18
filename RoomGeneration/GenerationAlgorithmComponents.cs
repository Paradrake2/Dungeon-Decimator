using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class WallVectorDirections
{
    public static Dictionary<string, Vector2Int[]> directions = new Dictionary<string, Vector2Int[]>()
    {
        {"up", new Vector2Int[] { Vector2Int.up }},
        {"down", new Vector2Int[] { Vector2Int.down }},
        {"left", new Vector2Int[] { Vector2Int.left }},
        {"right", new Vector2Int[] { Vector2Int.right }},
        {"upLeft", new Vector2Int[] { Vector2Int.up, Vector2Int.left }},
        {"upRight", new Vector2Int[] { Vector2Int.up, Vector2Int.right }},
        {"downLeft", new Vector2Int[] { Vector2Int.down, Vector2Int.left }},
        {"downRight", new Vector2Int[] { Vector2Int.down, Vector2Int.right }},
    };
}



// This big class contains static helper methods for generation algorithms
public static class GenerationAlgorithmComponents
{
    public static bool test = false;
    public static Vector2Int GetRandomDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0: return Vector2Int.up;
            case 1: return Vector2Int.right;
            case 2: return Vector2Int.down;
            default: return Vector2Int.left;
        }
    }
    public static bool IsEdge()
    {
        return true;
    }
    public static void GenerateContents(RoomData rd, HashSet<Vector2Int> floorPositions)
    {
        
    }
    public static void GenerateWalls(HashSet<Vector2Int> floorPositions, RoomData rd, Tilemap map)
    {
        HashSet<Vector2Int> basicWallPos = FindWallsInDirections(floorPositions);
        if (test)
        {
            foreach (var pos in basicWallPos)
            {
                bool floorUp = floorPositions.Contains(pos + Vector2Int.up);
                bool floorDown = floorPositions.Contains(pos + Vector2Int.down);
                bool floorLeft = floorPositions.Contains(pos + Vector2Int.left);
                bool floorRight = floorPositions.Contains(pos + Vector2Int.right);
                bool floorUpRight = floorPositions.Contains(pos + Vector2Int.up + Vector2Int.right);
                bool floorUpLeft = floorPositions.Contains(pos + Vector2Int.up + Vector2Int.left);
                bool floorDownRight = floorPositions.Contains(pos + Vector2Int.down + Vector2Int.right);
                bool floorDownLeft = floorPositions.Contains(pos + Vector2Int.down + Vector2Int.left);
                TileBase wallTileToPlace;

                if (floorUp)
                {
                    wallTileToPlace = rd.roomEnv.wallTile.up[Random.Range(0, rd.roomEnv.wallTile.up.Length)];
                } else if (floorDown)
                {
                    wallTileToPlace = rd.roomEnv.wallTile.down[Random.Range(0, rd.roomEnv.wallTile.down.Length)];
                }
                else if (floorLeft)
                {
                    wallTileToPlace = rd.roomEnv.wallTile.left[Random.Range(0, rd.roomEnv.wallTile.left.Length)];
                }
                else if (floorRight)
                {
                    wallTileToPlace = rd.roomEnv.wallTile.right[Random.Range(0, rd.roomEnv.wallTile.right.Length)];
                }
                else if (floorUpLeft)
                {
                    wallTileToPlace = rd.roomEnv.wallTile.upLeft[Random.Range(0, rd.roomEnv.wallTile.upLeft.Length)];
                }
                else if (floorUpRight)
                {
                    wallTileToPlace = rd.roomEnv.wallTile.upRight[Random.Range(0, rd.roomEnv.wallTile.upRight.Length)];
                }
                else if (floorDownLeft)
                {
                    wallTileToPlace = rd.roomEnv.wallTile.downLeft[Random.Range(0, rd.roomEnv.wallTile.downLeft.Length)];
                }
                else if (floorDownRight)
                {
                    wallTileToPlace = rd.roomEnv.wallTile.downRight[Random.Range(0, rd.roomEnv.wallTile.downRight.Length)];
                }
                else
                {
                    wallTileToPlace = rd.roomEnv.wallTile.single[Random.Range(0, rd.roomEnv.wallTile.single.Length)];
                }
                PlaceSingleTile(map, wallTileToPlace, pos, rd);
            }
        } else
        {
            foreach (var pos in basicWallPos)
            {
                if (rd.roomEnv.wallTile.ruleTile is CustomWallRuleTile ruleTile)
                {
                    ruleTile.SetFloorTiles(rd.roomEnv.floorTile);
                }
                PlaceSingleTile(map, rd.roomEnv.wallTile.ruleTile, pos, rd);
            }
        }
    }
    
    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        
        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.up + Vector2Int.left,
            Vector2Int.up + Vector2Int.right,
            Vector2Int.down + Vector2Int.left,
            Vector2Int.down + Vector2Int.right

        };
        foreach (var pos in floorPositions)
        {
            foreach (var dir in directions)
            {
                Vector2Int neighborPos = pos + dir;
                if (!floorPositions.Contains(neighborPos))
                {
                    wallPositions.Add(neighborPos);
                }
            }
        }
        return wallPositions;
    }
    public static void PlaceFloorTile(HashSet<Vector2Int> floorPositions, RoomData roomData, Tilemap map)
    {
        foreach (var pos in floorPositions)
        {
            PlaceSingleTile(map, roomData.roomEnv.floorTile[Random.Range(0, roomData.roomEnv.floorTile.Length)], pos, roomData);
        }
    }
    static void PlaceSingleTile(Tilemap map, TileBase tile, Vector2Int position, RoomData rd)
    {
        Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
        map.SetTile(tilePosition, tile);
        map.SetColor(tilePosition, RoomColor(rd));
    }
    public static Color RoomColor(RoomData rd)
    {
        switch (rd.roomAttribute)
        {
            case RoomAttribute.Light:
                return Color.yellow;
            case RoomAttribute.Dark:
                return Color.gray;
            case RoomAttribute.Fire:
                return Color.red;
            case RoomAttribute.Water:
                return Color.blue;
            case RoomAttribute.Wind:
                return Color.green;
            case RoomAttribute.Normal:
                return Color.clear;
            default:
                return Color.white;
        }
    }
}
