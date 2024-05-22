using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelObject : MonoBehaviour { //should make this an inherit or interface... nah
    
    [SerializeField] GameObject DropPrefab;
    [SerializeField] List<ItemData> itemDropList;
    
    ItemData GetDroppedItem() {
        int randomNum = Random.Range(1, 101); //generic rock has 100 drop so no null if ever
        
        List<ItemData> possibleItems = new List<ItemData>();
        foreach (var item in itemDropList) {
            if(randomNum <= item.dropChance)
                possibleItems.Add(item);
        }

        if(possibleItems.Count > 0) {
            ItemData loot = possibleItems[Random.Range(0, possibleItems.Count)];
            return loot;
        }
        return null;
    }
    
    public int hitPoints = 1;
    private double halfHP;
    [SerializeField] int lootDropAmount = 1;
    
    ItemData itemDropOfThisObject;
    private void Awake() {
        itemDropOfThisObject = GetDroppedItem();

        hitPoints = itemDropOfThisObject.hitPoints; //maybe increase this depending on the size of sprite
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
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds; //update party AI
        bounds.Expand(2);
        AstarPath.active.UpdateGraphs(bounds);

        for(int i = 0; i < lootDropAmount; i++) {
            GameObject itemDrop = Instantiate(DropPrefab, transform.position, Quaternion.identity, transform.parent);
            itemDrop.name = gameObject.name;
            itemDrop.GetComponent<SpawnItemDrop>().itemData = itemDropOfThisObject;
        }

        ObjectDestroyedPersistence.ObjectDestroyed(gameObject);
        DataManager.instance.WriteSaveFile();
        Destroy(gameObject);
    }
}
