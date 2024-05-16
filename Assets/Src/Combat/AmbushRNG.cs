using UnityEngine;
using UnityEngine.Rendering;

public class AmbushRNG : MonoBehaviour{
    public float timer;

    private void Awake() {
        timer =  Random.Range(10, 45);
    }

    private void Update() {
        timer -= Time.deltaTime;

        if(timer <= 0) {
            int randomInterval = Random.Range(1, 20);
            timer = randomInterval;

            DataManager.instance.WriteSaveFile();
            SceneHandler.LoadScene("Combat");

            Debug.Log("Enemy Ambush!");
        }
    }
}
