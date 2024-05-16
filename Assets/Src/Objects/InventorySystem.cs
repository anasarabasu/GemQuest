using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class InventorySystem : MonoBehaviour, ISaveable {
    [SerializeField] GameObject InventoryPanel;
    [SerializeField] GameObject InfoPanel;
    [SerializeField] GameObject InventorySlotPrefab;

    public static InventorySystem instance;

    public List<Item> inventoryContents = new();

    private void Awake() {
        instance = this;
        
    }

    private List<ItemFunction> GatherItems() {
        IEnumerable<ItemFunction> obj = FindObjectsOfType<MonoBehaviour>().OfType<ItemFunction>();
        return new List<ItemFunction>(obj);
    }

    internal void AddItem(Item item) {
        bool itemExists = false;
        foreach (Item inventoryItem in inventoryContents) {
            if(inventoryItem.itemName == item.itemName) {
                 inventoryItem.amount += item.amount;
                itemExists = true;
            }
        }
        if(!itemExists)
            inventoryContents.Add(item);

        UpdateInventoryUI();
    }

    private void UpdateInventoryUI() {
        foreach (Transform item in InventoryPanel.GetComponentInChildren<Transform>()) 
            Destroy(item.gameObject);

        foreach (Item item in inventoryContents) {
            RectTransform itemSlot = Instantiate(InventorySlotPrefab, InventoryPanel.transform).GetComponent<RectTransform>();

            itemSlot.GetComponent<ItemFunction>().itemName = item.itemName;
            itemSlot.GetComponent<ItemFunction>().itemDescription = item.description;

            itemSlot.GetComponentInChildren<Image>().sprite = item.sprite;
            itemSlot.GetComponentInChildren<Image>().color = item.color;

            if(item.amount > 1)
                itemSlot.GetComponentInChildren<TextMeshProUGUI>().SetText(item.amount.ToString());
            else
                itemSlot.GetComponentInChildren<TextMeshProUGUI>().SetText("");
        }
    }

    private bool showInventory = false;
    public void _ToggleInventory() {        
        if(!showInventory) {
            InventoryPanel.transform.DOLocalMoveY(30, 0.5f);
            showInventory = true;
            Joystick.MovementState(false);
        }
        else {
            InventoryPanel.transform.DOLocalMoveY(150, 0.5f);
            showInventory = false;

            InfoPanel.transform.DOLocalMoveY(150, 0.5f);
            Joystick.MovementState(true);
        }  
    }

    Item selectedItem;
    public void ShowInfo(string itemName, string itemDescription) {
        InfoPanel.transform.DOLocalMoveY(30, 0.5f);
        InfoPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(itemName + "\n" + itemDescription);

        
    }

    public void Save(DataRoot data) {
    }

    public void Load(DataRoot data) {
    }
}

/**

- Title menu revamp
- Intro cutscene concept*

- Dialogue revamp

- Inventory System
- UI redesign

**/