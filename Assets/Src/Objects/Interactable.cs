using System;
using UnityEngine;

public class Interactable : MonoBehaviour { //should make this an inherit or interface
    [SerializeField] int HitPoints;
    [SerializeField] GameObject ItemDrop;

    private double halfHP;
    private void Awake() {
        halfHP = HitPoints / 2;
    }

    private void Update() {
        if(HitPoints == Math.Round(halfHP, 0)) //half destroyed sprite
            this.GetComponent<SpriteRenderer>().color = Color.red;
        else if(HitPoints == 0) //rubble sprite
            IsDestroyed();
    }

    public void GetMined() {
        iTween.ShakePosition(this.gameObject, new Vector3(0.75f, 0.75f), 0.05f);
        HitPoints -= 1;
    }

    private void IsDestroyed() {
        Instantiate(ItemDrop, this.transform.position, Quaternion.identity, this.transform.parent);
        Destroy(this.gameObject);
    }


    private void OnDestroy() {
        Debug.Log("destroyed");
    }
}
