using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class DataRoot {
    public GameData gameData;
    public OverworldData overworldData;
    public LevelData levelData;
    public List<ItemData> inventoryData;

    public CharacterStats characterStats;
}

[Serializable] public class GameData { 
    public GameData() { //default values
        newGame = true;
        dialogueIndex = 0;
        teamComposition = new List<string> {"Pik", "Hels"};
        showFPS = true;
    }
    
    public bool newGame;
    public int dialogueIndex;
    public string lastScene;
    public List<string> teamComposition;
    public bool showFPS;
}

[Serializable] public class OverworldData {
    public OverworldData() {
        penaltyWrongCountry = 0;
        overworldCoordinates = new Vector3(0, 0, -6.88f);

    }

    public Vector3 overworldCoordinates;
    public int penaltyWrongCountry;
}

[Serializable] public class LevelData {
    public LevelData() {
        levelCoordinates = new Vector2(0, -1);
        currentLevel = 1;
    }

    public int currentLevel;
    public Vector2 levelCoordinates;
    public List<GameObject> lvl1ObjectPersistence;
    // public GameObject[] lvl2ObjectPersistence;

}

[SerializeField] public class CharacterStats {

}
    

    //pik stats
    //mac stats
    //alys stats
    //bom stats

    //inventory
