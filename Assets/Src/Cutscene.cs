using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        Debug.Log("Cutscene loaded");
    }

    public void LoadOverworld () {
        Debug.Log("Switching to Overworld");
        SceneManager.LoadSceneAsync("Overworld");
    }
}
