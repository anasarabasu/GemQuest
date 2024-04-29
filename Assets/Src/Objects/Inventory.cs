using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour {
    public static Inventory instance;

    [SerializeField] GameObject InventoryPanel;

    private GameObject[] SlotsPanel;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        SlotsPanel = GameObject.FindGameObjectsWithTag("InventorySlots");
        InventoryPanel.SetActive(false);
    }

    public List<string> inventoryContents = new();

    internal void Add(string objectName) {
        inventoryContents.Add(objectName);
        inventoryContents.Sort();
        // UpdateContents();

    }

    public void ToggleInventory(InputAction.CallbackContext context) {

        if(!InventoryPanel.activeInHierarchy)
            InventoryPanel.SetActive(true);
        else
            InventoryPanel.SetActive(false);
        
        Debug.Log("Show Inventory");
    }

    private void UpdateContents() {
        for (int i = 0; i < inventoryContents.Count; i++) {
            SlotsPanel[i].GetComponentInChildren<TextMeshProUGUI>().SetText(inventoryContents[i]);
        }
        // foreach (GameObject slot in SlotsPanel) {
        //     slot.transform.GetComponentInChildren<TextMeshProUGUI>().SetText(inventoryContents[0]);
        // }
    }
}
