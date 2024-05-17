using System;
using System.Collections;
using Aarthificial.Reanimation;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class AttackTypes {
    public string name = "Hit";
    public enum Type {Melee, Ranged};
    public Type type = new();
    public int damage = 1;
}

public class CombatStats : MonoBehaviour {
    public string characterName;
    public bool isAlive = true;
    public int level = 1;
    public int health = 5;
    public int energy = 5;
    public int speed = 1;
    public AttackTypes[] attackTypes = new AttackTypes[3];

    private Reanimator reanimator;
    private void Awake() {
        reanimator = GetComponent<Reanimator>();
        reanimator.AddListener("ActionFinished", ActionFinished);
        //listen attackhut
    }

    internal bool isActionFinished;
    private void ActionFinished() => isActionFinished = true;

    public void PerformAttack(GameObject target, int attackKey) {
        if(gameObject.CompareTag("Hero"))
            reanimator.Set("Action", attackKey+1);

        CombatStats _target = target.GetComponent<CombatStats>();
        StartCoroutine(_target.IsAttacked(attackTypes[attackKey].damage));
    }


    IEnumerator IsAttacked(int damageTaken) {
        health -= damageTaken;
        reanimator.Renderer.color = Color.red;
        transform.DOShakePosition(1);
        
        yield return null;
        reanimator.Renderer.color = Color.white;
        isActionFinished = false;
        CheckHealth();
    }

    private void CheckHealth() {
        if(health <= 0) {
            reanimator.Set("Action", 4); //dead
            isAlive = false;
            // reanimator.Renderer.color = Color.black;
        }
    }

    // private void IsAttacked(int damageTaken) {

    // }

    }
