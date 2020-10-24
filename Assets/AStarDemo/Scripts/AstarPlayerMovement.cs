﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPlayerMovement : MonoBehaviour
{
    private Vector2 movement;

    [Tooltip("How fast are we?")]
    public float moveSpeed = 5.0f;

    [Tooltip("Is our movement frozen?")]
    public bool isFrozen = false;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (!isFrozen)
        {
            rb.AddForce(movement * moveSpeed);
        }
    }
}