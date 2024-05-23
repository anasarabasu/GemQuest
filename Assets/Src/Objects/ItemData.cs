using UnityEngine;

[CreateAssetMenu] [ExecuteInEditMode]
public class ItemData : ScriptableObject {
    public bool unlockLevelDescription;
    [TextArea] public string LevelDescription = "description";

    public bool unlockCombatDescription;
    [TextArea] public string CombatDescription = "description";

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

    public int UpgradesTool;

    //constructiom -> shield
    //tool -> attack
    //medicine ->heal
    //electric -> electrify
    //weight -> stun

    public void UseItem_LEVEL() {
        Mine.instance.Upgrade(UpgradesTool);
    }

    public void UseItem_EFFECT(Combat target) {
        if(itemEffectType == ItemEffectType.ELECTRIC)
            target.SetElectrocute(effectTurnDuration, effectAmount);

        if(itemEffectType == ItemEffectType.ACID)
            target.SetAcid(effectAmount);

        if(itemEffectType == ItemEffectType.SHIELD) //block damage
            target.SetShield(effectTurnDuration);

        if(itemEffectType == ItemEffectType.HEAL)
            target.SetHeal(effectAmount);

        if(itemEffectType == ItemEffectType.STUN) //can hold
            target.SetStun(effectTurnDuration);
    }

    public void UseItem_SKILL() {
        
    }
    
}
