using UnityEngine;

public class ApplyGravity : MonoBehaviour {
    [SerializeField] OrbitPhysics planet;
    [SerializeField] Rigidbody body;

    private void FixedUpdate() {
        planet.Orbit(body.transform);
    }
}
