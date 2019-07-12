using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMinionMachine : MonoBehaviour {

	 //call the state machine and give teh states a name, give it as public otherwise cannot access switch components
    public MinionStates states;

    public float cooldownTimer = 5f;
    
    bool startPath = false;

    public int ViewDistance = 150;
    public float minionDistance;

    //private GameObject[] Enemies;
    private Transform enemyTrans;
    private Vector3 rayDirection;

    private BlueMinionMachine _target;

    public Transform target;
    float speed = 15;
    Vector3[] path;
    int targetIndex;


    //statemachine states
    public enum MinionStates
    {
        Wander,
        Chase,
        Idle
    }

	// Use this for initialization
	void Start () {

        //initialize the state machine and set it to the first state
        states = MinionStates.Idle;

        enemyTrans = GameObject.FindGameObjectWithTag("BlueMinion").transform;

     

    }
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;

        cooldownTimer = Mathf.Clamp(0, cooldownTimer, 5);

        if (enemyTrans == null)
        {
            enemyTrans = GameObject.FindGameObjectWithTag("BlueMinion").transform;
        }

        switch (states)
        {
            case MinionStates.Idle:
            {
                    
                    Debug.Log("Minion waiting for game start");
                    //count down the time for every second in game.
                    cooldownTimer -= Time.deltaTime;

                    if(cooldownTimer <= 0)
                    {
                        states = MinionStates.Wander;
                    }
                    break;
            }
            case MinionStates.Chase:
            {

                    if (_target == null)
                    {
                        states = MinionStates.Wander;
                        return;
                    }

                    transform.LookAt(enemyTrans);
                    transform.Translate(Vector3.forward * 10 * Time.deltaTime);
                    //the raycast will read the script of the colliding object and subtract its health

                    if (Physics.Raycast(transform.position, transform.forward, out hit))
                    {

                        if (hit.collider.gameObject.tag == "BlueMinion")
                        {
                            hit.collider.gameObject.GetComponent<MinionHealth>().health -= 5f;
                            print("BlueEnemy Detected");                          
                        }

                    }
                  //  Debug.Log(states);
                    break;
            }


            case MinionStates.Wander:
            {
                   if(startPath == false)
                   {
                        startPath = true;
                        PathFindingRequest.RequestPath(transform.position, target.position, OnPathFound);

                   }


                    var targetToAggro = CheckForAggro();
                    if (targetToAggro != null)
                    {
                        _target = targetToAggro.GetComponent<BlueMinionMachine>();
                        states = MinionStates.Chase;
                    }
                    Debug.Log("Minion moving down lane");
                    break;
            }
        }
        
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

    //checking if the raycast and transform is true or false for the target
    private Transform CheckForAggro()
    {
        RaycastHit hit; 
        var pos = transform.position;

        //convert the enemies distance into a float
        minionDistance = Vector3.Distance(enemyTrans.position, transform.position);

        //if the float is lower(within range) than the distance look at the target
        if (minionDistance <= ViewDistance)
        {
            transform.LookAt(enemyTrans);

            //if the raycast has hit an object
            if (Physics.Raycast(pos, transform.forward, out hit, ViewDistance))
            {
                //check if the collision of object contains component
                var minion = hit.collider.GetComponent<BlueMinionMachine>();

                //if component is true
                if (minion != null)
                {
                    //return the target transform as true
                    return minion.transform;
                }
            
            }
        }

        //otherwise return false
        return null;
    }

    void OnDrawGizmos()
    {
 
        Vector3 frontRayPoint = transform.position + (transform.forward * ViewDistance);

        Debug.DrawLine(transform.position, frontRayPoint, Color.green);
       

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
