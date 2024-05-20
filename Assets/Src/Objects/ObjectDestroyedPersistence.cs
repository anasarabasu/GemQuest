using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ObjectDestroyedPersistence : MonoBehaviour, ISaveable {
    private void Start() {
        foreach(var obj in persistingObjectDestroyed) 
            Destroy(obj); 
    }

    static List<GameObject> persistingObjectDestroyed = new();
    public static void ObjectDestroyed(GameObject obj) {
        persistingObjectDestroyed.Add(obj);
    }

    public void Save(DataRoot data) {
        if(data.levelData.currentLevel == 1)
            data.levelData.lvl1ObjectPersistence = persistingObjectDestroyed;
        // if(currentLevel == 2)
        //     data.levelData.lvl2ObjectPersistence = levelObjects;
    }

    public void Load(DataRoot data) {
        if(data.levelData.currentLevel == 1)
            persistingObjectDestroyed = data.levelData.lvl1ObjectPersistence;
    }
}
