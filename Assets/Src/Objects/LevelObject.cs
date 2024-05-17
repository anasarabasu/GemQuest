using System;
using DG.Tweening;
using UnityEngine;

public class LevelObject : MonoBehaviour { //should make this an inherit or interface... nah
    
    [SerializeField] ItemData itemData;
    [SerializeField] GameObject DropPrefab;
    
    internal bool isDestroyedPersistent = false;

    private int hitPoints;
    private double halfHP;
    
    private void Awake() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.level.sprite;
        spriteRenderer.color = itemData.color;

        hitPoints = itemData.level.hitPoints;
        halfHP = hitPoints / 2;
    }

    private void Update() {
        if(hitPoints == 0) { //rubble  
            DropLoot();
        }
        else if(hitPoints == Math.Round(halfHP, 0)) //half destroyed sprite
            Debug.Log("half sprite");
    }

    public void GetMined() {
        transform.DOShakePosition(1, 0.25f);
        hitPoints -= 1;
    }

    private void DropLoot() {
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        bounds.Expand(2);
        AstarPath.active.UpdateGraphs(bounds);

        GameObject itemDrop = Instantiate(DropPrefab, transform.position, Quaternion.identity, transform.parent);
        itemDrop.name = gameObject.name;
        itemDrop.GetComponent<SpawnItemDrop>().itemData = itemData;

        Destroy(gameObject);
    }

    private void OnDestroy() {
        isDestroyedPersistent = true;
    }
}
