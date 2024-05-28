using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class FlashLightMechanic : MonoBehaviour {
    [SerializeField] Light2D[] flashLights;
    [SerializeField] float flashLightCharge = 12;
    [SerializeField] float decreaseSpeed = 0.05f;

    public static FlashLightMechanic instance;
    private void Awake() {
        instance = this;
    }

    private void Update() {
        flashLightCharge -= decreaseSpeed * Time.deltaTime;

        foreach(var flashLight in flashLights) {
            flashLight.pointLightOuterRadius = flashLightCharge;
        }

        if(inTheLight) {
            AmbushRNG.SetFrequency(0);
            return;
        }

        switch (flashLightCharge) {
            case <= 0:
                flashLightCharge = 0;
                break;

            case <= 3:
                AmbushRNG.SetFrequency(75);
                break;

            case <= 8:
                AmbushRNG.SetFrequency(50);
                break;

            case <= 12:
                decreaseSpeed = 0.05f;
                AmbushRNG.SetFrequency(25);
                break;

            case <= 30:
                decreaseSpeed = 0.5f;
                AmbushRNG.SetFrequency(0);
                break;
        }
    }

    bool inTheLight;

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Light")) 
            inTheLight = true;
    }

    private void OnTriggerExit2D() => inTheLight = false;

    public void AddBattery(int amount) => flashLightCharge = amount;
}
