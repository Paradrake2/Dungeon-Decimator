using System.Collections.Generic;
using UnityEngine;

public static class GenerationAlgorithmComponents
{
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
}
