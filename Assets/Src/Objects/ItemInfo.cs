using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour {
    public static ItemInfo instance;
    [SerializeField] RectTransform infoPanel;
    private void Awake() {
        instance = this;
    }

    private ItemData selectedItem;
    public void SelectItem(ItemData item) {
        selectedItem = item;
        ToggleInfoPanel();
    }

    private void ToggleInfoPanel() {
        infoPanel.DOAnchorPosY(-28.5285f, 0.5f);
        infoPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(selectedItem.name + "\n" + selectedItem.LevelDescription);
    }

    internal void HideInfoPanel() {
        if(infoPanel != null)
            infoPanel.DOAnchorPosY(-141.3f, 0.5f);
    }

    public void _UseItem() {
        // selectedItem.UseItem();
        InventorySystem.instance.UpdateInventoryUI();
    }
}
