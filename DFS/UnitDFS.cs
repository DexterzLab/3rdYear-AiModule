﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDFS : MonoBehaviour {

    public Transform target;
    float speed = 5;
    Vector3[] path;
    int targetIndex;

    void Start()
    {
        PathFindingRequestDFS.RequestPath(transform.position, target.position, OnPathFound);
    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;

            //stop the coroutine if is already running
            StopCoroutine("FollowPath");

            //start it again 
            StartCoroutine("FollowPath");

        }



    }


    IEnumerator FollowPath()
    {

        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }


            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }

    }

    //drawing the calculated path
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

}
