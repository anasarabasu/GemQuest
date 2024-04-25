using UnityEngine;

public class AutoSave : MonoBehaviour {

    private int autosaveInterval = 30;
    private float timer;

    private void Update() {
        timer -= Time.deltaTime;

        if(timer <= 0) {
            DataManager.instance.SaveGame();
            timer = autosaveInterval;
            
            Debug.Log("Game auto-saved");
        }
    }

    //show saving icon?
}
