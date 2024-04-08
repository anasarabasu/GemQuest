using TMPro;
using UnityEngine;

public class Cutscene : MonoBehaviour, ISaveLoad {
    [SerializeField] TextMeshProUGUI screenText;
    private bool newGame;
    private string[] dialogueList = {
        "introductory cutscene/ dialogue",
        "bla bla bla",
        "tba",
        "to overworld :)"
    };
    private int dialogueIndex;

    private void Update() {
        screenText.text = dialogueList[dialogueIndex];
    }

    public void _Continue() { //dialogue step incrementor TEMP
        //TODO: if newgame -> intro cutscene | else -> lastsaved scene
        if(dialogueIndex+1 < dialogueList.Length) {
            dialogueIndex++;
        }
        else 
            _FinishCutscene();
        
        Debug.Log(dialogueIndex);
    }

    public void _FinishCutscene() { //toggles newgame to false TEMP
        newGame = false;
        DataManager.instance.SaveFile();
        SceneLoader.instance.SwitchTo("Overworld");
    }

    public void Save(ref DataRoot data) {
        data.userData.newGame = newGame;
        data.userData.dialogueIndex = dialogueIndex;
    }

    public void Load(DataRoot data) {
        newGame = data.userData.newGame;
        dialogueIndex = data.userData.dialogueIndex;
    }
}
