using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CountryRoulette : MonoBehaviour, ISaveable {
    public static CountryRoulette instance; //might remove this

    private string[] targetCountries = {"Czechia", "Mexico", "Egypt", "Italy", "Turkey"};
    public void _CountryChecker(GameObject country) {
        Time.timeScale = 1;

        string name = country.GetComponent<TextMeshProUGUI>().text;
        if(targetCountries.Contains(name)) {
            if(name == "Czechia")
                CorrectCountry(name);
        }
        else
            WrongCountry();    

        DataManager.instance.WriteSaveFile();
    
    }

    private void CorrectCountry(string sceneName) {
        SceneHandler.LoadScene(sceneName); 
        // chapter ++;
    }

    private int penaltyWrongCountry; //might move this or something
    public void  WrongCountry() {
        SceneHandler.LoadScene("WrongChoice");
        penaltyWrongCountry ++;
    }

    public void Save(DataRoot data) {
        // data.gameData.chapter = this.chapter;
        data.overworldData.penaltyWrongCountry = penaltyWrongCountry;
    }

    public void Load(DataRoot data) {
        penaltyWrongCountry = data.overworldData.penaltyWrongCountry;
    }
}
