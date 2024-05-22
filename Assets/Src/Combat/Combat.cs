using System;
using System.Collections;
using System.Collections.Generic;
using Aarthificial.Reanimation;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Combat : MonoBehaviour {
    public EntityStatData combatData;
    [SerializeField] GameObject floatTextPrefab;
    [SerializeField] int healthEnemy = 1; //for enemy
    [SerializeField] Reanimator reanimator;

    private void Awake() {
        instance = this;

        reanimator.AddListener("ActionFinished", ActionFinished);
        reanimator.AddListener("AttackHit", AnimationAttackHit);
        reanimator.AddListener("ShowItem", ShowItem);
        healthEnemy = combatData.currentHealth;

        entities = FindObjectsOfType<Combat>();
    }

    public void ShowItem() => CombatSystem.instance.CreateItem(true);

    internal bool isActionFinished;
    private void ActionFinished() => isActionFinished = true;


    private Combat target;
    private int damageFromAttacker;
    private int stunFromAttacker;
    public void SetTarget(GameObject target, int damage, int stun) {
        this.target = target.GetComponent<Combat>();
        damageFromAttacker = damage;
        stunFromAttacker = stun;
    }
    public void PerformAttack(int attackKey) {
        combatData.currentEnergy -= combatData.movesets[attackKey].energyCost;
        reanimator.Set("State", attackKey+1);
    }

    private void AnimationAttackHit() {
        StartCoroutine(target.IsAttacked(damageFromAttacker, 0));
        Debug.Log("Hit!");
    }

    public static Combat instance;

    Combat[] entities;
    public void SearchItemAni () { //hels searching ani
        foreach(var obj in entities) {
            if(obj.name.Replace("(Clone)", "") == "Hels") {
                if(obj.isAlive) {
                    obj.reanimator.Set("State", 5);
                    obj.reanimator.Set("Item", 0);
                }
            }
        }
    }

    public void UpItemAni () {
        foreach(var obj in entities) 
            if(obj.name.Replace("(Clone)", "") == "Hels") {
                if(obj.isAlive) {
                    obj.reanimator.Set("Action", 0);
                    obj.reanimator.Set("Item", 1);
                }
                else {
                    CombatSystem.instance.CreateItem(false);
                }
            }
    }

    public void HelsBackToIdle() {
        foreach(var obj in entities) 
            if(obj.name.Replace("(Clone)", "") == "Hels") 
                if(obj.isAlive)
                    obj.reanimator.Set("State", 0);
    }

    public void Damage(int amount, int stunAmount) => StartCoroutine(IsAttacked(amount, stunAmount));

    float flickerSpeed = 0.1f;
    IEnumerator IsAttacked(int damageAmount, int stunAmount) {
        if(damageAmount == 0) 
            yield break;

        GameObject damageFloat = Instantiate(floatTextPrefab, transform.position, Quaternion.identity,GameObject.FindWithTag("Floating Text").transform);
        damageFloat.GetComponentInChildren<TextMeshProUGUI>().SetText("-" +damageAmount);
        Destroy(damageFloat, 4);

        GameObject effectGO = Instantiate(CombatEffects.instance.damage, transform.position, Quaternion.identity, transform);
        var effect = effectGO.GetComponent<ParticleSystem>();
        effect.Play();
        Destroy(effectGO, effect.main.duration);

        if(CompareTag("Enemy")) 
            healthEnemy -= damageAmount;
        else {
            if(combatData.currentEnergy > 0)
                combatData.currentHealth -= damageAmount;
        }

        stunDuration += stunAmount;

        transform.DOShakePosition(1);
        for(int i = 0; i < 4; i++) { 
            reanimator.Renderer.color = Color.red;
            yield return new WaitForSeconds(flickerSpeed);
            reanimator.Renderer.color = Color.white;
            yield return new WaitForSeconds(flickerSpeed);
        }
        yield return null;

        if(damageAmount <= 3) {
            GameObject textFloat = Instantiate(floatTextPrefab, transform.position, Quaternion.identity,GameObject.FindWithTag("Floating Text").transform);
            textFloat.GetComponentInChildren<TextMeshProUGUI>().SetText("Not effective!");
            Destroy(textFloat, 4);
        }
        if(damageAmount >= 20) {
            GameObject textFloat = Instantiate(floatTextPrefab, transform.position, Quaternion.identity,GameObject.FindWithTag("Floating Text").transform);
            textFloat.GetComponentInChildren<TextMeshProUGUI>().SetText("Super effective!");
            Destroy(textFloat, 4);
        }
        
        reanimator.Renderer.color = Color.white;
        isActionFinished = false;
        CheckHealth();
    }

    public void Heal(int amount) => StartCoroutine(IsHealed(amount));

    IEnumerator IsHealed(int healAmount) {
        if(healAmount == 0) 
            yield break;
            
        GameObject effectGO = Instantiate(CombatEffects.instance.heal, transform.position, Quaternion.identity, transform);
        var effect = effectGO.GetComponent<ParticleSystem>();
        effect.Play();

        if(CompareTag("Enemy")) 
            healthEnemy += healAmount;
        else 
            combatData.currentHealth += healAmount;

        //use heal ani or something

        for(int i = 0; i < 4; i++) { 
            reanimator.Renderer.color = Color.green;
            yield return new WaitForSeconds(flickerSpeed);
            reanimator.Renderer.color = Color.white;
            yield return new WaitForSeconds(flickerSpeed);

        }
        yield return null;

        Destroy(effectGO, effect.main.duration);
        isActionFinished = false;
        CheckHealth();

    }

    public int stunDuration;
    //is stunned
    //is electrocuted

    public bool isAlive = true;

    public void CheckHealth() {
        if(gameObject.tag == "Hero") {
            if(combatData.currentHealth <= 0) {
                reanimator.Set("State", 4); //dead
                reanimator.Renderer.color = Color.white;
                isAlive = false;
                combatData.currentEnergy = 0;
            }
            else {
                reanimator.Set("State", 0); //dead
                isAlive = true;
            }
        }
        if(gameObject.tag == "Enemy") {
            if(healthEnemy <= 0) {
                reanimator.Renderer.color = Color.clear;
                isAlive = false;
            }
        }
    }
}
