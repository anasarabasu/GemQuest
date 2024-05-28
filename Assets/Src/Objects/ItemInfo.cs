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
    [SerializeField] RectTransform viewPort;
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
        viewPort.DOAnchorPosY(-50.019f, 0.5f);


        infoPanel.Find("Name").GetComponent<TextMeshProUGUI>().SetText(selectedItem.name);
        UpdateTextColour();

        infoPanel.Find("Intro").GetComponent<TextMeshProUGUI>().SetText(selectedItem.introDesccription);

        if(selectedItem.description.unlockedLevelDescription) 
            infoPanel.Find("Level").GetComponent<TextMeshProUGUI>().SetText(selectedItem.description.Level);
        else
            infoPanel.Find("Level").GetComponent<TextMeshProUGUI>().SetText("Can this be used here?");


        if(selectedItem.description.unlockedCombatDescription_ITEM)
            infoPanel.Find("Combat1").GetComponent<TextMeshProUGUI>().SetText(selectedItem.description.Combat_ITEM);
        else
            infoPanel.Find("Combat1").GetComponent<TextMeshProUGUI>().SetText("Not sure how this could be used in battle...");

        if(selectedItem.description.unlockedCombatDescription_SKILL)
            infoPanel.Find("Combat2").GetComponent<TextMeshProUGUI>().SetText(selectedItem.description.Combat_SKILL);
        else
            infoPanel.Find("Combat2").GetComponent<TextMeshProUGUI>().SetText("Can it be combined with an attack?");
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
                    StartCoroutine(NoticePanel.instance.ShowNotice("This mineral doesn't sell much", 2));
                    break;
                case ItemData.LevelFunction.PriceRank.Mid:
                    StartCoroutine(NoticePanel.instance.ShowNotice("A very useful mineral with an average price", 2));
                    break;
                case ItemData.LevelFunction.PriceRank.High:
                    StartCoroutine(NoticePanel.instance.ShowNotice("A high value nad demand mineral that sells at a high price", 2));
                    break;
                default:
                    StartCoroutine(NoticePanel.instance.ShowNotice("What do you expect the generic rock to sell for?", 2));
                    return;                    
            }
        else 
            StartCoroutine(NoticePanel.instance.ShowNotice("Don't know how much this sells...", 2));
    }

    internal void HideInfoPanel() {
        if(infoPanel != null) {
            selectedItem = null;
            viewPort.DOAnchorPosY(-300f, 0.5f);
        }
    }

    private EntityStatData ChooseRandomPartyMember() {
        var currentParty = FindObjectsOfType<PartyData>();
        return currentParty[Random.Range(0, currentParty.Length)].characterData;
    }

    public int itemsSold;
    public void _SellItem() {
        if(selectedItem) {
            if(selectedItem.inventoryAmount == 0) {
                selectedItem = null;
                HideInfoPanel();
                return;
            }

            EntityStatData randomParty = ChooseRandomPartyMember();
            switch (selectedItem.levelFunction.priceRank) {

                case ItemData.LevelFunction.PriceRank.Zero:
                    randomParty.IncreaseXP(1);
                    randomParty.currentEnergy += 1;
                    StartCoroutine(NoticePanel.instance.ShowNotice($"Sold {selectedItem.name}!\nI don't know what you were expecting when you sold that rock...", 2));
                    break;

                case ItemData.LevelFunction.PriceRank.Low:
                    randomParty.IncreaseXP(16);
                    randomParty.currentEnergy += 5;
                    StartCoroutine(NoticePanel.instance.ShowNotice($"Sold {selectedItem.name}!\nAlthough it wasn't much, {randomParty.name.Replace("Stats", "")} was still pleased with the profit", 2));
                    break;

                case ItemData.LevelFunction.PriceRank.Mid:
                    randomParty.IncreaseXP(44);
                    randomParty.currentEnergy += 12;
                    StartCoroutine(NoticePanel.instance.ShowNotice($"Sold {selectedItem.name}!\nThe heavier wallet encourages {randomParty.name.Replace("Stats", "")} to continue for more!", 2));
                    break;

                case ItemData.LevelFunction.PriceRank.High:
                    randomParty.IncreaseXP(102);
                    randomParty.currentEnergy += 18;
                    StartCoroutine(NoticePanel.instance.ShowNotice($"Sold {selectedItem.name}!\nMore gems sold means even more money, {randomParty.name.Replace("Stats", "")} needs to go even deeper!", 2));
                    break;
            }
            if(randomParty.currentEnergy >= randomParty.energy)
                randomParty.currentEnergy = randomParty.energy;
                
            itemsSold++;

            selectedItem.inventoryAmount--;
            selectedItem.levelFunction.unlockedPriceRank = true;
            
            UpdateTextColour();
            InventorySystem.instance.UpdateInventoryUI();
        }
        else 
            StartCoroutine(NoticePanel.instance.ShowNotice("Select a mineral to sell first!", 0.5f));
    }

    public void _UseItem() {
        if(selectedItem) {
            if(selectedItem.inventoryAmount == 0) {
                selectedItem = null;
                HideInfoPanel();
                return;
            }
            selectedItem.description.unlockedLevelDescription = true;
            selectedItem.inventoryAmount--;

            LevelItemsUseEffect.instance.UseItem(selectedItem);
            InventorySystem.instance.UpdateInventoryUI();
        }
        else 
            StartCoroutine(NoticePanel.instance.ShowNotice("Select a mineral to use first!", 0.5f));

        // selectedItem.UseItem();
    }
}
