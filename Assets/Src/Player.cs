using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour { //should i rename this as movement?
    [SerializeField] Rigidbody2D player;
    [SerializeField] float speed;
    private Vector2 moveVal;

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

    //--- OVERWORLD TRIGGER
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("collide");
    }
}

