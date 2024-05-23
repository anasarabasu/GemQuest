using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SpawnItemDrop : MonoBehaviour{
    internal ItemData itemData;

    private void Start() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.sprite;
        
        transform.DOJump(transform.position + Random.insideUnitSphere * 2.5f, 1, 2, 1f);
        StartCoroutine(Wait());

    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, GameObject.FindWithTag("Pik").transform.position, 0.01f);
        
    }

    private void OnTriggerEnter2D(Collider2D collider) { 
        if(collider.CompareTag("Hels")) {
            InventorySystem.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(0.5f);
        tag = "Loot";

    }

}
