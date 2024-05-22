using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu] [ExecuteInEditMode]
public class ItemData : ScriptableObject {
    [TextArea] public string CombatDescription = "description";

    [TextArea] public string LevelDescription = "description";
    public Sprite sprite;
    public int dropChance = 1;
    public int maxLootDrop = 1;
    public int hitPoints = 1;
    public int inventoryAmount = 1;
    public enum ItemType {MINERAL, FOOD}
    public ItemType itemType;

    public enum ItemEffectType {NONE, ELECTRIC, ACID, SHIELD, HEAL, STUN}
    public ItemEffectType itemEffectType;


    public int effectTurnDuration = 1;
    public int effectAmount = 1;


    public enum Enemies {NONE, SHADE, GOLEM}
    public Enemies strongAgainst;
    public Enemies notEffectiveTo;
    public bool oneHit;
    public int damageAddedToSkill;
    public bool failSkill;

    //constructiom -> shield
    //tool -> attack
    //medicine ->heal
    //electric -> electrify
    //weight -> stun

    public void UseItem_EFFECT(Combat target) {
        // if(itemEffectType == ItemEffectType.ELECTRIC) //can hold
        //     target.SetElectrocute(effectTurnDuration, effectAmount);

        // if(itemEffectType == ItemEffectType.ACID)
        //     target.Acid(effectAmount);

        // if(itemEffectType == ItemEffectType.SHIELD) //block damage
        //     target.SetShield(effectTurnDuration);

        // if(itemEffectType == ItemEffectType.HEAL)
        //     target.Heal(effectAmount);

        // if(itemEffectType == ItemEffectType.STUN) //can hold
        //     target.Stun(effectTurnDuration, effectAmount);
        
    }

    public void UseItem_SKILL() {
        
    }
}
