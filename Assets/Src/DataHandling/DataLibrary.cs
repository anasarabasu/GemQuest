using System;
using UnityEngine;

[Serializable] public class DataRoot {
    public GameData gameData;
    public OverworldData overworldData;
    public LevelData levelData;
    public CharacterStats characterStats;
}

[Serializable] public class GameData { 
    public GameData() { //default values
        newGame = true;
        dialogueIndex = 0;
        chapter = 1;
    }
    
    public bool newGame;
    public int dialogueIndex;
    public int chapter;
    public string lastScene;
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
        PIK_levelCoordinates = new Vector2(0, -1);
        HELS_levelCoordinates = new Vector2(0, -1);
        ISKA_levelCoordinates = new Vector2(0, -1);
        POM_levelCoordinates = new Vector2(0, -1);
    }
    // public ??? levelObstacleChanges;

    public Vector2 PIK_levelCoordinates;
    public Vector2 HELS_levelCoordinates;
    public Vector2 ISKA_levelCoordinates;
    public Vector2 POM_levelCoordinates;
}

[SerializeField] public class CharacterStats {

}
    

    //pik stats
    //mac stats
    //alys stats
    //bom stats

    //inventory
