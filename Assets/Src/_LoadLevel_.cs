using UnityEngine;

public class _LoadLevel_ : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D trigger) {
        if(trigger.name == "NextLevelTrigger")
            SceneHandler.LoadScene("Czechia2");
    }
}
