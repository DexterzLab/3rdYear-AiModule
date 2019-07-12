using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float health = 200;
    float maxHealth = 200;

   //Transform myTransform;
    public Transform spawnTransform;

    // Use this for initialization
    void Start()
    {
        spawnTransform = GameObject.FindGameObjectWithTag("SpawnBlue").transform;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(maxHealth, 0, health);
        if (health <= 0)
        {
            transform.position = spawnTransform.transform.position;
        }
    }
}
