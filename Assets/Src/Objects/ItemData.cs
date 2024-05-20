using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu]
public class ItemData : ScriptableObject {
    [TextArea] public string description = "description";
    public Sprite sprite;
    public int hitPoints = 1;
    public int inventoryAmount = 1;

    public enum ItemType {MINERAL, FOOD}
    public ItemType itemType;

    public int healAmount = 0;
    public int energyAmount = 0;
    public int stunDuration = 0;
    public int electricDuration = 0;
    public int damageAmount = 0;

    //constructiom -> shield
    //tool -> attack
    //medicine ->heal
    //electric -> electrify
    //weight -> stun

    public void UseItem(Combat target) {
        if(SceneManager.GetActiveScene().name == "Combat") {
            if(itemType == ItemType.MINERAL) {
                target.Heal(healAmount);
                target.Damage(damageAmount);
                //target electrocute
                //target stun

            }
        }
        else {

        }
        
    }
}
