using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelObject : MonoBehaviour { //should make this an inherit or interface... nah
    [SerializeField] int levelRequired = 1;
    [SerializeField] GameObject DropPrefab;
    [SerializeField] ItemData theCommonRock;
    [SerializeField] List<ItemData> itemDropList_LVL1;
    [SerializeField] List<ItemData> itemDropList_LVL2;
    
    ItemData GetDroppedItems() {
        int randomNum = Random.Range(1, 101); //generic rock has 100 drop so no null if ever
        
        List<ItemData> possibleItems = new List<ItemData>();
        if(SceneManager.GetActiveScene().name == "Level1") {
            foreach (var item in itemDropList_LVL1) {
                if(randomNum <= item.dropChance)
                    possibleItems.Add(item);
            }
        }
        if(SceneManager.GetActiveScene().name == "Level2") {
            foreach (var item in itemDropList_LVL2) {
                if(randomNum <= item.dropChance)
                    possibleItems.Add(item);
            }
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
        itemDropOfThisObject = GetDroppedItems();

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

    public void GetMined(int pickaxeLevel) {
        transform.DOShakePosition(1, 0.25f);

        if(pickaxeLevel >= levelRequired) {
            hitPoints -= 1;
        }
        else {
            if(notice != null)
                StopCoroutine(notice);
                
            notice = StartCoroutine(NoticePanel.instance.ShowNotice("Your tool is to weak to mine this rock..."));
        }
    }
    Coroutine notice;

    private void DropLoot() {
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds; //update party AI
        bounds.Expand(2);
        AstarPath.active.UpdateGraphs(bounds);

        for(int i = 0; i < Random.Range(1, 5); i++) { //rocks lol
            GameObject itemDrop = Instantiate(DropPrefab, transform.position, new Quaternion(0, 0, Random.rotation.z, 1), transform.parent);
            itemDrop.name = theCommonRock.name;
            itemDrop.GetComponent<SpawnItemDrop>().itemData = theCommonRock;
        }

        for(int i = 0; i < Random.Range(1, itemDropOfThisObject.maxLootDrop); i++) {
            GameObject itemDrop = Instantiate(DropPrefab, transform.position, new Quaternion(0, 0, Random.rotation.z, 1), transform.parent);
            itemDrop.transform.localScale = new Vector3(Random.Range(0.4f, 0.6f), Random.Range(0.4f, 0.6f), Random.Range(0.4f, 0.6f));
            itemDrop.name = itemDropOfThisObject.name;
            itemDrop.GetComponent<SpawnItemDrop>().itemData = itemDropOfThisObject;
        }

        ObjectDestroyedPersistence.ObjectDestroyed(gameObject);
        DataManager.instance.WriteSaveFile();
        Destroy(gameObject);
    }
}
