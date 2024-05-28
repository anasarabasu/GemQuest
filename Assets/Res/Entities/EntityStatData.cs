using System;
using UnityEngine;


[CreateAssetMenu]
public class EntityStatData : ScriptableObject {
    public int level = 0;
    public int currentXP = 0;
    public int nextLevelThreshold = 50;
    public void IncreaseXP(int amount) {
        currentXP += amount;

        if(currentXP >= nextLevelThreshold) {
            level++;
            currentXP -= nextLevelThreshold;
            nextLevelThreshold += level * 11;

            health += level * 4;
            energy += level * 2;
            foreach(var move in movesets) {
                move.damage += level * 2;
            }
        }
    }

    public int toolLevel = 0;
    public int currentToolXP = 0;
    public int nextToolLevelThreshold = 10;
    public void UpgradeTool(int amount) {
        currentToolXP += amount;

        if(currentToolXP >= nextToolLevelThreshold) {
            toolLevel++;
            currentToolXP -= nextToolLevelThreshold;
            nextLevelThreshold += toolLevel * 11;
        }
    }

    public float health = 30;
    public float currentHealth = 30;
    public int energy = 20;
    public int currentEnergy = 20;
    public int speed = 1;
    public enum EntityType {Hero, Shade, Golem}
    public EntityType entityType;
    public Moveset[] movesets;

    [Serializable]
    public class Moveset {
        public string name;
        public enum Type {Skill, MeleeAttack, RangedAttack};
        public float attackDistance = 1;
        public Type type;
        public int damage = 1;
        public int energyCost = 1;
    }
}

