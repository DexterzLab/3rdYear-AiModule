using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    float forward = 0f;
    float movementspeed = 40f;
    float side;
 
    

    // Use this for initialization
    void Start()
    {
       
    }

    void Update()
    {
     
        forward = Input.GetAxis("Vertical") * movementspeed;

        side = Input.GetAxis("Horizontal") * movementspeed; 

    }



    private void FixedUpdate()
    {
        //Vector2 velocity = rigidbody.velocity;
        //velocity.x = forward;
        //rigidbody.velocity = velocity;
        // transform.eulerAngles = new Vector3(0.0f,turn,0.0f);

        //rigidbody.AddForce(forward * Time.deltaTime, 0, 0);
        transform.Translate(0, 0, forward * Time.deltaTime);

        transform.Translate(side * Time.deltaTime,0,0);

    }
}
