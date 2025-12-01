using UnityEngine;
using System.Collections.Generic;

public class NodeGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;
    public Transform player;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private void Awake()
    {
        //nodeDiameter = nodeRadius * 2;
        //gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        //gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        //CreateGrid();
    }

    public void SetGridSize(Vector2Int size)
    {
        gridWorldSize = size;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    }
    public void CreateGrid(HashSet<Vector2Int> wallPos)
    {
        Debug.LogWarning("CREATING GRID");
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics2D.OverlapArea(worldPoint - Vector3.one * nodeRadius, worldPoint + Vector3.one * nodeRadius, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }
    public void SetUnwalkable(Vector2Int wallTilePos)
{
    // Each tile is 1x1 unit, so we need to check all nodes within this area
    Vector3 tileBottomLeft = new Vector3(wallTilePos.x, wallTilePos.y, 0);
    Vector3 tileTopRight = new Vector3(wallTilePos.x + 1f, wallTilePos.y + 1f, 0);
    
    // Find all nodes within the tile bounds
    for (int x = 0; x < gridSizeX; x++)
    {
        for (int y = 0; y < gridSizeY; y++)
        {
            Node node = grid[x, y];
            
            // Check if this node's world position is within the wall tile
            if (node.worldPosition.x >= tileBottomLeft.x && 
                node.worldPosition.x < tileTopRight.x &&
                node.worldPosition.y >= tileBottomLeft.y && 
                node.worldPosition.y < tileTopRight.y)
            {
                node.walkable = false;
            }
        }
    }
}
    public int MaxSize => gridSizeX * gridSizeY;

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Calculate bottom-left corner of the grid
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        
        // Convert world position to local grid coordinates
        Vector3 localPosition = worldPosition - worldBottomLeft;
        
        // Calculate grid indices directly from local position
        int x = Mathf.FloorToInt(localPosition.x / nodeDiameter);
        int y = Mathf.FloorToInt(localPosition.y / nodeDiameter);
        
        // Clamp to grid bounds
        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);
        
        return grid[x, y];
    }

    public List<Node> path;
    
    /**
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y));
        
        if (grid != null)
        {
            Node playerNode = null;
            if (player != null)
                playerNode = NodeFromWorldPoint(player.position);
                
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (playerNode == n)
                    Gizmos.color = Color.cyan;
                if (path != null && path.Contains(n))
                    Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
    **/
}
