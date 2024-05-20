using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour {
    [SerializeField] GameObject trigger;

    private void OnTriggerEnter2D(Collider2D collider2D) {
        if(collider2D.gameObject == trigger) {
            SceneHandler.LoadScene(SceneManager.GetActiveScene().name + "2");
            transform.position = Vector3.zero;
        }
    }
}
