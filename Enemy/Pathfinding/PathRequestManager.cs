using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages pathfinding requests to prevent overloading
/// </summary>
public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    
    public static PathRequestManager Instance;
    Pathfinding pathfinding;
    
    bool isProcessingPath;
    
    void Awake()
    {
        Instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }
    
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        Instance.pathRequestQueue.Enqueue(newRequest);
        Instance.TryProcessNext();
    }
    
    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.FindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }
    
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;
        
        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> cb)
        {
            pathStart = start;
            pathEnd = end;
            callback = cb;
        }
    }
}
