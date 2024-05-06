using UnityEngine;

public class AmbushRNG : MonoBehaviour{
    public float timer;

    private void Awake() {
        timer =  Random.Range(5, 20);
    }

    private void Update() {
        timer -= Time.deltaTime;

        if(timer <= 0) {
            int randomInterval = Random.Range(1, 20);
            timer = randomInterval;

            DataManager.instance.SaveGame();
            SceneHandler.LoadScene("Combat");

            Debug.Log("Enemy Ambush!");
        }
    }
}
