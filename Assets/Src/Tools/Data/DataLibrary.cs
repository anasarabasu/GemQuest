using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class DataRoot {
    public GameData gameData;
    public OverworldData overworldData;
    public LevelData levelData;
    public CharacterStats characterStats;
    public List<Item> InventoryData;
}

[Serializable] public class GameData { 
    public GameData() { //default values
        newGame = true;
        dialogueIndex = 0;
        chapter = 1;
        teamComposition = new List<string> {"Pik", "Hels"};
        showFPS = true;
    }
    
    public bool newGame;
    public int dialogueIndex;
    public int chapter;
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
    }
    public Dictionary<string, bool> lvl1_ObstaclePersistence;
    public GameObject[] lvl2_ObstaclePersistence;

    // public Dictionary<GameObject, bool> destroyablesStatePersistence;
    public Vector2 levelCoordinates;
}

[SerializeField] public class CharacterStats {

}
    

    //pik stats
    //mac stats
    //alys stats
    //bom stats

    //inventory
