using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour {
    private TextMeshProUGUI text;
    private bool showFPS;
    private void Awake() => text = GetComponent<TextMeshProUGUI>();

    public void ShowFPS(bool showFPS) => this.showFPS = showFPS;

    private void Update() {
		float fps = (int)(1 / Time.unscaledDeltaTime);;

        if(!showFPS)
            text.SetText("");
        else
            text.SetText((fps).ToString());
    }
}
