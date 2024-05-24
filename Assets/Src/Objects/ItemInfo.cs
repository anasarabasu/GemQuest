using System.Collections.Generic;
using Aarthificial.Reanimation;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class ItemInfo : MonoBehaviour {
    public static ItemInfo instance;
    [SerializeField] RectTransform infoPanel;
    private void Awake() => instance = this;


    public float timer = 30;
    private void Update() {
        if(itemsSold >= 10) {
            timer -= Time.deltaTime;

            if(timer <= 0) {
                itemsSold = 0;
                timer = 30;
            }
        }
    }

    private ItemData selectedItem;
    public void SelectItem(ItemData item) {
        selectedItem = item;
        ToggleInfoPanel();
    }

    private void ToggleInfoPanel() {
        infoPanel.DOAnchorPosY(-7.949f, 0.5f);


        infoPanel.Find("Name").GetComponent<TextMeshProUGUI>().SetText(selectedItem.name);
        UpdateTextColour();

        infoPanel.Find("Intro").GetComponent<TextMeshProUGUI>().SetText(selectedItem.introDesccription);

        if(selectedItem.description.unlockedLevelDescription) 
            infoPanel.Find("Level").GetComponent<TextMeshProUGUI>().SetText(selectedItem.description.Level);
        else
            infoPanel.Find("Level").GetComponent<TextMeshProUGUI>().SetText("...");


        if(selectedItem.description.unlockedCombatDescription_ITEM)
            infoPanel.Find("Combat1").GetComponent<TextMeshProUGUI>().SetText(selectedItem.description.Combat_ITEM);
        else
            infoPanel.Find("Combat1").GetComponent<TextMeshProUGUI>().SetText("...");

        if(selectedItem.description.unlockedCombatDescription_SKILL)
            infoPanel.Find("Combat2").GetComponent<TextMeshProUGUI>().SetText(selectedItem.description.Combat_SKILL);
        else
            infoPanel.Find("Combat2").GetComponent<TextMeshProUGUI>().SetText("...");
    }

    Color textColour = Color.black;
    private void UpdateTextColour() {
        if(selectedItem.levelFunction.unlockedPriceRank) 
            switch (selectedItem.levelFunction.priceRank) {
                case ItemData.LevelFunction.PriceRank.Low:
                    ColorUtility.TryParseHtmlString("#48A448", out textColour);
                    break;
                case ItemData.LevelFunction.PriceRank.Mid:
                    ColorUtility.TryParseHtmlString("#1C1CBA", out textColour);
                    
                    break;
                case ItemData.LevelFunction.PriceRank.High:
                    ColorUtility.TryParseHtmlString("#D42DD4", out textColour);
                    break;
                case ItemData.LevelFunction.PriceRank.Zero:
                    textColour = Color.black;
                    break;                    
            }
        else
            textColour = Color.black;

        infoPanel.Find("Name").GetComponent<TextMeshProUGUI>().color = textColour;
    }

    public void _ShowWhyTextIsThatColour() {
        if(selectedItem.levelFunction.unlockedPriceRank)
            switch (selectedItem.levelFunction.priceRank) {
                case ItemData.LevelFunction.PriceRank.Low:
                    StartCoroutine(NoticePanel.instance.ShowNotice("This mineral doesn't sell much"));
                    break;
                case ItemData.LevelFunction.PriceRank.Mid:
                    StartCoroutine(NoticePanel.instance.ShowNotice("A very useful mineral with an average price"));
                    break;
                case ItemData.LevelFunction.PriceRank.High:
                    StartCoroutine(NoticePanel.instance.ShowNotice("A high value nad demand mineral that sells at a high price"));
                    break;
                default:
                    StartCoroutine(NoticePanel.instance.ShowNotice("What do you expect the generic rock to sell for?"));
                    return;                    
            }
        else 
            StartCoroutine(NoticePanel.instance.ShowNotice("Don't know how much this sells..."));
    }

    internal void HideInfoPanel() {
        if(infoPanel != null)
            infoPanel.DOAnchorPosY(-160.1f, 0.5f);
    }

    private EntityStatData ChooseRandomPartyMember() {
        var currentParty = FindObjectsOfType<PartyData>();
        return currentParty[Random.Range(0, currentParty.Length)].characterData;
    }

    public int itemsSold;
    public void _SellItem() {
        if(selectedItem) {
            EntityStatData randomParty = ChooseRandomPartyMember();
            if(randomParty.currentEnergy < randomParty.energy) {
                switch (selectedItem.levelFunction.priceRank) {
                    case ItemData.LevelFunction.PriceRank.Low:
                        ChooseRandomPartyMember().currentEnergy += 5;
                        break;
                    case ItemData.LevelFunction.PriceRank.Mid:
                        ChooseRandomPartyMember().currentEnergy += 12;
                        break;
                    case ItemData.LevelFunction.PriceRank.High:
                        ChooseRandomPartyMember().currentEnergy += 18;
                        break;
                }
                if(randomParty.currentEnergy >= randomParty.energy)
                    randomParty.currentEnergy = randomParty.energy;
                
                StartCoroutine(NoticePanel.instance.ShowNotice($"{selectedItem.name} sold!  \nThe heavier wallet energised {randomParty.name.Replace("Stats", "")}!"));

            }
            else {
                StartCoroutine(NoticePanel.instance.ShowNotice($"Sold {selectedItem.name}!"));
            }
            itemsSold++;

            selectedItem.inventoryAmount--;
            selectedItem.levelFunction.unlockedPriceRank = true;
            
            UpdateTextColour();
            
            InventorySystem.instance.UpdateInventoryUI();
        }
        else 
            StartCoroutine(NoticePanel.instance.ShowNotice("Select a mineral to sell first!"));
    }

    public void _UseItem() {
        if(selectedItem) {
            InventorySystem.instance._ToggleInventory();
            selectedItem.UseItem_LEVEL();
            selectedItem.description.unlockedLevelDescription = true;
            selectedItem.inventoryAmount--;

            StartCoroutine(NoticePanel.instance.ShowNotice($"Used {selectedItem.name}"));
            InventorySystem.instance.UpdateInventoryUI();
        }
        // selectedItem.UseItem();
    }
}
