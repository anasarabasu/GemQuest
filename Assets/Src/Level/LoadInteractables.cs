using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadInteractables : MonoBehaviour, ISaveable {
    public Dictionary<GameObject, bool> StatePersistence = new Dictionary<GameObject, bool>();

    public void Load(DataRoot data) {
        // Instantiate(data.levelData.destroyableObjects[0], transform);
        // destroyableObjects = data.levelData.destroyableObjects;
    }

    public void Save(DataRoot data) {
        // data.levelData.destroyablesStatePersistence = StatePersistence;
    }

    private void Awake() {
        // StatePersistence.Add(GameObject.FindGameObjectWithTag("Destroyable"), GameObject.FindGameObjectWithTag("Destroyable").GetComponent<LevelObject>().isDestroyedPersistent);

        // gameObject.GetInstanceID
        
    }

    private void Update() {
        
    }
}
