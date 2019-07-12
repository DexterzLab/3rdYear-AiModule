using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower2 : MonoBehaviour {
    //call the state machine and give teh states a name, give it as public otherwise cannot access switch components
    public TowerStates states;
    
    public float ViewDistance = 30;

    public Transform target;
   // private BlueMinionMachine targetEnemy;

    public float range = 15f;
    public string minionTag = "RedMinion";


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
    }

    // Update is called once per frame
    void Update()
    {
        switch (states)
        {
            case TowerStates.Idle:
                {
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
                        target = nearestEnemy.transform;
                        states = TowerStates.MinionAttack;
                        
                    }
                    else
                    {
                        target = null;
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
                    
                    break;
                }
        }

    }
}
