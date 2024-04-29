using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class DataManager: MonoBehaviour {
    public static DataManager instance;
    private DataRoot data;
    private List<ISaveLoad> ISaveLoadList;
    private string dataPath;
    private void Awake() {
        instance = this;

        data = new DataRoot() {
            gameData = new GameData()
        };

        ISaveLoadList = FindSaveLoadDataList();
        
        dataPath =  Application.persistentDataPath + Path.AltDirectorySeparatorChar + "GameData.json";

        if(File.Exists(dataPath)) 
            LoadGame();
        else {
            string createJSON = JsonUtility.ToJson(data, true); //create default json file
            using (StreamWriter streamWriter = new StreamWriter(dataPath))
                streamWriter.Write(createJSON);
            
            Debug.Log("Save file created");
        }
    }

    private List<ISaveLoad> FindSaveLoadDataList() {
        IEnumerable<ISaveLoad> obj = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>();
        return new List<ISaveLoad>(obj);
    }

    public void SaveGame() {
        foreach(ISaveLoad item in ISaveLoadList) 
            item.Save(data);

        string saveJSON = JsonUtility.ToJson(data, true); //write data to json
        using (StreamWriter streamWriter = new StreamWriter(dataPath))
            streamWriter.Write(saveJSON);

        Debug.Log("Game data saved");
    }

    public void LoadGame() {
        string loadJSON;
        using (StreamReader streamReader = new StreamReader(dataPath))
            loadJSON = streamReader.ReadToEnd();
        DataRoot loadData = JsonUtility.FromJson<DataRoot>(loadJSON); //create data from json
        data = loadData;

        foreach(ISaveLoad item in ISaveLoadList) 
            item.Load(data);

        Debug.Log("Game data loaded");
    }

    public void DeleteFile() {
        if(File.Exists(dataPath)) {
            File.Delete(dataPath);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            Debug.Log("Save file deleted... Game Reloaded");
        }
        else 
            Debug.Log("No save file to delete");
    }

    public void DeleteJSON(InputAction.CallbackContext context) {
        DeleteFile();
    }

    public void DisplayJSON(InputAction.CallbackContext context) {
        Debug.Log(JsonUtility.ToJson(data, true));
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    private void OnApplicationPause(bool pauseStatus) {
        SaveGame();
    }
}

