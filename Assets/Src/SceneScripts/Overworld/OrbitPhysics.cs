using UnityEngine;

public class OrbitPhysics: MonoBehaviour {
    [SerializeField] float gravity = -46f;
    
    public void Orbit(Transform player) {
        Vector3 gravityUp = (player.position - transform.position).normalized;
        Quaternion targetUp = Quaternion.FromToRotation(player.up, gravityUp) * player.rotation;

        player.rotation = Quaternion.Slerp(player.rotation, targetUp, 50f * Time.deltaTime);
        player.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
    }
}
