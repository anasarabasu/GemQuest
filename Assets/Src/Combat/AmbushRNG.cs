using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbushRNG : MonoBehaviour{
    public float timer = 60;
    private void Awake() => timer = Random.Range(10, 45);

    [SerializeField] GameObject ambushImage;
    AsyncOperation combatScene;

    private void Update() {
        timer -= Time.deltaTime;

        if(timer <= 0) {
            int randomInterval = Random.Range(1, 20);
            timer = randomInterval;

            DataManager.instance.WriteSaveFile();
            
            combatScene = SceneManager.LoadSceneAsync("Combat");
            combatScene.allowSceneActivation = false;
            if(!combatScene.isDone) 
                StartCoroutine(EnemyAmbush());


            Debug.Log("Enemy Ambush!");
        }
    }

    IEnumerator EnemyAmbush() {
        //shadow dash ani towards party
        ambushImage.SetActive(true);
        
        yield return new WaitForSeconds(2);
        combatScene.allowSceneActivation = true;
    }
}
