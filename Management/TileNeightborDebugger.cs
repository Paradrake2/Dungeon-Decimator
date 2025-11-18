using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileNeightborDebugger : MonoBehaviour
{
    public Tilemap tilemap;

    [ContextMenu("Log Neighbors At Mouse")]
    void LogNeighborsAtMouse()
    {
        if (tilemap == null)
        {
            Debug.LogWarning("Tilemap not assigned.");
            return;
        }

        Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = tilemap.WorldToCell(world);

        Debug.Log($"Checking neighbors at cell {cell}");

        LogNeighbor("Up",    cell + new Vector3Int(0, 1, 0));
        LogNeighbor("Down",  cell + new Vector3Int(0,-1, 0));
        LogNeighbor("Left",  cell + new Vector3Int(-1,0, 0));
        LogNeighbor("Right", cell + new Vector3Int(1, 0, 0));
    }

    void LogNeighbor(string label, Vector3Int pos)
    {
        TileBase t = tilemap.GetTile(pos);
        Debug.Log($"{label}: {(t ? t.name : "NULL")}");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) LogNeighborsAtMouse();
    }
}
