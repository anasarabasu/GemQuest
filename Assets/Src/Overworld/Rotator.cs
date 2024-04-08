using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class Rotator : MonoBehaviour {
	[SerializeField] Rigidbody sphere;
	[SerializeField] float rotationSpeed;
	private bool dragging = false;
	
	float x;
	
	private void OnMouseDown() {
		dragging = true;
	}

	

	private void OnMouseUp() {
		dragging = false;
	}

    private void Update() {
		Debug.Log(dragging);

		// if(Input.touchCount > 0) {
		// 	Touch touch = Input.GetTouch(0);

		// 	if(touch.phase == TouchPhase.Began) {
		// 		dragging = true;
		// 	}
		// 	if(touch.phase == TouchPhase.Ended) {
		// 		dragging = false;
		// 	}
		// }
	}

	private void FixedUpdate() {
		if(dragging) {
			float x = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
			float y = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

			sphere.AddTorque(Vector3.down * x);
			sphere.AddTorque(Vector3.right * y);
		}
	}
}
