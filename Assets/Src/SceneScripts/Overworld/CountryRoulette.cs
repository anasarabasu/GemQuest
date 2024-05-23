using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountryRoulette : MonoBehaviour, ISaveable {
    public void _CountryChecker(GameObject country) {
        Time.timeScale = 1;

        string name = country.GetComponent<TextMeshProUGUI>().text;
        if(name == "China") 
            SceneManager.LoadSceneAsync("Level1");
        else {
            SceneManager.LoadSceneAsync("WrongChoice"); //should be additive scene
            penaltyWrongCountry ++;
        }

        DataManager.instance.WriteSaveFile();
    }

    private int penaltyWrongCountry; //might move this or something

    public void Save(DataRoot data) {
        data.levelData.currentLevel = 1;
        data.levelData.levelCoordinates = new Vector2(0, -2);
        data.overworldData.penaltyWrongCountry = penaltyWrongCountry;
    }

    public void Load(DataRoot data) {
        penaltyWrongCountry = data.overworldData.penaltyWrongCountry;
    }
}
