using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerMovement : Movement
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        moveStates.Add(Tile.States.free);
    }

    void Update()
    {


        SetSurroundCollision();

        if (Input.GetButtonDown("Fire1"))
        {
            transform.position = startPos;
        }

        if (Input.GetKey(KeyCode.W) && !isMoving  && !colliderCheck[0])
        {
            StartCoroutine(Move(Vector3.up));
        }
        if (Input.GetKey(KeyCode.A) && !isMoving && !colliderCheck[2])
        {
            StartCoroutine(Move(Vector3.left));
        }
        if (Input.GetKey(KeyCode.S) && !isMoving && !colliderCheck[1])
        {
            StartCoroutine(Move(Vector3.down));
        }
        if (Input.GetKey(KeyCode.D) && !isMoving && !colliderCheck[3])
        {
            StartCoroutine(Move(Vector3.right));
        }
    }

    public void Lose()
    {
        Destroy(this);
    }

    
}
