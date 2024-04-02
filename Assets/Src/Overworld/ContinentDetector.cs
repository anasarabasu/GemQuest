using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinentDetector : MonoBehaviour {
    [SerializeField] string targetContinent = "Europe";
    public string currentContinent;

    private void OnTriggerEnter(Collider continent) {
        currentContinent = continent.name;
        Debug.Log("In " +currentContinent);
    }        

    private void OnTriggerExit() {
        currentContinent = "";
    }
    
    void OnInteract() {
        Debug.Log("Interact button clicked");

        if(currentContinent == "") Debug.Log("Nothing to interact with");
        else if(currentContinent == targetContinent) {
            Debug.Log("Loading world one");
            SceneManager.LoadSceneAsync("CzechCaverns");
        }
        else Debug.Log("Wrong continent");
    }
}
