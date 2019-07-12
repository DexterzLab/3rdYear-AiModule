using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//script used for calculating paths for multiple nodes but spread across multiple frames
//to stop any bottlenecks
public class PathFindingRequestDFS : MonoBehaviour {

    //a queue which will hold the objects of paths being drawn
    Queue<PathRequestDFS> pathRequestQueue = new Queue<PathRequestDFS>();
   
    //storing the current processed path
    PathRequestDFS currentPathRequest;


    //set a reference to this script in order to run the request method 
    static PathFindingRequestDFS instance;

    //reference to the pathfinding class/script
    PathfindingDFS pathfinding;
    bool isProcessingPath;

 

     void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathfindingDFS>();
    }


    //the path isnt caluculated immediatly but is done over the duration of frames
    //A method will be fed into the Action variable, but it written as an array as there will be more 
    //than one vector calculated
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequestDFS newRequest = new PathRequestDFS(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    //check to see if we are already processing a path, and if not then process the next path
    void TryProcessNext()
    {

        if(!isProcessingPath && pathRequestQueue.Count > 0) {
            //get the first item of the request 
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

  

    public void FinishedProcessingPath(Vector3[] path, bool sucess)
    {
        currentPathRequest.callback(path, sucess);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequestDFS
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequestDFS(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
