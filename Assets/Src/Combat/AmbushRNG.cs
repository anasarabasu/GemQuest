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
            int randomInterval = Random.Range(1, 10);
            timer = randomInterval;

            int randomRoll = Random.Range(0, 101);
            if(randomRoll <= frequency) 
                AmbushOccurs();
        }
    }

    private void AmbushOccurs() {
        DataManager.instance.WriteSaveFile();
            // combatScene = SceneManager.LoadSceneAsync("Combat");
            // combatScene.allowSceneActivation = false;
            // if(!combatScene.isDone) 
            //     StartCoroutine(EnemyAmbush());


            Debug.Log("Enemy Ambush!");
    }

    public static void SetFrequency(int amount) => frequency = amount;

    IEnumerator EnemyAmbush() {
        //shadow dash ani towards party
        ambushImage.SetActive(true);
        
        yield return new WaitForSeconds(2);
        combatScene.allowSceneActivation = true;
    }
}
