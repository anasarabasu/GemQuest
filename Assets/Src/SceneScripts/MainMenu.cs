using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, ISaveable {
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] Transform settingsPanel;

    private bool newGame;
    private string nextScene;
    AsyncOperation introCutscene;

    private void Start() {
        if(newGame) {
            startText.SetText("* Begin Story *");
            nextScene = "Cutscene";

        }
        else {
            startText.SetText("* Continue *");
            nextScene = "Level1";
        }
    }

    public void _StartGame() {
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
    }

    private bool settingsToggle = false;
    public void _ToggleSettings() {
        if(!settingsToggle) {
            settingsPanel.DOLocalMoveY(-28.9f, 1);
            settingsToggle = true;
        }
        else {
            settingsPanel.DOLocalMoveY(-156.4f, 1);
            settingsToggle = false;
        }            
    }

    public void _ShowCredits() {}

    public void Save(DataRoot data) {}

    public void Load(DataRoot data) {
        newGame = data.gameData.newGame;
    }
}
