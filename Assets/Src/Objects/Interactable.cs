using System;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

public class Interactable : MonoBehaviour { //should make this an inherit or interface
    [SerializeField] int HitPoints;
    [SerializeField] GameObject ItemDropPrefab;
    
    private double halfHP;
    private void Awake() {
        halfHP = HitPoints / 2;
    }

    private void Update() {
        if(HitPoints == 0) { //rubble sprite 
            // AstarPath.active.Scan();
            IsDestroyed();
        }
        else if(HitPoints == Math.Round(halfHP, 0)) //half destroyed sprite
            Debug.Log("half sprite");
            // this.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void GetMined() {
        //particles
        transform.DOShakePosition(1, 0.25f);

        HitPoints -= 1;
    }

    private void IsDestroyed() {
        GameObject itemDrop = Instantiate(ItemDropPrefab, transform.position, Quaternion.identity, transform.parent);
        itemDrop.name = gameObject.name;
        itemDrop.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        itemDrop.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
        itemDrop.transform.localScale = new Vector3(0.5f, 0.5f);

        Destroy(gameObject);
    }

    private void OnDestroy() {
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        bounds.Expand(2);
        AstarPath.active.UpdateGraphs(bounds);
    }
}
