using UnityEngine;
using Input = UnityEngine.Input;

public class Rotator : MonoBehaviour {
	[SerializeField] Rigidbody sphere;
	[SerializeField] float rotationSpeed;
	private Vector2 spinVector;
	private bool dragging = false;
	
	private void OnMouseDown() {
		dragging = true;
	}

	private void OnMouseUp() {
		dragging = false;
	}

    private void Update() {
		if(dragging) {
			if(Input.touchCount > 0) {
				spinVector = Input.GetTouch(0).deltaPosition * rotationSpeed * Time.deltaTime;
				Debug.Log("Touch");
			}
			else {
				spinVector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * (rotationSpeed * 50);
				Debug.Log("Mouse");
			}
		} 
	}

	private void FixedUpdate() {
		if(dragging) {
			sphere.AddTorque(Vector3.down * spinVector.x);
			sphere.AddTorque(Vector3.right * spinVector.y);
		}
	}
}
