using System;
using UnityEngine;

[CreateAssetMenu] [ExecuteInEditMode]
public class ItemData : ScriptableObject {
    [TextArea] public string introDesccription;
    public Sprite sprite;
    public int dropChance = 1;
    public int maxLootDrop = 1;
    public int hitPoints = 1;
    public int inventoryAmount = 1;

    public Description description;

    [Serializable] public class Description {
        public bool unlockedLevelDescription;
        [TextArea] public string Level;

        public bool unlockedCombatDescription_ITEM;
        [TextArea] public string Combat_ITEM;

        public bool unlockedCombatDescription_SKILL;
        [TextArea] public string Combat_SKILL;

    }


    public LevelFunction levelFunction;
    [Serializable] public class LevelFunction {
        public bool unlockedPriceRank;
        public enum PriceRank {Low, Mid, High, Zero} //have a boost after certain amount sold 10 
        public PriceRank priceRank;
        public enum Function {None, ReplenishBattery, UpgradeTool, Heal, DeterEnemies, ConvertToOther}
        public Function function;
        public int amount;
    }


    public CombatFunction combatFunction;
    [Serializable] public class CombatFunction {
        public enum Enemy {None, Shade, Golem}
        public Enemy effectiveTo;
        public Enemy uselessTo;

        public enum AsAnItem {None, Stun, Electric, Acid, Shield, Heal}
        public AsAnItem asAnItem;
        public int asAnItemDuration;
        public int asAnItemAmount;


        public enum WithASkill  {Fail, IncreaseAttack, OneHit}
        public WithASkill withASkill;
        public int skillDamageIncreaseAmount;
    }

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
