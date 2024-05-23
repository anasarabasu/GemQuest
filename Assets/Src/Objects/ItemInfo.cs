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
        if(selectedItem.unlockLevelDescription)
            infoPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(selectedItem.name + "\n" + selectedItem.LevelDescription);
        else
            infoPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(selectedItem.name + "\n" + "We don't know what this mineral is for...");
    }

    internal void HideInfoPanel() {
        if(infoPanel != null)
            infoPanel.DOAnchorPosY(-141.3f, 0.5f);
    }

    public void _UseItem() {
        if(selectedItem) {
            ToggleInfoPanel();
            HideInfoPanel();
            selectedItem.UseItem_LEVEL();
            NoticePanel.instance.ShowNotice($"Used {selectedItem.name}");
            selectedItem.unlockLevelDescription = true;
            infoPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(selectedItem.name + "\n" + selectedItem.LevelDescription);
        }
        // selectedItem.UseItem();
        InventorySystem.instance.UpdateInventoryUI();
    }
}
