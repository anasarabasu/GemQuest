using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour, ISaveable {
    [SerializeField] GameObject trigger;

    public void Load(DataRoot data) {}

    public void Save(DataRoot data) {
        if(newPos != null)
            data.levelData.levelCoordinates = newPos;
    }

    Vector3 newPos;
    private void OnTriggerEnter2D(Collider2D collider2D) {
        if(collider2D.gameObject == trigger) {
            SceneManager.LoadSceneAsync("Level2");
            newPos = Vector3.zero;
        }
    }
}
