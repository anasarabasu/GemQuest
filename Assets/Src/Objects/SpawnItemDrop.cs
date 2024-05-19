using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpawnItemDrop : MonoBehaviour{
    internal ItemData itemData;

    private void Start() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.inventory.sprite;
        
        Vector3 dropDirection = new Vector2(Random.Range(-1, 1), Random.Range(0, 1));
        transform.DOJump(transform.position + dropDirection, 1, 2, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collider) { 
        if(collider.CompareTag("Hels")) {
            InventorySystem.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
