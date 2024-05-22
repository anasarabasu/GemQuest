using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour {
    public static ItemInfo instance;
    internal Transform infoPanel;
    private void Awake() {
        instance = this;
        infoPanel = transform;
    }

    private ItemData selectedItem;
    public void SelectItem(ItemData item) {
        selectedItem = item;
        ToggleInfoPanel();
    }

    private void ToggleInfoPanel() {
        infoPanel.DOLocalMoveY(30, 0.5f);
        infoPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(selectedItem.name + "\n" + selectedItem.LevelDescription);
    }

    internal void HideInfoPanel() {
        if(infoPanel != null)
            infoPanel.DOLocalMoveY(150, 0.5f);
    }

    public void _UseItem() {
        // selectedItem.UseItem();
        InventorySystem.instance.UpdateInventoryUI();
    }
}
