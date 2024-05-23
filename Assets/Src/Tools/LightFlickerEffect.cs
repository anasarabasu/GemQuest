using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class LightFlicker : MonoBehaviour {

    [SerializeField] Transform flickerLight;
    [SerializeField] float intensityMin = 0;
    [SerializeField] float intensityMax = 4;
    Light2D flickerLightComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        flickerLightComponent = flickerLight.GetComponent<Light2D>();
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        for (; ; ) //this is while(true)
        {
            float randomIntensity = Random.Range(intensityMin, intensityMax);
            flickerLightComponent.intensity = randomIntensity;


            float randomTime = Random.Range(0f, 0.1f);
            yield return new WaitForSeconds(randomTime);
        }
    }
}