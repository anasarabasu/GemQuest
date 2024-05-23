using System.Collections.Generic;
using UnityEngine;

public class DebugItems : MonoBehaviour {
    [SerializeField] List<ItemData> items;

    public void _GenerateItemsForCheats() {
        foreach(var obj in items) 
            InventorySystem.instance.AddItem(obj);
    }
}
