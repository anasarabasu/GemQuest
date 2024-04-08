using UnityEngine;

public class OrbitPhysics: MonoBehaviour {
    [SerializeField] float gravity = -46f;
    public void Attract(Transform player) {
        Vector3 gravityUp = (player.position - this.transform.position).normalized;
        player.rotation = Quaternion.FromToRotation(player.up, gravityUp) * player.rotation;
        player.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
    }
}
