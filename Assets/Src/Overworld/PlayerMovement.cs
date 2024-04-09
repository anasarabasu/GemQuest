using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] OrbitPhysics planet;
    [SerializeField] Rigidbody body;
    [SerializeField] float speed = 4f;
    private Vector3 velocity;

    private void FixedUpdate() {
        planet.Orbit(body.transform);
        
        body.AddForce(body.transform.TransformDirection(velocity) * body.drag);
    }

    public void OnMove3D(InputAction.CallbackContext context) { //joystick
        Vector2 rawMoveValue = context.ReadValue<Vector2>();
        velocity = new Vector3(rawMoveValue.x, 0, rawMoveValue.y) * speed;
    }
}