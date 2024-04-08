using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOverworldMovement : MonoBehaviour {
    [SerializeField] OrbitPhysics planet;
    [SerializeField] Rigidbody body;
    [SerializeField] float speed = 4f;
    private Vector3 velocity;

    private void FixedUpdate() {
        planet.Attract(body.transform);

        // body.MovePosition(body.position + transform.TransformDirection(velocity) * Time.deltaTime);
        body.AddForce(transform.TransformDirection(velocity) * body.drag);
    }

    public void OnMove(InputAction.CallbackContext context) {
        Vector2 rawMoveValue = context.ReadValue<Vector2>();
        velocity = new Vector3(rawMoveValue.x, 0, rawMoveValue.y) * speed;
    }
}