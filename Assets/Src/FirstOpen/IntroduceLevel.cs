using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IntroduceLevelControls : MonoBehaviour, ISaveable {
    public bool hasIntroducedScene;

    private void Start() {
        if(hasIntroducedScene)
            return;

        StartCoroutine(IntroduceScene());
    }
    

    [SerializeField] RectTransform tutorialPanel;

    public bool joystickDiscovered;
    public void _Joystick() => joystickDiscovered = true;
    [SerializeField] RectTransform joystick;


    public bool mineButtonDiscovered;
    public void _Mine() => mineButtonDiscovered = true;
    [SerializeField] RectTransform mine;

    [SerializeField] RectTransform statBar;


    public bool inventoryButtonDiscovered;
    public void _Inventory() => inventoryButtonDiscovered = true;
    [SerializeField] RectTransform inventory;


    [SerializeField] RectTransform useItemFake;
    [SerializeField] GameObject useItem;


    [SerializeField] RectTransform sellItemFake;
    [SerializeField] GameObject sellItem;


    public bool enemyDiscovered;
    [SerializeField] GameObject enemyAmbush;

    float moveSpeed = 0.5f;

    IEnumerator IntroduceScene() {
        joystick.anchoredPosition = new Vector2(-55.78342f, joystick.anchoredPosition.y); //make it so that its out of the viewing cam then ease them in at the end
        mine.anchoredPosition = new Vector2(40.47557f, mine.anchoredPosition.y);
        statBar.anchoredPosition = new Vector2(statBar.anchoredPosition.x, 40.16111f);
        inventory.anchoredPosition = new Vector2(27.19112f, inventory.anchoredPosition.y);
        
        useItem.SetActive(false);
        useItemFake.gameObject.SetActive(true);

        sellItem.SetActive(false);
        sellItemFake.gameObject.SetActive(true);

        enemyAmbush.SetActive(false);

        WaitForSeconds wait = new WaitForSeconds(0.5f);
        TextMeshProUGUI tutorialPanelText = tutorialPanel.GetComponentInChildren<TextMeshProUGUI>();

        tutorialPanel.DOAnchorPosX(-11.847f, moveSpeed);
        tutorialPanelText.SetText("Have a look around.");
        joystick.DOAnchorPosX(48, moveSpeed);
        yield return new WaitUntil(() => joystickDiscovered);
        yield return wait;

        tutorialPanelText.SetText("See those rocks and crates?\nYou can destroy them by holding the pickaxe.");
        mine.DOAnchorPosX(-60.76563f, moveSpeed);
        yield return new WaitUntil(() => mineButtonDiscovered);
        yield return wait;

        tutorialPanel.DOAnchorPosY(-24.27527f, 0.25f);
        tutorialPanelText.SetText("Make sure to keep track of your energy and take a moment to rest.");
        statBar.DOAnchorPosY(9.5f, moveSpeed);
        yield return new WaitUntil(() => InventorySystem.instance.inventoryContents.Count >= 1);
        yield return new WaitForSeconds(5);

        tutorialPanelText.SetText("You can click on the backpack to view the items you've gathered.");
        joystick.DOAnchorPosX(-55.78342f, moveSpeed);
        inventory.DOAnchorPosX(-23.0791f, moveSpeed);
        yield return new WaitUntil(() => inventoryButtonDiscovered);
        yield return wait;

        tutorialPanelText.SetText("Click on the item to see some information about it."); //we should have a starting item
        inventory.DOAnchorPosX(27.19112f, moveSpeed);
        yield return new WaitForSeconds(5);

        tutorialPanelText.SetText("We don't know what these rocks are for.\nBut if you use it then you have a chance of finding out."); //change this kasi di makita ung text
        useItemFake.DOAnchorPosX(-20.98767f, moveSpeed);
        yield return new WaitForSeconds(5);

        tutorialPanelText.SetText("Its function varies on where it is used: In level or in combat.");
        yield return new WaitForSeconds(5);

        tutorialPanelText.SetText("You can also sell the minerals you've collected. Don't know how much each the ores are though. Maybe you can try selling one to find out");
        sellItemFake.DOAnchorPosX(-20.98767f, moveSpeed);
        yield return new WaitForSeconds(5);

        InventorySystem.instance._ToggleInventory();
        useItem.SetActive(true);
        useItemFake.gameObject.SetActive(false);
        sellItem.SetActive(true);
        sellItemFake.gameObject.SetActive(false);

        tutorialPanelText.SetText("Its up to you to collect them and discover their different functions.");
        joystick.DOAnchorPosX(48, moveSpeed);
        inventory.DOAnchorPosX(-23.0791f, moveSpeed);
        yield return new WaitForSeconds(5);

        tutorialPanel.DOAnchorPosX(-261.1f, 0.5f);
        enemyAmbush.SetActive(true);
        hasIntroducedScene = true;
        DataManager.instance.WriteSaveFile();
        yield break;
    }
    // [SerializeField] ItemData copper;
    // public void CheckForCopper() {
    //     if(InventorySystem.instance.inventoryContents.Contains(copper))
    //         return;
        
    //     InventorySystem.instance.AddItem(copper);
    // }

    public void Save(DataRoot data) {
        data.gameData.hasIntroducedLevelControls = hasIntroducedScene;
    }

    public void Load(DataRoot data) {
        hasIntroducedScene  = data.gameData.hasIntroducedLevelControls;
    }
}
