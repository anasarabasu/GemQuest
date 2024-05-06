using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour, ISaveLoad {
    public static void LoadScene(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);

        Debug.Log("Loaded " +sceneName);
    }

    public static void ReloadScene(InputAction.CallbackContext context) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Scene reloaded");
    }

    //TODO: transition effects 
    public void Save(DataRoot data) {
        if( SceneManager.GetActiveScene().name != "MainMenu")
            data.gameData.lastScene = SceneManager.GetActiveScene().name;
    }

    public void Load(DataRoot data) {
    }
}
