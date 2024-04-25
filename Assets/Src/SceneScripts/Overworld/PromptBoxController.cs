using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class PromptBoxController : MonoBehaviour {
    [SerializeField] GameObject travelPrompt;
    [SerializeField] TextMeshProUGUI travelText;
    [SerializeField] GameObject countrySelection;
    [SerializeField] PointClick playerMovement;
    [SerializeField] GameObject cluePanel;

    private GameObject[] countryLabels;
    private void Awake() {
        countryLabels = GameObject.FindGameObjectsWithTag("CountryRoulette");
    }

    private void Start() {
        travelPrompt.SetActive(false);
        countrySelection.SetActive(false);        
    }
    
    private string currentContinent;
    internal void ShowTravelPrompt(string continent) {
        currentContinent = continent;
        
        travelText.SetText("Travel to " +continent+ "?");
        travelPrompt.SetActive(true);
    }

    internal void HideTravelPrompt() {
        travelPrompt.SetActive(false);
    }

    private Dictionary<string, string[]> countryDictionary = new Dictionary<string, string[]> {
        {"Africa", new string[6] {"Egypt", "Liberia", "Uganda", "Ghana", "Burundi", "Chad"}},
        {"Europe", new string[6] {"Italy", "Czechia", "Ukraine", "Hungary", "Montenegro", "Greece"}},
        {"Asia", new string[6] {"Turkey", "Myanmar", "Iran", "Afghanistan", "Japan", "Laos"}},
        {"Australia", new string[6] {"Australia", "New Zealand", "Fiji", "Kiribati", "Tuvalu", "Nauru"}},
        {"North America", new string[6] {"USA", "Mexico", "Bahamas", "Canada", "Guatemala", "Cuba"}},
        {"South America", new string[6] {"Argentina", "Peru", "Uruguay", "Chile", "Brazil", "Venezuela"}},
    }; 

    private string[] ShuffleCountries(string[] list) {
        string[] shuffledList = new string[list.Length];
        List<string> unshuffledList = list.Cast<string>().ToList();
        Random random = new Random();

        for (int i = 0; i < shuffledList.Length; i++) {
            int index = random.Next(0, unshuffledList.Count);

            shuffledList[i] = unshuffledList[index];
            unshuffledList.RemoveAt(index);
        }
        return shuffledList;
    }

    private void LabelCountryButtons(string continent) {
        int index = 0;
        string[] country = ShuffleCountries(countryDictionary[continent]);

        foreach(GameObject label in countryLabels) {
            TextMeshProUGUI textComp = label.GetComponent<TextMeshProUGUI>();
            textComp.SetText(country[index]);              
            index ++;
        }
    }

    public void _OpenCountrySelection() {
        if(currentContinent != "Antarica") {
            LabelCountryButtons(currentContinent);

            countrySelection.SetActive(true);
            travelPrompt.SetActive(false);
            playerMovement.SetActive(false);
        }
        else
            CountryRoulette.instance.WrongCountry();
    }

    public void _CloseCountrySelection() {
        countrySelection.SetActive(false);
        travelPrompt.SetActive(true);
        playerMovement.SetActive(true);
    } 

    public void _ShowRiddle() {
        // cluePanel.transform.position = 
    }
}
