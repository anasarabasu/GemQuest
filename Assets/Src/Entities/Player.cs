using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    public Rigidbody2D player;
    public float speed;

    private Vector2 moveVal;

    void Start() {
        Debug.Log("Start");
    }

    private void FixedUpdate() {
        player.AddForce(moveVal*player.drag); //player movement
    }

    void OnMove(InputValue val) { // player WASD input
        moveVal = val.Get<Vector2>()*speed;
        Debug.Log("Player is moving" +moveVal);
    }
}
