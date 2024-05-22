using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugItems : MonoBehaviour {
    [SerializeField] List<ItemData> items;

    public void _GenerateItemsForCheats() {
        foreach(var obj in items) 
            InventorySystem.instance.AddItem(obj);
    }
}
