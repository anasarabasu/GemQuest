using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class DataManager: MonoBehaviour {
    public static DataManager instance;
    private DataRoot data;
    private List<ISaveable> ISaveLoadList;
    private string dataPath;
    private void Awake() {
        instance = this;
        data = new DataRoot() {gameData = new GameData()};
        ISaveLoadList = FindSaveLoadDataList();
        dataPath =  Application.persistentDataPath + Path.AltDirectorySeparatorChar + "GameData.json";
        
        if(File.Exists(dataPath)) 
            LoadSaveFile();
        else {
            string createJSON = JsonUtility.ToJson(data, true); //create default json file
            using (StreamWriter streamWriter = new(dataPath))
                streamWriter.Write(createJSON);
            
            Debug.Log("Save file created");
        }
    }

    private List<ISaveable> FindSaveLoadDataList() {
        IEnumerable<ISaveable> obj = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
        return new List<ISaveable>(obj);
    }

    public void WriteSaveFile() {
        foreach(ISaveable item in ISaveLoadList) 
            item.Save(data);

        string saveJSON = JsonUtility.ToJson(data, true); //write data to json
        using (StreamWriter streamWriter = new(dataPath))
            streamWriter.Write(saveJSON);

        Debug.Log("Game data saved");
    }

    public void LoadSaveFile() {
        string loadJSON;
        using (StreamReader streamReader = new(dataPath))
            loadJSON = streamReader.ReadToEnd();
        DataRoot loadData = JsonUtility.FromJson<DataRoot>(loadJSON); //create data from json
        data = loadData;

        foreach(ISaveable item in ISaveLoadList) 
            item.Load(data);

        Debug.Log("Game data loaded");
    }

    public void DeleteSaveFile() {
        if(File.Exists(dataPath)) {
            // ResetInventoryData();

            File.Delete(dataPath);
            SceneHandler.LoadScene(SceneHandler.Scene.TitleMenu);
            SceneHandler.LoadScene(SceneHandler.Scene.TitleMenu);
            Debug.Log("Save file deleted... Game Reloaded");
        }
        else 
            Debug.Log("No save file to delete");
    }

    private void ResetInventoryData () {
        foreach (var item in data.inventoryData)
            item.inventoryAmount = 1;
    }

    private void OnApplicationQuit() => WriteSaveFile();

    private void OnApplicationPause(bool pauseStatus) => WriteSaveFile();
}