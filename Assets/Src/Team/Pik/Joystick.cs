using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Joystick : MonoBehaviour, ISaveable {
    [SerializeField] Rigidbody2D body;
    [SerializeField] float speed = 4f;
    [SerializeField] EntityStatData pikStats;

    private void Start() {
        GameObject[] teamMates = {GameObject.FindGameObjectWithTag("Hels"), GameObject.FindGameObjectWithTag("Pom"), GameObject.FindGameObjectWithTag("Iska")};

        foreach (GameObject member in teamMates) {
            if(member != null)
                Physics2D.IgnoreCollision(member.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());    
        }


        slowSpeed = speed / 2;
    }

    internal static bool canMove = true;
    internal static void MovementState(bool state) {
        canMove = state;
    }

    float actualSpeed;
    float slowSpeed;
    private void Update() {
        if(pikStats.currentEnergy > 0)
            actualSpeed = speed;
        else
            actualSpeed = slowSpeed;

        if(!body.IsAwake())
            if(pikStats.currentEnergy < pikStats.energy)
                pikStats.currentEnergy += 2 * Time.deltaTime;
    }


    private void FixedUpdate() {
        if(canMove && direction != Vector2.zero) 
            body.AddForce(direction * actualSpeed); 
    }

    private enum Facing {FRONT = 0, BACK = 1, LEFT = 2, RIGHT = 3}
    private static int facingDirection;
    internal static int UpdateDirection() {
        if(direction.y < 0) {
            facingDirection = (int)Facing.FRONT;
            // flashLight.transform.Rotate(0,0,180);
        }
        else if(direction.y > 0) {
            facingDirection = (int)Facing.BACK;
            // flashLight.transform.Rotate(0,0,0);
        }

        if(direction.x < -0.5) {
            facingDirection = (int)Facing.LEFT;
            // flashLight.transform.Rotate(0,0,90);
        }
        else if(direction.x > 0.5) {

            facingDirection = (int)Facing.RIGHT;
            // flashLight.transform.Rotate(0,0,-90);
        }

        return facingDirection;
    }

    private static Vector3 rayDirection;
    private static float offset = 1.75f;
    internal static Vector2 UpdateRay() {
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

    public static Vector2 direction;
    internal static Vector2 GetDirection() {
        return direction;
    }
    public void OnMove(InputAction.CallbackContext context) {
        direction = context.ReadValue<Vector2>();
    }

    public void Save(DataRoot data) {
        data.levelData.levelCoordinates = transform.position;
    }

    public void Load(DataRoot data) {
        transform.position = data.levelData.levelCoordinates;
    }

}