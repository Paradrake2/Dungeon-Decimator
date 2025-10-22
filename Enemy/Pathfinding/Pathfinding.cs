using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// A* Pathfinding implementation
/// </summary>
public class Pathfinding : MonoBehaviour
{
    NodeGrid grid;
    
    void Awake()
    {
        grid = GetComponent<NodeGrid>();
    }
    
    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPathCoroutine(startPos, targetPos));
    }

    IEnumerator FindPathCoroutine(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (!startNode.walkable)
        {
            startNode = FindNearestWalkableNode(startPos);
            if (startNode == null)
            {
                Debug.LogWarning("No walkable start node found!");
                yield break;
            }
        }

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newCostToNeighbour < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCostToNeighbour;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                        else
                            openSet.UpdateItem(neighbor);
                    }
                }
            }
        }

        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }

        PathRequestManager.Instance.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Node FindNearestWalkableNode(Vector3 worldPos)
    {
        Node node = grid.NodeFromWorldPoint(worldPos);
        Queue<Node> searchQueue = new Queue<Node>();
        HashSet<Node> searchedNodes = new HashSet<Node>();

        searchQueue.Enqueue(node);
        searchedNodes.Add(node);
        while (searchQueue.Count > 0)
        {
            Node currentNode = searchQueue.Dequeue();
            if (currentNode.walkable)
            {
                return currentNode;
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!searchedNodes.Contains(neighbor))
                {
                    searchedNodes.Add(neighbor);
                    searchQueue.Enqueue(neighbor);
                }
            }
        }
        return null;
    }
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        
        Vector3[] waypoints = SimplifyPath(path);
        System.Array.Reverse(waypoints);
        return waypoints;
    }
    
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
    
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        
        if (dstX > dstY)
            return 14*dstY + 10*(dstX-dstY);
        return 14*dstX + 10*(dstY-dstX);
    }
}
