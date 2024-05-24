using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelItemsUseEffect : MonoBehaviour {
    public static LevelItemsUseEffect instance;
    private void Awake() => instance = this;

    [SerializeField] EntityStatData pik;
    [SerializeField] EntityStatData hels;

    // [SerializeField] game
    public void UseItem(ItemData item) {
        string starting = $"Used {item.name}\n";
        switch (item.levelFunction.function) {
            case ItemData.LevelFunction.Function.None:
                StartCoroutine(NoticePanel.instance.ShowNotice(starting + "Nothing happened..."));
                break;

            case ItemData.LevelFunction.Function.ReplenishBattery:
                FlashLightMechanic.instance.AddBattery(item.levelFunction.amount);
                StartCoroutine(NoticePanel.instance.ShowNotice(starting + "Flashlight charged!"));
                InventorySystem.instance._ToggleInventory();
                break;

            case ItemData.LevelFunction.Function.UpgradeTool:
                pik.UpgradeTool(item.levelFunction.amount);
                break;

            case ItemData.LevelFunction.Function.Heal:
                StartCoroutine(NoticePanel.instance.ShowNotice(starting + "not yet added..."));
                break;

            case ItemData.LevelFunction.Function.DeterEnemies:
                StartCoroutine(NoticePanel.instance.ShowNotice(starting + "Things seems a little quiter... for now\nWe stink though!"));
                InventorySystem.instance._ToggleInventory();
                break;
            
        }
        
    }
}
