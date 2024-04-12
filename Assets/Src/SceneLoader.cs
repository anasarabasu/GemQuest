using UnityEngine;
using UnityEngine.InputSystem;

public class SceneManager : MonoBehaviour {
    public static SceneManager instance;

    private void Awake() {
        instance = this;
    }

    public void LoadScene(string sceneName) {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        Debug.Log("Loaded " +sceneName);
    }

    public void ReloadScene(InputAction.CallbackContext context) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Scene reloaded");
    }
    //TODO: transition effects 
}
