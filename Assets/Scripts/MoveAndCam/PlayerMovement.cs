﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Script is attached to player object
    // Then player objects rigidbody is set to the variable.

    public float moveSpeed;     // variable for movement speed
    private Rigidbody2D rb;      // variable for players 2d rigibody
    private Vector2 moveDir;    // vector variable for calculating movement direction (only needs to be a 2dvector)
    private Animator animator;
    private SpriteRenderer sprite;

    new Camera camera;

    private void Start()
	{
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        camera = Camera.main;
    }

	// Update is called once per frame
	void Update()
    {
        sprite.sortingOrder = -Mathf.RoundToInt(transform.position.y);
        Inputs();
    }

    // FixedUpdate called every fixed-rate frame
    void FixedUpdate()
	{
        actionDirection();
        idleDirection();
        Move();
    }

    // handles the players inputs
    void Inputs()
	{
        if (GetComponent<PlayerDamage>().health <= 0)
            return;

        float X = Input.GetAxisRaw("Horizontal");           // reads left and right inputs
        float Y = Input.GetAxisRaw("Vertical");             // reads up and down inputs
        moveDir = new Vector2(X, Y).normalized;             // calculates the desired postion of the players inputs and noramlizes it (making the directional magnitude 1)

        animator.SetFloat("Horizontal", X);
        animator.SetFloat("Vertical", Y);
        animator.SetFloat("Speed",moveDir.sqrMagnitude);
    }

    // moves the player object (the rigibody to be exact)
    void Move()
	{
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);    // set the players rigibody velocity vector to the moveDirection vecotr (multipying the values by moveSpeed)
	}

    void actionDirection()
	{
        //float X = camera.ScreenToWorldPoint(Input.mousePosition).normalized.x;
        //float Y = camera.ScreenToWorldPoint(Input.mousePosition).normalized.y;

        float X = Input.GetAxisRaw("Horizontal");
        float Y = Input.GetAxisRaw("Vertical");

        //down
        if (X == 0 && Y == -1)
        {
            animator.SetFloat("ActionX", 0);
            animator.SetFloat("ActionY", -1);
        }
        //down/right
        if (X == 1 && Y == -1)
        {
            animator.SetFloat("ActionX", 1);
            animator.SetFloat("ActionY", -1);
        }
        //right
        if (X == 1 && Y == 0)
        {
            animator.SetFloat("ActionX", 1);
            animator.SetFloat("ActionY", 0);
        }
        //up/right
        if (X == 1 && Y == 1)
        {
            animator.SetFloat("ActionX", 1);
            animator.SetFloat("ActionY", 1);
        }
        //up
        if (X == 0 && Y == 1)
        {
            animator.SetFloat("ActionX", 0);
            animator.SetFloat("ActionY", 1);
        }
        //up/left
        if (X == -1 && Y == 1)
        {
            animator.SetFloat("ActionX", -1);
            animator.SetFloat("ActionY", 1);
        }
        //left
        if (X == -1 && Y == 0)
        {
            animator.SetFloat("ActionX", -1);
            animator.SetFloat("ActionY", 0);
        }
        //down/left
        if (X == -1 && Y == -1)
        {
            animator.SetFloat("ActionX", -1);
            animator.SetFloat("ActionY", -1);
        }
    }


    //For determining the players idle direction
    void idleDirection()
	{
        float X = Input.GetAxisRaw("Horizontal");
        float Y = Input.GetAxisRaw("Vertical");

        //down
        if (X == 0 && Y == -1)
        {
            animator.SetFloat("LastX", 0);
            animator.SetFloat("LastY", -1);
        }
        //down/right
        if (X == 1 && Y == -1)
        {
            animator.SetFloat("LastX", 1);
            animator.SetFloat("LastY", -1);
        }
        //right
        if (X == 1 && Y == 0)
        {
            animator.SetFloat("LastX", 1);
            animator.SetFloat("LastY", 0);
        }
        //up/right
        if (X == 1 && Y == 1)
        {
            animator.SetFloat("LastX", 1);
            animator.SetFloat("LastY", 1);
        }
        //up
        if (X == 0 && Y == 1)
        {
            animator.SetFloat("LastX", 0);
            animator.SetFloat("LastY", 1);
        }
        //up/left
        if (X == -1 && Y == 1)
        {
            animator.SetFloat("LastX", -1);
            animator.SetFloat("LastY", 1);
        }
        //left
        if (X == -1 && Y == 0)
        {
            animator.SetFloat("LastX", -1);
            animator.SetFloat("LastY", 0);
        }
        //down/left
        if (X == -1 && Y == -1)
        {
            animator.SetFloat("LastX", -1);
            animator.SetFloat("LastY", -1);
        }
        

    }

    public void IncreaseSpeed (float percentIncrease = 1)
    {
        moveSpeed = moveSpeed * percentIncrease;
        int new_floor = GameController.FloorLevel + 1;
        GameController.StartGame("next floor", new_floor);
    }
}
