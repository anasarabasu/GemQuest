using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ItemData : ScriptableObject {
    [TextArea] public string description = "description";
    public Level level;
    public Inventory inventory;

    [Serializable] public class Level {
        public Sprite sprite;
        public int hitPoints = 1;
    }

    [Serializable] public class Inventory {
        public Sprite sprite;
        public int amount = 1;
    }

}
