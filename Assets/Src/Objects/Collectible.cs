using DG.Tweening;
using UnityEngine;

public class Collectible : MonoBehaviour {

    private void Start() {
        gameObject.tag = "Loot";

        Vector3 dropDirection = new Vector2(Random.Range(-1, 1), Random.Range(0, 1));
        transform.DOJump(transform.position + dropDirection, 1, 2, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Hels")) {
            Inventory.instance.Add(gameObject.name);
            Destroy(gameObject);
        }
    }
}
