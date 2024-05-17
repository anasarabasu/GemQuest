using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {
    public static CombatUI instance;
    private void Awake() => instance = this;

    [SerializeField] Transform choicePanel;
    bool choiceToggle;
    public void ToggleChoicePanel() {
        if(!choiceToggle) {
            choicePanel.DOLocalMoveY(0, 0.5f);
            choiceToggle = true;
        }
        else {
            choicePanel.DOLocalMoveY(-30, 0.5f);
            choiceToggle = false;
        }
    }
    [SerializeField] Transform cam;
    bool cameraMove;
    private void MoveCamera() {
        if(!cameraMove) {
            cam.DOLocalMoveX(-13.2f, 0.5f);
            cameraMove = true;
        }
        else {
            cam.DOLocalMoveX(0, 0.5f);
            cameraMove = false;
        }
    }

    [SerializeField] Transform skillPanel;
    internal void UpdateSkillLabels(CombatStats entity) {
        skillPanel.Find("PlayerStats").Find("Name").GetComponent<TextMeshProUGUI>().SetText(entity.characterName);
        Button[] panels = skillPanel.Find("Skills").GetComponentsInChildren<Button>();

        for (int i = 0; i < entity.attackTypes.Length; i++) {
            panels[i].gameObject.name = entity.attackTypes[i].name;
            panels[i].GetComponentInChildren<TextMeshProUGUI>().SetText(entity.attackTypes[i].name);
        }
    }
    bool skillToggle;
    public void _ToggleSkillPanel() {
        if(!skillToggle) {
            ToggleChoicePanel();
            CombatSystem.instance.SetMoveType(CombatSystem.MoveType.Skill);
            // MoveCamera();
            
            skillPanel.DOLocalMoveX(-78.3f, 0.5f);
            skillToggle = true;
        }
        else {
            // MoveCamera();
            skillPanel.DOLocalMoveX(-263.2f, 0.5f);
            skillToggle = false;
        }
    }

    [SerializeField] Transform itemPanel;
    bool itemToggle;
    internal void ToggleUseItemButton() => itemPanel.Find("Button").DOLocalMoveY(-58.5f, 0.5f);

    public void _ToggleItemPanel() {
        if(!itemToggle) {
            ToggleChoicePanel();
            CombatSystem.instance.SetMoveType(CombatSystem.MoveType.Item);
            InventorySystem.instance.UpdateInventoryUI();

            itemPanel.DOLocalMoveY(0, 0.5f);
            itemToggle = true;
        }

        else {
            itemPanel.Find("Button").DOLocalMoveY(-95, 0.5f);
            itemPanel.DOLocalMoveY(-100, 0.5f);
            itemToggle = false;
        }
    }

}
