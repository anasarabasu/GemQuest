using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {
    public static CombatUI instance;
    private void Awake() => instance = this;

    [SerializeField] RectTransform SIRPanel;
    [SerializeField] float SIRPanelShow;
    [SerializeField] float SIRPanelHide;
    [SerializeField] float SIRPSpeed = 0.35f;
    bool SIRSelectionToggle;
    public void _ToggleSIRSelectionPanel() {
        if(!SIRSelectionToggle) {
            SIRPanel.DOAnchorPosX(SIRPanelShow, SIRPSpeed);
            SIRSelectionToggle = true;
        }
        else {
            SIRPanel.DOAnchorPosX(SIRPanelHide, SIRPSpeed);
            SIRSelectionToggle = false;
        }
    }

    internal void UpdateLabelsToEntity(Combat entity) {
        skillPanel.Find("Character Focus Stats").GetComponent<StatBarUI>().UpdateEntityFocusPanel(entity.combatData);
        Button[] panels = skillPanel.Find("Moveset").GetComponentsInChildren<Button>();
        for (int i = 0; i < entity.combatData.movesets.Length; i++) {
            panels[i].gameObject.name = entity.combatData.movesets[i].name;
            panels[i].GetComponentInChildren<TextMeshProUGUI>().SetText(entity.combatData.movesets[i].name);
        }
    }

    [SerializeField] RectTransform skillPanel;
    [SerializeField] float skillPanelShow;
    [SerializeField] float skillPanelHide;
    [SerializeField] float skillPSpeed = 0.35f;
    bool skillToggle;
    public void _ToggleSkillPanel() {
        if(!skillToggle) {
            _ToggleSIRSelectionPanel();
            skillPanel.DOAnchorPosX(skillPanelShow, skillPSpeed);
            skillToggle = true;
        }
        else {
            if(selectedButton != null) {
                selectedButton.DOLocalMoveX(selectedButton.transform.localPosition.x - selectedButtonMoveOffset, skillPSpeed);
                selectedButton.GetComponent<Image>().color = Color.white;
                selectedButton.transform.localScale = Vector3.one;
                selectedButton = null;
            }
            noticePanel.DOAnchorPosY(-12.2f, 0.5f);
            skillPanel.DOAnchorPosX(skillPanelHide, skillPSpeed);
            skillToggle = false;

        }
    }
    Transform selectedButton;
    float selectedButtonMoveOffset = 20;
    [SerializeField] float buttonMoveSpeed = 0.2f;
    [SerializeField] float buttonScaleAmount = 0.25f;
    [SerializeField] float buttonScaleSpeed = 0.5f;
    public void _SelectSkillButton(Transform button) {
        if(selectedButton != null) {
            if(selectedButton == button) {
                selectedButton.DOPunchScale(new Vector3(buttonScaleAmount, -buttonScaleAmount), buttonScaleSpeed, 1, 0);
                return;
            }
            noticePanel.DOAnchorPosY(-12.2f, 0.5f);
            selectedButton.transform.localScale = Vector3.one;
            selectedButton.GetComponent<Image>().color = Color.white;
            selectedButton.DOLocalMoveX(selectedButton.transform.localPosition.x - selectedButtonMoveOffset, buttonMoveSpeed);
            selectedButton = button;
            selectedButton.DOLocalMoveX(selectedButton.transform.localPosition.x + selectedButtonMoveOffset, buttonMoveSpeed);
        }
        else {
            selectedButton = button;
            selectedButton.DOLocalMoveX(selectedButton.transform.localPosition.x + selectedButtonMoveOffset, buttonMoveSpeed);
        }
        selectedButton.GetComponent<Image>().color = Color.white;
    }

    [SerializeField] RectTransform useSkillWithItem;
    public void _ToggleUSWIButton(bool show) {
        if(show) {
            useSkillWithItem.DOAnchorPosX(-67.7f, 0.35f);
        }
        else{
            useSkillWithItem.DOAnchorPosX(78.5f, 0.35f);
        }
    }

    public void _ToggleItemWithSkillKey () {
        
    }

    [SerializeField] float pulsateSpeed;
    public IEnumerator PulsatingSkillButton() {
        while(true) {
        DOVirtual.Color(Color.white, new Color(0.6792453f, 0.6183695f, 0.6651971f, 1), pulsateSpeed, (value) => selectedButton.GetComponent<Image>().color = value);
        yield return new WaitForSeconds(pulsateSpeed);
        DOVirtual.Color(new Color(0.6792453f, 0.6183695f, 0.6651971f, 1), Color.white, pulsateSpeed, (value) => selectedButton.GetComponent<Image>().color = value);
        yield return new WaitForSeconds(pulsateSpeed);
        }
    }

    [SerializeField] RectTransform noticePanel;

    [SerializeField] float cameramoveSpeed = 0.25f;
    [SerializeField] RectTransform itemPanel;
    [SerializeField] float itemPanelMoveSpeed = 0.25f;
    bool itemToggle;
    public void _ToggleItemPanel() {
        if(!itemToggle) {
            SIRPanel.DOAnchorPosX(SIRPanelHide, SIRPSpeed);
            SIRSelectionToggle = false;

            Combat.instance.SearchItemAni();
            InventorySystem.instance.UpdateInventoryUI();

            itemPanel.DOAnchorPosY(0, itemPanelMoveSpeed);
            itemToggle = true;
        }

        else {
            UpdateItemText();
    
            itemPanel.DOAnchorPosY(-167.0197f, itemPanelMoveSpeed);
            itemToggle = false;
        }
    }
    [SerializeField] Transform _camera;
    public void _MoveCameraDown(bool yes) {
        if(yes)
            _camera.DOLocalMoveY(-13.9f, cameramoveSpeed);
        else
            _camera.DOLocalMoveY(0, cameramoveSpeed);
    }

    public void UpdateItemText() {
        itemPanel.Find("Header").Find("Name").GetComponent<TextMeshProUGUI>().SetText("No item selected");
        itemPanel.Find("Description").GetComponent<TextMeshProUGUI>().SetText("Thinking...");
    }

    public void UpdateItemText(ItemData selectedItem, CombatSystem.MoveType moveType) {
        Color textColour = Color.white;
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
                    textColour = Color.white;
                    break;                    
            }
        else
            textColour = Color.white;

        itemPanel.Find("Header").Find("Name").GetComponent<TextMeshProUGUI>().SetText(selectedItem.name);
        itemPanel.Find("Header").Find("Name").GetComponent<TextMeshProUGUI>().color = textColour;


        if(moveType == CombatSystem.MoveType.ITEM)
            if(selectedItem.description.unlockedCombatDescription_ITEM)
                itemPanel.Find("Description").GetComponent<TextMeshProUGUI>().SetText(selectedItem.description.Combat_ITEM);
            else
                itemPanel.Find("Description").GetComponent<TextMeshProUGUI>().SetText("What does this mineral do?");
        
        if(moveType == CombatSystem.MoveType.SKILL)
            if(selectedItem.description.unlockedCombatDescription_SKILL)
                itemPanel.Find("Description").GetComponent<TextMeshProUGUI>().SetText(selectedItem.description.Combat_SKILL);
             else
                itemPanel.Find("Description").GetComponent<TextMeshProUGUI>().SetText("What does this mineral do?");
    }

    [SerializeField] RectTransform itemTargetSelect;
    bool itemTargetSelectToggle;
    public void _ToggleSelectTargetToUseItemOn () {
        if(!itemTargetSelectToggle) {
            itemTargetSelect.DOAnchorPosY(0, 0.5f);
            itemTargetSelectToggle = true;

            _ToggleItemPanel();
        }
        else {
            itemTargetSelect.DOAnchorPosY(-27.83662f, 0.5f);
            itemTargetSelectToggle = false;
        }
    }
}
