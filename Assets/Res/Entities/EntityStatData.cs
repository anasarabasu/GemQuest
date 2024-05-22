using System;
using UnityEngine;


[CreateAssetMenu]
public class EntityStatData : ScriptableObject {
    public float health = 40;
    public float currentHealth = 40;
    public float energy = 30;
    public float currentEnergy = 30;
    public int speed = 1;
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

