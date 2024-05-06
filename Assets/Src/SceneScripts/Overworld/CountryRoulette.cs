using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CountryRoulette : MonoBehaviour, ISaveLoad {
    public static CountryRoulette instance; //might remove this
    private void Awake() {
        instance = this;
    }

    private string[] targetCountries = {"Czechia", "Mexico", "Egypt", "Italy", "Turkey"};
    private int chapter;
    public void _CountryChecker(GameObject country) {

        string name = country.GetComponent<TextMeshProUGUI>().text;
        if(targetCountries.Contains(name)) {
            if(chapter == 1 & name == "Czechia")
                CorrectCountry(name);
            else if(chapter == 2 & name == "Turkey")
                CorrectCountry(name);
            else if(chapter > 2)
                CorrectCountry(name) ;
            else
                WrongCountry();
        }
        else
            WrongCountry();     
        DataManager.instance.SaveGame();
           
    }

    private void CorrectCountry(string sceneName) {
        SceneHandler.LoadScene(sceneName +"1"); 
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
        chapter = data.gameData.chapter;
        penaltyWrongCountry = data.overworldData.penaltyWrongCountry;
    }
}
