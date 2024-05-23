using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class LightFlicker : MonoBehaviour {

    [SerializeField] Transform flickerLight;
    Light2D flickerLightComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        flickerLight = transform.GetChild(1);
        flickerLightComponent = flickerLight.GetComponent<Light2D>();

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        for (; ; ) //this is while(true)
        {
            float randomIntensity = Random.Range(1.5f, 3.5f);
            flickerLightComponent.intensity = randomIntensity;


            float randomTime = Random.Range(0f, 0.1f);
            yield return new WaitForSeconds(randomTime);
        }
    }
}