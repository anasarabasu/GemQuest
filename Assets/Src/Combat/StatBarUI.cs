using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StatBarUI : MonoBehaviour {
    public static StatBarUI instance;

    public EntityStatData entity;

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider easeHealthSlider;
    public float currentHealth;

    [SerializeField] Slider energySlider;
    public float currentEnergy;

    private void Awake() {
        instance = this;

        transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(entity.name.Replace("Stats", ""));

        healthSlider.maxValue = entity.health;
        easeHealthSlider.maxValue = healthSlider.maxValue;

        currentHealth = entity.currentHealth;
        healthSlider.value = currentHealth;
        easeHealthSlider.value = currentHealth;

        energySlider.maxValue = entity.energy;
        currentEnergy = entity.currentEnergy;
    }

    public void UpdateEntityFocusPanel(EntityStatData newEntity) {
        entity = newEntity;

        transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(entity.name.Replace("Stats", ""));

        healthSlider.maxValue = entity.health;
        easeHealthSlider.maxValue = healthSlider.maxValue;

        currentHealth = entity.currentHealth;
        healthSlider.value = currentHealth;
        easeHealthSlider.value = currentHealth;

        energySlider.maxValue = entity.energy;
        currentEnergy = entity.currentEnergy;
    }

    float lastHealth;
    private void Update() {
        currentHealth = entity.currentHealth;
        lastHealth = easeHealthSlider.value;

        if(lastHealth > currentHealth) {
            easeHealthSlider.gameObject.transform.Find("Fill").GetComponent<Image>().color = Color.yellow;

            if(healthSlider.value != currentHealth) healthSlider.value = currentHealth;
            if(healthSlider.value != easeHealthSlider.value) easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, 0.05f);
        
        }
        else {
            easeHealthSlider.gameObject.transform.Find("Fill").GetComponent<Image>().color = Color.green;
            if(easeHealthSlider.value != currentHealth) easeHealthSlider.value = currentHealth;
            if(easeHealthSlider.value != healthSlider.value) healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, 0.05f);

        }


        currentEnergy = entity.currentEnergy;
        if(energySlider.value != currentEnergy)
            energySlider.value = currentEnergy;

    }
}
