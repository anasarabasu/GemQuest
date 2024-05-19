using System;
using UnityEngine;


[CreateAssetMenu]
public class CombatData : ScriptableObject {
    public int health = 40;
    public int currentHealth = 40;
    public int energy = 30;
    public int currentEnergy = 30;
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

