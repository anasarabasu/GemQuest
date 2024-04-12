using UnityEngine;

public class ContinentTrigger : MonoBehaviour {
    [SerializeField] PromptBoxController prompt;

    private void OnTriggerEnter(Collider continent) {
        prompt.ShowTravelPrompt(continent.name);
    }

    private void OnTriggerExit() {
        prompt.HideTravelPrompt();
    }

}
