using UnityEngine;

public class Settings : MonoBehaviour, ISaveable{
    FPSCounter fps;
    bool showFPS;
    private void Awake() {
        fps = GameObject.FindGameObjectWithTag("FPS").GetComponent<FPSCounter>();
        fps.ShowFPS(fps);
    }

    public void _DeleteSave() => DataManager.instance.DeleteSaveFile();

    public void _ToggleFPS() {
        showFPS = !showFPS;

        fps.ShowFPS(showFPS);
        DataManager.instance.WriteSaveFile();
    }

    public void Load(DataRoot data) => showFPS = data.gameData.showFPS;

    public void Save(DataRoot data) => data.gameData.showFPS = showFPS;
}
