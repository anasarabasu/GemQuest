using UnityEngine;

public class SetFPS : MonoBehaviour {
    private void Awake() => Application.targetFrameRate = 60;
}
