using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour, ISaveable {
    [SerializeField] GameObject trigger;
    [SerializeField] string next = "Level2";

    public void Load(DataRoot data) {}

    public void Save(DataRoot data) {}

    bool loadNext;
    private void OnTriggerEnter2D(Collider2D collider2D) {
        if(collider2D.gameObject == trigger) {
            DataManager.instance.WriteSaveFile();
            SceneManager.LoadSceneAsync(next);
        }
    }
}
