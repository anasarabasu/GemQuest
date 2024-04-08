using UnityEngine;
using UnityEngine.InputSystem;

public class GlobeRotator : MonoBehaviour {
    [SerializeField] GameObject cameraObj;
    [SerializeField] Transform target;
    [SerializeField] float cameraMoveSpeed;
    private Vector2 cameraMoveValue;

    [SerializeField] GameObject globeObj;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] float globeMoveSpeed;
    private Vector3 globeMoveValue;


    void Update() {
        cameraObj.transform.RotateAround(target.position, Vector3.left, cameraMoveValue.y*Time.deltaTime);
        rigidBody.AddTorque(globeObj.transform.up*globeMoveValue.x*rigidBody.drag);
    }

    void OnMoveVertical(InputValue inputValue) {
        cameraMoveValue = inputValue.Get<Vector2>()*cameraMoveSpeed;
    }

    void OnMoveHorizontal(InputValue inputValue) {
        globeMoveValue = inputValue.Get<Vector2>()*globeMoveSpeed;
    }
}
