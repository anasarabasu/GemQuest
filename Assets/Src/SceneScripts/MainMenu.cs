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
        else {
            startText.SetText("* Continue Story *");
            nextScene = "Overworld";
        }
    }

    public void _StartGame() {
        SceneManager.instance.LoadScene(nextScene);
    }

    public void _Options() {}

    public void _Credits() {}

    //TODO: prettify main menu

    public void Save(ref DataRoot data) {
        // data.userData.newgame = newgame;
    }

    public void Load(DataRoot data) {
        this.newGame = data.gameData.newGame;
    }
}
