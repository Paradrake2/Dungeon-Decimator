using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "StandardRoom", menuName = "Rooms/StandardRoom")]
public class StandardRoom : RoomData
{
    public int _width = 50;
    public int _height = 50;
    public override void GenerateRoom()
    {
        Debug.Log("Generating Standard Room: " + roomName);
    }
}
