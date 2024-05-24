using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyData : MonoBehaviour {
    public EntityStatData characterData;
    private int currentLevel;
    private int currentToolLevel;
    private void Awake() {
        currentLevel = characterData.level;
        currentToolLevel = characterData.toolLevel;
    }
    
    private void Update() {
        if(currentLevel != characterData.level) {
            currentLevel = characterData.level;
            StartCoroutine(NoticePanel.instance.ShowNotice(characterData.name.Replace("Stats", "") + " has levelled up!"));
        }

        if(currentToolLevel != characterData.toolLevel) {
            currentToolLevel = characterData.toolLevel;
            StartCoroutine(NoticePanel.instance.ShowNotice(characterData.name.Replace("Stats", "") + "'s tool improved!"));
        }
    }
}
