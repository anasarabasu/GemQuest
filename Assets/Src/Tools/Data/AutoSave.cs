using UnityEngine;

public class AutoSave : MonoBehaviour {

    private float timer;
    private int autosaveInterval = 30;

    private void Update() {
        timer -= Time.deltaTime;

        if(timer <= 0) {
            DataManager.instance.WriteSaveFile();
            timer = autosaveInterval;
            
            Debug.Log("Game auto-saved");
        }
    }

    //show saving icon?
}
