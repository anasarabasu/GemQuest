using System;
using UnityEngine;

[Serializable] public class DataRoot {
    public GameData gameData;
}

[Serializable] public class GameData { 
    public GameData() { //default values
        newGame = true;
        dialogueIndex = 0;
        chapter = 1;
        penaltyWrongCountry = 0;
    }
    
    public bool newGame; // titletext
    public int dialogueIndex;
    public int chapter;
    public int penaltyWrongCountry;
    public Vector3 overworldCoordinates;
    public Vector2 levelCoordinates;

    //show touch 
    //audio
    //sound
    //brightness

    //current chapter
    //cutscene index
    //current world
    //current level
    //player coords

    //pik stats
    //mac stats
    //alys stats
    //bom stats

    //inventory

}

//character stats

//inventory

//settings