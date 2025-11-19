using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// A* Pathfinding implementation with agent size support
/// </summary>
public class Pathfinding : MonoBehaviour
{
    NodeGrid grid;
    
    void Awake()
    {
        grid = GetComponent<NodeGrid>();
    }
    
    // Updated to accept agentRadius parameter
    public void FindPath(Vector3 startPos, Vector3 targetPos, float agentRadius = 0.5f)
    {
        StartCoroutine(FindPathCoroutine(startPos, targetPos, agentRadius));
    }

    IEnumerator FindPathCoroutine(Vector3 startPos, Vector3 targetPos, float agentRadius)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        // Check if agent can fit at start position
        if (!CanAgentFitAtNode(startNode, agentRadius))
        {
            startNode = FindNearestWalkableNode(startPos, agentRadius);
            if (startNode == null)
            {
                Debug.LogWarning($"No walkable start node found for agent with radius {agentRadius}!");
                PathRequestManager.Instance.FinishedProcessingPath(waypoints, false);
                yield break;
            }
        }

        // Check if agent can fit at target position
        if (!CanAgentFitAtNode(targetNode, agentRadius))
        {
            targetNode = FindNearestWalkableNode(targetPos, agentRadius);
            if (targetNode == null)
            {
                Debug.LogWarning($"No walkable target node found for agent with radius {agentRadius}!");
                PathRequestManager.Instance.FinishedProcessingPath(waypoints, false);
                yield break;
            }
        }

        if (CanAgentFitAtNode(startNode, agentRadius) && CanAgentFitAtNode(targetNode, agentRadius))
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

                // Updated to use size-aware neighbor checking
                foreach (Node neighbor in GetWalkableNeighbors(currentNode, agentRadius))
                {
                    if (closedSet.Contains(neighbor))
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

    // New method: Check if agent can fit at a specific node
    bool CanAgentFitAtNode(Node node, float agentRadius)
    {
        if (!node.walkable) return false;
        
        // Check if agent would overlap with obstacles at this position
        Vector3 position = node.worldPosition;
        return !Physics2D.OverlapCircle(position, agentRadius, grid.unwalkableMask);
    }

    // New method: Get neighbors that the agent can actually use
    List<Node> GetWalkableNeighbors(Node node, float agentRadius)
    {
        List<Node> walkableNeighbors = new List<Node>();
        List<Node> allNeighbors = grid.GetNeighbors(node);

        foreach (Node neighbor in allNeighbors)
        {
            // Check if this neighbor is accessible for this agent size
            if (CanAgentFitAtNode(neighbor, agentRadius))
            {
                // For diagonal movement, do additional checking
                if (IsDiagonalMovement(node, neighbor))
                {
                    if (CanAgentPassThroughDiagonal(node, neighbor, agentRadius))
                    {
                        walkableNeighbors.Add(neighbor);
                    }
                }
                else
                {
                    // Direct horizontal/vertical movement
                    walkableNeighbors.Add(neighbor);
                }
            }
        }

        return walkableNeighbors;
    }

    // Check if movement is diagonal
    bool IsDiagonalMovement(Node from, Node to)
    {
        return Mathf.Abs(from.gridX - to.gridX) == 1 && Mathf.Abs(from.gridY - to.gridY) == 1;
    }

    // Check if agent can squeeze through diagonal passage
    bool CanAgentPassThroughDiagonal(Node from, Node to, float agentRadius)
    {
        Vector3 fromPos = from.worldPosition;
        Vector3 toPos = to.worldPosition;
        
        // Sample multiple points along the diagonal path
        int samples = 3;
        for (int i = 0; i <= samples; i++)
        {
            float t = (float)i / samples;
            Vector3 samplePos = Vector3.Lerp(fromPos, toPos, t);
            
            if (Physics2D.OverlapCircle(samplePos, agentRadius, grid.unwalkableMask))
            {
                return false; // Path blocked
            }
        }
        return true;
    }

    // Updated to consider agent size
    Node FindNearestWalkableNode(Vector3 worldPos, float agentRadius)
    {
        Node startNode = grid.NodeFromWorldPoint(worldPos);
        Queue<Node> searchQueue = new Queue<Node>();
        HashSet<Node> searchedNodes = new HashSet<Node>();

        searchQueue.Enqueue(startNode);
        searchedNodes.Add(startNode);
        
        while (searchQueue.Count > 0)
        {
            Node currentNode = searchQueue.Dequeue();
            
            // Check if agent can fit at this node
            if (CanAgentFitAtNode(currentNode, agentRadius))
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
