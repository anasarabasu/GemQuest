using System;

[Serializable]
public class DataRoot {
    public UserData userData;
    public GamePlayData gamePlayData;
}

[Serializable] 
public class UserData { //user specific
    public UserData() { //default values
        newGame = true;
    }
    public bool newGame; // titletext
    public int dialogueIndex;

    //show touch
    //audio
    //sound
    //brightness
}

[Serializable]
public class GamePlayData {
    public GamePlayData() { //default values
    }
    
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

