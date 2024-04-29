using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour, ISaveLoad {
    [SerializeField] TextMeshProUGUI startText;
    private bool newGame;
    private string nextScene;

    private void Start() {
        if(newGame) {
            startText.SetText("* Begin Story *");
            nextScene = "Cutscene";
        }
        else 
            startText.SetText("* Continue Story *");
    }

    public void _StartGame() {
        SceneHandler.instance.LoadScene(nextScene);
    }

    public void _Options() {}

    public void _Credits() {}

    //TODO: prettify main menu

    public void Save(DataRoot data) {
        // data.userData.newgame = newgame;
    }

    public void Load(DataRoot data) {
        newGame = data.gameData.newGame;
        nextScene = data.gameData.lastScene;
    }
}
