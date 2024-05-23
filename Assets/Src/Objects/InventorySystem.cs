using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour, ISaveable {
    public static InventorySystem instance;
    private void Awake() => instance = this;

    public List<ItemData> inventoryContents = new();
    internal void AddItem(ItemData item) {
        bool itemExists = false;
        item.inventoryAmount ++;

        foreach (ItemData existingItem in inventoryContents) {
            if(existingItem == item) {
                itemExists = true;
            }
        }
        if(!itemExists) 
            inventoryContents.Add(item);

        UpdateInventoryUI();
    }

    [SerializeField] GameObject InventorySlotPrefab;
    [SerializeField] RectTransform InventoryPanel;
    public void UpdateInventoryUI() {
        ItemData itemToDelete = null;

        foreach (Transform reloadItem in InventoryPanel.GetComponentInChildren<Transform>()) {
            if(reloadItem.gameObject.name == "Starting Item")
                continue;
            Destroy(reloadItem.gameObject);
        }

        foreach (ItemData item in inventoryContents) {
            GameObject itemSlot = Instantiate(InventorySlotPrefab, InventoryPanel);
            itemSlot.GetComponent<FocusItem>().itemData = item;
            itemSlot.GetComponentInChildren<Image>().sprite = item.sprite;

            if(item.inventoryAmount > 1)
                itemSlot.GetComponentInChildren<TextMeshProUGUI>().SetText(item.inventoryAmount.ToString());
            else if(item.inventoryAmount == 1)
               itemSlot.GetComponentInChildren<TextMeshProUGUI>().SetText("");
            else {
                itemToDelete = item;
                Destroy(itemSlot);
                // ItemInfo.instance.HideInfoPanel();
            }
        }

        inventoryContents.Remove(itemToDelete);
    }
    [SerializeField] RectTransform movepanel;
    private bool showInventory = false;
    public void _ToggleInventory() {  
        if(!showInventory) {
            movepanel.DOAnchorPosX(0, 0.5f);
            showInventory = true;
            Joystick.MovementState(false); 
        }
        else {
            movepanel.DOAnchorPosX(207.6f, 0.5f);
            showInventory = false;

            ItemInfo.instance.HideInfoPanel();
            Joystick.MovementState(true);
        }  
    }

    public void Save(DataRoot data) {
        List<int> inventoryID = new();
        foreach (var item in inventoryContents) 
            inventoryID.Add(item.GetInstanceID());
        
        data.inventoryData = inventoryContents;
    }

    public void Load(DataRoot data) {
        inventoryContents = data.inventoryData; 
        UpdateInventoryUI();
    }
}