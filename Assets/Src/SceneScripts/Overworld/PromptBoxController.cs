using System.Collections.Generic;
using System.Linq;
using TMPro;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

public class PromptBoxController : MonoBehaviour {
    private GameObject[] countryLabels;
    
    private void Awake() => countryLabels = GameObject.FindGameObjectsWithTag("CountryRoulette");
    
    private void Update() => Time.timeScale = isPaused ? 0 : 1;
    
    [SerializeField] Transform travelPrompt;
    private string currentContinent;
    
    internal void UpdateTravelPrompt(string continent) {
        currentContinent = continent;
        travelPrompt.GetComponentInChildren<TextMeshProUGUI>().SetText("Travel to " + continent+ "?");
        ShowTravelPrompt();
    }
    private void ShowTravelPrompt() => travelPrompt.DOScale(Vector3.one, 0.5f).SetUpdate(true);
    
    internal void HideTravelPrompt() => travelPrompt.DOScale(Vector3.zero, 0.5f).SetUpdate(true);

    private Dictionary<string, string[]> countryDictionary = new() {
        {"Africa", new string[6] {"Egypt", "Liberia", "Uganda", "Ghana", "Burundi", "Chad"}},
        {"Europe", new string[6] {"Italy", "Czechia", "Ukraine", "Hungary", "Montenegro", "Greece"}},
        {"Asia", new string[6] {"Turkey", "China", "Iran", "Afghanistan", "Japan", "Laos"}},
        {"Australia", new string[6] {"Australia", "New Zealand", "Fiji", "Kiribati", "Tuvalu", "Nauru"}},
        {"North America", new string[6] {"USA", "Mexico", "Bahamas", "Canada", "Guatemala", "Cuba"}},
        {"South America", new string[6] {"Argentina", "Peru", "Uruguay", "Chile", "Brazil", "Venezuela"}},
    }; 
    private string[] ShuffleCountries(string[] list) {
        string[] shuffledList = new string[list.Length];
        List<string> unshuffledList = list.Cast<string>().ToList();
        Random random = new();

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

    [SerializeField] Transform countrySelection;
    bool isPaused;
    public void _OpenCountrySelection() {
        if(currentContinent != "Antarica") {
            LabelCountryButtons(currentContinent);
            _ToggleCluePanel();
            HideTravelPrompt();
            countrySelection.DOLocalJump(Vector3.zero, 2, 1, 0.5f).SetUpdate(true);
            isPaused = true;
        }
        else    
            Debug.Log("no");
    }

    public void _CloseCountrySelection() {
        _ToggleCluePanel();
        ShowTravelPrompt();
        countrySelection.DOLocalMoveY(-180, 0.5f).SetUpdate(true);
        isPaused = false;
    }

    [SerializeField] Transform cluePanel;
    bool clueToggle = true;
    public void _ToggleCluePanel() {
        if(clueToggle) {
            cluePanel.DOLocalMoveX(-241.1f, 0.5f).SetUpdate(true);
            clueToggle = false;
        }
        else {
            cluePanel.DOLocalMoveX(-119.625f, 0.5f).SetUpdate(true);
            clueToggle = true;
        }
    }
}
