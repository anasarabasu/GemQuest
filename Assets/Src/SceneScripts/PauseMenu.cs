using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    [SerializeField] Transform menuPanel;
    [SerializeField] Transform settingsPanel;
    Image darken;
    private void Awake() {
        darken = GetComponent<Image>();
        darken.enabled = false;
    }

    bool isPaused;
    private void Update() => Time.timeScale = isPaused ? 0 : 1;

    private bool menuToggle = false;
    public void _ToggleMenu() {
        if(!menuToggle) {
            isPaused = true;
            darken.enabled = true;
            menuPanel.DOLocalMoveX(113, 0.5f).SetUpdate(true);
            menuToggle = true;
        }
        else {
            isPaused = false;
            if(settingsToggle)
                _ToggleSettings();
            darken.enabled = false;
            menuPanel.DOLocalMoveX(274, 0.5f).SetUpdate(true);
            menuToggle = false;
        }
    }

    private bool settingsToggle = false;
    public void _ToggleSettings() {
        if(!settingsToggle) {
            settingsPanel.DOLocalMoveX(-74.2f, 0.5f).SetUpdate(true);
            settingsToggle = true;
        }
        else {
            settingsPanel.DOLocalMoveX(-266f, 0.5f).SetUpdate(true);
            settingsToggle = false;
        }
    }

    public void  _QuitGame() {
        DataManager.instance.WriteSaveFile();
        Application.Quit(0);
    }
}
