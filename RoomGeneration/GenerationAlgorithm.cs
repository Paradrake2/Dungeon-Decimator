using System.Collections.Generic;
using UnityEngine;


public abstract class GenerationAlgorithm : ScriptableObject
{
    public static GenerationAlgorithm Instance;
    public abstract void GenerateMap(RoomData roomData);
    protected Vector2Int startPos = new Vector2Int(0, 0);
    public HashSet<Vector2Int> floorPosTracker = new HashSet<Vector2Int>();
}
