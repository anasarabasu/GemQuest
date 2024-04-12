using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickMovement : MonoBehaviour, ISaveLoad {
    [SerializeField] Rigidbody2D body;
    [SerializeField] float speed = 4f;

    private bool active = true;
    public void SetActive(bool value) {
        this.active = value;
    }

    private void FixedUpdate() {
        if(active) 
            body.AddForce(velocity * speed * body.drag); 
    }
    
    private Vector2 velocity;
    public void OnMove(InputAction.CallbackContext context) {
        velocity = context.ReadValue<Vector2>();
    }

    public void Save(ref DataRoot data) {
        data.gameData.levelCoordinates = transform.position;
    }

    public void Load(DataRoot data) {
        transform.position = data.gameData.levelCoordinates;
    }

}