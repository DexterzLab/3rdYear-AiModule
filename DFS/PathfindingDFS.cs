using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathfindingDFS : MonoBehaviour {

	//public Transform seeker, target;
	Grid grid;

    PathFindingRequestDFS requestManager;

	void Awake() {

        requestManager = GetComponent<PathFindingRequestDFS>();
		grid = GetComponent<Grid> ();
	}

	void Update() {
		//FindPath (seeker.position, target.position);
	}

    //extension method for the PathFindingRequest class
    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }





    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {

        Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);


        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        if (startNode.walkable && targetNode.walkable)
        {

            Stack<Node> openSet = new Stack<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Push(startNode);

            while (openSet.Count > 0)
            {
                Node node = openSet.Pop();

                closedSet.Add(node);
                

                if (node == targetNode)
                {
                    pathSuccess = true;

                    break;

                }

                foreach (Node neighbour in grid.GetNeighbours(node))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Push(neighbour);   
                }
            }
        }
        yield return null;
        //after the frame has finished call the finishedprocessingpath method

        if (pathSuccess){
           waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
	}

	Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
        

	}

    //Reduces the number of waypoints if there is no direction change
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 directionOld = Vector3.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 directionNew = new Vector3(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY, path[i-1].gridZ - path[i].gridZ);
            if(directionNew != directionOld)
            {
                //reckognise the path has changed direction
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }

        //convert list to an array
        return waypoints.ToArray();
    }

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        return dstX * dstX + dstY * dstY + dstZ * dstZ;
        /*
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
        */
	}
}
