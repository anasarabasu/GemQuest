using System;
using System.Collections;
using Aarthificial.Reanimation;
using DG.Tweening;
using UnityEngine;

public class Combat : MonoBehaviour {
    public CombatData combatData;
    [SerializeField] int healthEnemy = 1; //for enemy
    [SerializeField] Reanimator reanimator;

    private void Awake() {
        reanimator.AddListener("ActionFinished", ActionFinished);
        reanimator.AddListener("AttackHit", AttackHit);
        healthEnemy = combatData.currentHealth;
    }

    internal bool isActionFinished;
    private void ActionFinished() => isActionFinished = true;


    private Combat target;
    private int damage;
    public void SetTarget(GameObject target, int damage) {
        this.target = target.GetComponent<Combat>();
        this.damage = damage;
    }

    private void AttackHit() {
        StartCoroutine(target.IsAttacked(damage));
        Debug.Log("Hit!");
    }

    public void PerformAttack(int attackKey) {
        combatData.currentEnergy -= combatData.movesets[attackKey].energyCost;
        reanimator.Set("State", attackKey+1);
    }


    IEnumerator IsAttacked(int damageTaken) {
        if(CompareTag("Enemy")) 
            healthEnemy -= damageTaken;
        else 
            combatData.currentHealth -= damageTaken;

        reanimator.Renderer.color = Color.red;
        transform.DOShakePosition(1);
        yield return null;
        
        reanimator.Renderer.color = Color.white;
        isActionFinished = false;
        CheckHealth();
    }

    public bool isAlive = true;
    private void CheckHealth() {
        if(combatData.currentHealth <= 0) {
            reanimator.Set("State", 4); //dead
            isAlive = false;
        }
        if(healthEnemy <= 0) {
            reanimator.Renderer.color = Color.clear;
            isAlive = false;
        }
    }
}
