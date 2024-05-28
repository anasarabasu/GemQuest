using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class AmbushRNG : MonoBehaviour{
    public float timer = 10;

    [SerializeField] GameObject ambushImage;
    public int frequencyForShow = 50; //75, 50, 25, 0
    public static int frequency = 50;
    AsyncOperation combatScene;

    private void Update() {
        frequencyForShow = frequency;

        timer -= Time.deltaTime;

        if(timer <= 0) {
            int randomInterval = Random.Range(5, 15);

            int randomRoll = Random.Range(0, 101);
            timer = randomInterval;
            if(randomRoll <= frequency) {
                StartCoroutine(EnemyAmbush());
            }
        }
    }

    public static AmbushRNG instance;

    private void Awake() => instance = this;

    public static void SetFrequency(int amount) => frequency = amount;

    IEnumerator EnemyAmbush() {
        DataManager.instance.WriteSaveFile();

        combatScene = SceneManager.LoadSceneAsync("Combat");
        combatScene.allowSceneActivation = false;

        yield return new WaitUntil(() => !combatScene.isDone);
        yield return new WaitForSeconds(0.5f);

        ambushImage.SetActive(true);
        combatScene.allowSceneActivation = true;

        
        Debug.Log("Enemy Ambush!");
    }
}
