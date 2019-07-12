using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueNexus : MonoBehaviour {

    public GameObject player1;

	// Use this for initialization
	void Start () {

        player1 = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            player1.GetComponent<PlayerHealth>().health += 5f;
        }   
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            player1.GetComponent<PlayerHealth>().health += 5f;
        }
    }

}
