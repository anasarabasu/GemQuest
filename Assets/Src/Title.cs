using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] string nextScene;

    private void Start() {
        Debug.Log("Title screen loaded");
    }

    public void StartButton() {
        Debug.Log("Switching to " +nextScene);
        SceneManager.LoadSceneAsync(nextScene);
    }

}
