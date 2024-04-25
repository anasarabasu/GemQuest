using UnityEngine;

public class Collectible : MonoBehaviour {

    public void DropLoot() {
        
    }

    private void OnTriggerEnter2D(Collider2D player) {
        Destroy(this.gameObject);
    }
}
