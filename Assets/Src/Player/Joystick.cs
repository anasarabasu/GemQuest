using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour, ISaveLoad {
    [SerializeField] Rigidbody2D body;
    [SerializeField] float speed = 4f;

    // public static JoystickButtons instance;

    private void Awake() {
        canMove = true;
    }

    private void Start() {
        GameObject[] teamMates = {GameObject.FindGameObjectWithTag("Hels"), GameObject.FindGameObjectWithTag("Pom"), GameObject.FindGameObjectWithTag("Iska")};

        foreach (GameObject member in teamMates) {
            if(member != null)
                Physics2D.IgnoreCollision(member.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());    
        }
    }

    internal bool canMove;
    private void FixedUpdate() {
        if(canMove && direction != Vector2.zero) 
            body.AddForce(direction * speed * body.drag); 
    }

    private enum Facing {FRONT = 0, BACK = 1, LEFT = 2, RIGHT = 3}
    private int facingDirection;

    internal int UpdateDirection() {
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

    private Vector3 rayDirection;
    private float offset = 1.75f;
    internal Vector2 UpdateRay() {
         switch (UpdateDirection()) {
            case (int)Facing.FRONT: 
                rayDirection = new Vector3(0, 0 + -offset);
                break;
            case (int)Facing.BACK: 
                rayDirection = new Vector3(0, 0 + offset);
                break;
            case (int)Facing.LEFT: 
                rayDirection = new Vector3(0 + -offset, 0);
                break;
            case (int)Facing.RIGHT: 
                rayDirection = new Vector3(0 + offset, 0);
                break;
        }
        return rayDirection;
    }

    public Vector2 direction;
    public void OnMove(InputAction.CallbackContext context) {
        direction = context.ReadValue<Vector2>();
    }

    public void Save(DataRoot data) {
        data.levelData.PIK_levelCoordinates = transform.position;
    }

    public void Load(DataRoot data) {
        transform.position = data.levelData.PIK_levelCoordinates;
    }

}