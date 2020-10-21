using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Script is attached to player object
    // Then player objects rigidbody is set to the variable.

    public float moveSpeed;     // variable for movement speed
    public Rigidbody2D rb;      // variable for players 2d rigibody
    private Vector2 moveDir;    // vector variable for calculating movement direction (only needs to be a 2dvector)
    
    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    // FixedUpdate called every fixed-rate frame
    void FixedUpdate()
	{ 
        Move();
    }

    // handles the players inputs
    void Inputs()
	{
        float X = Input.GetAxisRaw("Horizontal");           // reads left and right inputs
        float Y = Input.GetAxisRaw("Vertical");             // reads up and down inputs
        moveDir = new Vector2(X, Y).normalized;             // calculates the desired postion of the players inputs and noramlizes it (making the directional magnitude 1)
	}

    // moves the player object (the rigibody to be exact)
    void Move()
	{
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);    // set the players rigibody velocity vector to the moveDirection vecotr (multipying the values by moveSpeed)
	}
}
