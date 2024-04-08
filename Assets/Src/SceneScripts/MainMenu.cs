using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour, ISaveLoad {
    [SerializeField] TextMeshProUGUI startText;
    private bool newGame;
    private string nextScene;

    private void Start() {
        if(newGame) {
            startText.text = "* Begin Story *";
            nextScene = "Cutscene";
        }
        else {
            startText.text = "* Continue Story *";
            nextScene = "Overworld";
        }
    }

    public void Save(ref DataRoot data) {
        // data.userData.newgame = newgame;
    }

    public void Load(DataRoot data) {
        newGame = data.userData.newGame;
    }

    public void _StartGame() {
        SceneLoader.instance.SwitchTo(nextScene);
    }

    public void _Options() {}

    public void _Credits() {}

    //TODO: prettify main menu


}
