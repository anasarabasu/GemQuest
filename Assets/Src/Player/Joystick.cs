using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour, ISaveLoad {
    [SerializeField] Rigidbody2D body;
    [SerializeField] float speed = 4f;

    // public static JoystickButtons instance;

    private void Awake() {
        canMove = true;
    }

    [HideInInspector] public bool canMove;
    private void FixedUpdate() {
        if(canMove) 
            body.AddForce(direction * speed * body.drag); 
    }

    private enum Facing {FRONT = 0, BACK = 1, LEFT = 2, RIGHT = 3}
    private int facingDirection;

    public int FacingDirection() {
        if(direction.y < 0) 
            facingDirection = (int)Facing.FRONT;
        else if(direction.y > 0) 
            facingDirection = (int)Facing.BACK;

        if(direction.x < -0.5) 
            facingDirection = (int)Facing.LEFT;
        else if(direction.x > 0.5) 
            facingDirection = (int)Facing.RIGHT;

        return facingDirection;
    }

    [HideInInspector] public Vector2 direction;
    public void OnMove(InputAction.CallbackContext context) {
        direction = context.ReadValue<Vector2>();
    }

    [HideInInspector] public bool IsMoving() {
        if(Math.Abs(body.velocity.x) > 0.4 || Math.Abs(body.velocity.y) > 0.4) 
            return true;
        else 
            return false;
    }

    public void Save(ref DataRoot data) {
        data.gameData.levelCoordinates = this.transform.position;
    }

    public void Load(DataRoot data) {
        this.transform.position = data.gameData.levelCoordinates;
    }

}