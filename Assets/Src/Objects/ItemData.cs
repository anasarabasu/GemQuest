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

    public void SellItem(Combat target, string name) {
        switch (levelFunction.priceRank) {
            case LevelFunction.PriceRank.Zero:
                target.combatData.IncreaseXP(1);
                target.SetEnergy(1, name);
                break;

            case LevelFunction.PriceRank.Low:
                target.combatData.IncreaseXP(16);
                target.SetEnergy(5, name);
                break;

            case LevelFunction.PriceRank.Mid:
                target.combatData.IncreaseXP(44);
                target.SetEnergy(12, name);
                break;

            case LevelFunction.PriceRank.High:
                target.GetComponent<Combat>().combatData.IncreaseXP(102);
                target.SetEnergy(20, name);
                break;
        }

        if(target.combatData.currentEnergy >= target.GetComponent<Combat>().combatData.energy)
                target.combatData.currentEnergy = target.GetComponent<Combat>().combatData.energy;
    }


    public CombatFunction combatFunction;
    [Serializable] public class CombatFunction {
        public enum Enemy {None, Shade, Golem}
        public Enemy effectiveTo;
        public Enemy uselessTo;

        public enum AsAnItem {None, Stun, Electric, Acid, Shield, Heal, Distract}
        public AsAnItem asAnItem;
        public int asAnItemDuration;
        public int asAnItemAmount;

        public void UseItem_Effect(Combat target) {
            if(asAnItem == AsAnItem.Stun) //can hold
                target.SetStun(asAnItemDuration, asAnItemAmount);

            if(asAnItem == AsAnItem.Electric)
            target.SetElectrocute(asAnItemDuration, asAnItemAmount);

            if(asAnItem == AsAnItem.Acid)
                target.SetAcid(asAnItemDuration, asAnItemAmount);

            if(asAnItem == AsAnItem.Shield) //block damage
                target.SetShield(asAnItemDuration);

            if(asAnItem == AsAnItem.Heal)
                target.SetHeal(asAnItemAmount);

            if(asAnItem == AsAnItem.Distract)
                target.SetDistraction(asAnItemDuration);
        }


        public enum WithASkill  {Fail, IncreaseAttack, OneHit}
        public WithASkill withASkill;
        public int skillDamageIncreaseAmount;

        public CombatSystem.SkillOutcome UseItem_Skill(Combat attacker) {
            if(withASkill == WithASkill.Fail)
                return CombatSystem.SkillOutcome.FAIL;

            if(withASkill == WithASkill.IncreaseAttack)
                return CombatSystem.SkillOutcome.INCREASE;

            if(withASkill == WithASkill.OneHit)
                return CombatSystem.SkillOutcome.ONEHIT;

            return CombatSystem.SkillOutcome.NONE;
        }
    }
}
