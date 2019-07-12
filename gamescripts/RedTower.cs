using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTower : MonoBehaviour {
    //call the state machine and give teh states a name, give it as public otherwise cannot access switch components
    public TowerStates states;

    public float ViewDistance = 30;
    public float playerDistance;

    public Transform target;
    private Transform playerTrans;
    // private BlueMinionMachine targetEnemy;

    public float range = 15f;
    public string minionTag = "BlueMinion";


    //statemachine states
    public enum TowerStates
    {
        Idle,
        MinionAttack,
        PlayerAttack
    }

    // Use this for initialization
    void Start()
    {

        //initialize the state machine and set it to the first state
        states = TowerStates.Idle;


        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (states)
        {
            case TowerStates.Idle:
                {
                    //Check for minions in the scene
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag(minionTag);
                    float shortestDistance = Mathf.Infinity;
                    GameObject nearestEnemy = null;
                    foreach (GameObject enemy in enemies)
                    {
                        //convert the distance of the enemy into a float 
                        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        //if their distance is within range
                        if (distanceToEnemy < shortestDistance)
                        {
                            //assign the first game object into the array as priority 
                            shortestDistance = distanceToEnemy;
                            nearestEnemy = enemy;
                        }
                    }

                    //if a object has been assigned 
                    if (nearestEnemy != null && shortestDistance <= range)
                    {
                        //change state making the same for check, and deal damage to minion
                        target = nearestEnemy.transform;
                        states = TowerStates.MinionAttack;

                    }
                    else
                    {
                        target = null;
                    }



                    var myplayer = GameObject.Find("Player");

                    if (myplayer != null)
                    {
                        playerDistance = Vector3.Distance(playerTrans.position, transform.position);
                        if (playerDistance < ViewDistance)
                        {
                            states = TowerStates.PlayerAttack;

                        }

                    }
                    if (myplayer == null)
                    {
                        Debug.Log("Player not in range");

                    }

                    Debug.Log("Waiting for minion");

                    break;

                }

            case TowerStates.MinionAttack:
                {
                    Debug.Log("Attacking Minion");
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag(minionTag);
                    float shortestDistance = Mathf.Infinity;
                    GameObject nearestEnemy = null;
                    foreach (GameObject enemy in enemies)
                    {
                        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distanceToEnemy < shortestDistance)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestEnemy = enemy;
                        }
                    }

                    if (nearestEnemy != null && shortestDistance <= range)
                    {
                        nearestEnemy.GetComponent<MinionHealth>().health -= 5;
                        target = nearestEnemy.transform;



                    }
                    else
                    {
                        target = null;
                        states = TowerStates.Idle;
                    }

                    break;
                }

            case TowerStates.PlayerAttack:
                {


                    var myplayer = GameObject.Find("Player");

                    if (myplayer)
                    {
                        playerDistance = Vector3.Distance(playerTrans.position, transform.position);
                        if (playerDistance < ViewDistance)
                        {
                            myplayer.GetComponent<PlayerHealth>().health -= 5f;
                            Debug.Log("Attacking Player");

                        }

                    }
                    if (!myplayer)
                    {
                        Debug.Log("Player not in range");

                    }
                    states = TowerStates.Idle;
                    break;
                }
        }

    }
}
