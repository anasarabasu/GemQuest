using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public static SceneLoader instance;

    private void Start() {
        instance = this;
    }

    public void SwitchTo(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);

        Debug.Log("Loaded " +sceneName);
    }

    public void ReloadScene(InputAction.CallbackContext context) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Scene reloaded");
    }
    //TODO: transition effects 
}
