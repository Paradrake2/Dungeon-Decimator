using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/RoomData")]
public class RoomData : ScriptableObject
{
    public string roomName;
    public int width;
    public int height;
    public int minEnemies;
    public int maxEnemies;
    public float obstacleDensity;
    public float chestChance; // chance for chest loot
    public bool bossRoom = false;
    
}
