using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "RoomEnvironment", menuName = "Scriptable Objects/RoomEnvironment")]
public class RoomEnvironment : ScriptableObject
{
    public TileBase[] floorTile;
    public TileBase[] wallTile;
}
