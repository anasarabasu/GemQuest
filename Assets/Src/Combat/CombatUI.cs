using DG.Tweening;
using UnityEngine;

public class CombatUI : MonoBehaviour {
    [SerializeField] CombatSystem combatSystem;
    [SerializeField] GameObject choicePanel;

    private void Update() {
        if(combatSystem.waitingForPlayerInput)
            choicePanel.transform.DOLocalMoveY(-64, 0.5f);
        else
            choicePanel.transform.DOLocalMoveY(-100, 0.5f);
    }

    private void UpdateChoicePanelUI() {
        
    }
}
