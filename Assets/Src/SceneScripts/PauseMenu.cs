using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    Image darken;
    private void Awake() {
        darken = GetComponent<Image>();
        darken.enabled = false;
    }

    bool isPaused;
    private void Update() => Time.timeScale = isPaused ? 0 : 1;

    [SerializeField] RectTransform menuPanel;
    private bool menuToggle = false;
    public void _ToggleMenu() {
        if(!menuToggle) {
            isPaused = true;
            darken.enabled = true;
            menuPanel.DOAnchorPosX(-55.661f, 0.5f).SetUpdate(true);
            menuToggle = true;
        }
        else {
            isPaused = false;
            if(settingsToggle)
                _ToggleSettings();
            darken.enabled = false;
            menuPanel.DOAnchorPosX(81.61897f, 0.5f).SetUpdate(true);
            menuToggle = false;
        }
    }

    [SerializeField] RectTransform settingsPanel;
    private bool settingsToggle = false;
    public void _ToggleSettings() {
        if(!settingsToggle) {
            settingsPanel.DOAnchorPosY(-26.722f, 0.25f).SetUpdate(true);
            settingsToggle = true;
        }
        else {
            settingsPanel.DOAnchorPosY(-176, 0.25f).SetUpdate(true);
            settingsToggle = false;
        }
    }

    public void  _QuitGame() {
        DataManager.instance.WriteSaveFile();
        Application.Quit(0);
    }
}
