using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player : MonoBehaviour {
    public Rigidbody2D player; //change to serialise
    public PlayerInput playerInput;
    public float speed;

    private Vector2 moveVal;

    void Start() {
        Debug.Log("Start");
    }

    private void FixedUpdate() {
        player.AddForce(moveVal*player.drag); // player movement update
    }


    //--- PLAYER INPUT
    void OnMove(InputValue input) { // player wasd | joystick input
        moveVal = input.Get<Vector2>()*speed;
        Debug.Log(moveVal +"Moving");
    }
    void OnInteract() {
        Debug.Log("Interact button pressed");
    }

    void OnInventory() {
        Debug.Log("Inventory button pressed");
    }
    //---

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("collide");
    }
}

