using System;
using System.Collections;
using Aarthificial.Reanimation;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Combat : MonoBehaviour {
    [SerializeField] GameObject shield;
    [SerializeField] GameObject electro;
    [SerializeField] GameObject heal;
    [SerializeField] GameObject hit;
    [SerializeField] GameObject energise;
    [SerializeField] GameObject acid;
    [SerializeField] GameObject stun;
    [SerializeField] GameObject distract;


    public EntityStatData combatData;
    [SerializeField] GameObject floatTextPrefab;
    [SerializeField] float healthEnemy = 1; //for enemy
    [SerializeField] Reanimator reanimator;
    string name;


    private void Awake() {
        if(tag == "Enemy") {
            combatData.speed = Random.Range(1, 11);
            GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        }
        
        name = gameObject.name.Replace("(Clone)", "");

        instance = this;

        reanimator.AddListener("ActionFinished", ActionFinished);
        reanimator.AddListener("AttackHit", AnimationAttackHit);
        reanimator.AddListener("ShowItem", ShowItem);
        reanimator.Set("Idle", Random.Range(0, 5));

        healthEnemy = combatData.currentHealth;

        entities = FindObjectsOfType<Combat>();
    }

    public void ShowItem() => CombatSystem.instance.CreateItem(true);

    internal bool isActionFinished;
    private void ActionFinished() => isActionFinished = true;

    private Combat target;
    private int damageOfAttack;
    public void SetTarget(GameObject target, int damage) {
        this.target = target.GetComponent<Combat>();

        damageOfAttack = damage;
    }

    public void Fail() {
        stunDuration = 1;
        Stun();
        StartCoroutine(NoticePanel.instance.ShowNotice("Ows... That didn't work...", 0.5f));
    }

    public int IncreaseAttack(EntityStatData target, ItemData selectedItem) {
        if(selectedItem.combatFunction.effectiveTo == ItemData.CombatFunction.Enemy.Shade && target.entityType == EntityStatData.EntityType.Shade)
            return 5;
        
        if(selectedItem.combatFunction.effectiveTo == ItemData.CombatFunction.Enemy.Golem && target.entityType == EntityStatData.EntityType.Golem)
            return 5;

        return 1;
    }

    public int OneHit(EntityStatData target, ItemData selectedItem) {
        if(selectedItem.combatFunction.effectiveTo == ItemData.CombatFunction.Enemy.Shade && target.entityType == EntityStatData.EntityType.Shade)
            return 999999;
        
        if(selectedItem.combatFunction.effectiveTo == ItemData.CombatFunction.Enemy.Golem && target.entityType == EntityStatData.EntityType.Golem)
            return 999999;

        return 1;
    }

    public void PerformAttack(int attackKey) {
        combatData.currentEnergy -= combatData.movesets[attackKey].energyCost;
        reanimator.Set("State", attackKey+1);
    }

    private void AnimationAttackHit() {
        hit.SetActive(true);
        var effect = hit.GetComponent<ParticleSystem>();
        effect.Play();

        StartCoroutine(target.IsAttacked(damageOfAttack));
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

    float flickerSpeed = 0.1f;
    public void Damage(int amount) => StartCoroutine(IsAttacked(amount));
    public void SetHeal(int amount) => StartCoroutine(IsHealed(amount));
    IEnumerator IsAttacked(int damageAmount) {
        if(shieldHitAmount > 0) {
            shield.transform.DOShakePosition(1);
            ShieldUsed = true;
            yield break;
        }
        
        GameObject damageFloat = Instantiate(floatTextPrefab, transform.position, Quaternion.identity,GameObject.FindWithTag("Floating Text").transform);
        damageFloat.GetComponentInChildren<TextMeshProUGUI>().SetText("-" +damageAmount);
        Destroy(damageFloat, 4);

        if(CompareTag("Enemy")) 
            healthEnemy -= damageAmount;
        else {
            if(combatData.currentEnergy > 0)
                combatData.currentHealth -= damageAmount;
        }

        transform.DOShakePosition(1);
        for(int i = 0; i < 4; i++) { 
            reanimator.Renderer.color = Color.red;
            yield return new WaitForSeconds(flickerSpeed);
            reanimator.Renderer.color = Color.white;
            yield return new WaitForSeconds(flickerSpeed);
        }
        yield return null;

        // if(damageAmount <= 4) {
        //     GameObject textFloat = Instantiate(floatTextPrefab, transform.position, Quaternion.identity,GameObject.FindWithTag("Floating Text").transform);
        //     textFloat.GetComponentInChildren<TextMeshProUGUI>().SetText("Not effective...");
        //     Destroy(textFloat, 4);
        // }
        // if(damageAmount >= 20) {
        //     GameObject textFloat = Instantiate(floatTextPrefab, transform.position, Quaternion.identity,GameObject.FindWithTag("Floating Text").transform);
        //     textFloat.GetComponentInChildren<TextMeshProUGUI>().SetText("Super effective!");
        //     Destroy(textFloat, 4);
        // }

        reanimator.Renderer.color = Color.white;
        isActionFinished = false;
        CheckHealth();
    }

    IEnumerator IsHealed(int healAmount) {
        heal.SetActive(true);
        var effect = heal.GetComponent<ParticleSystem>();
        effect.Play();

        if(CompareTag("Enemy")) 
            healthEnemy += healAmount;
        else {
            combatData.currentHealth += healAmount;
            if(combatData.currentHealth >= combatData.health)
                combatData.currentHealth = combatData.health;
        }

        for(int i = 0; i < 4; i++) { 
            reanimator.Renderer.color = Color.green;
            yield return new WaitForSeconds(flickerSpeed);
            reanimator.Renderer.color = Color.white;
            yield return new WaitForSeconds(flickerSpeed);
        }
        yield return null;

        isActionFinished = false;
        CheckHealth();
    }

    public void SetEnergy(int amount, string selectedItem) => StartCoroutine(IsEnergised(amount, selectedItem));

    IEnumerator IsEnergised(int amount, string selectedItem) {
        energise.SetActive(true);
        var effect = energise.GetComponent<ParticleSystem>();
        effect.Play();

        if(amount == 1)
            StartCoroutine(NoticePanel.instance.ShowNotice($"Sold {selectedItem}!\n{gameObject.name.Replace("(Clone)", "")} was energised... with a rock?", 0.5f));
        else 
            StartCoroutine(NoticePanel.instance.ShowNotice($"Sold {selectedItem}!\n{gameObject.name.Replace("(Clone)", "")} was energised!", 0.5f));

        combatData.currentEnergy += amount;
        if(combatData.currentEnergy >= combatData.energy)
                combatData.currentEnergy = combatData.energy;

        for(int i = 0; i < 4; i++) { 
            reanimator.Renderer.color = Color.yellow;
            yield return new WaitForSeconds(flickerSpeed);
            reanimator.Renderer.color = Color.white;
            yield return new WaitForSeconds(flickerSpeed);
        }
        yield return null;
    }


    public int stunDuration;
    public void SetStun(int duration, int amount) {
        stunDuration = duration;
        Stun();
        
        StartCoroutine(IsAttacked(amount));
        if(amount == 1)
            StartCoroutine(NoticePanel.instance.ShowNotice(name + " was hit by a rock!", 0.5f));
        else 
            StartCoroutine(NoticePanel.instance.ShowNotice(name + " was dazed from the impact!", 0.5f));
    }

    public void Stun() {
        stun.SetActive(true);
        var effect = stun.GetComponent<ParticleSystem>();
        effect.Play();
        transform.DOShakePosition(1);
    }

    public int electricDuration;
    private int electricDamage;
    public void SetElectrocute(int duration, int amount) {
        electricDuration = duration;
        electricDamage = amount;

        if(combatData.entityType == EntityStatData.EntityType.Shade)
            electricDamage *= 4;
        if(combatData.entityType == EntityStatData.EntityType.Golem)
            electricDamage = 1;

        StartCoroutine(NoticePanel.instance.ShowNotice(name + " was electrocuted!", 0.5f));
        Electrocuted();
    }

    public void Electrocuted() {
        electro.SetActive(true);
        var effect = electro.GetComponent<ParticleSystem>();
        effect.Play();

        StartCoroutine(IsAttacked(Random.Range(electricDamage-4, electricDamage)));
    }


    public int shieldHitAmount;
    public void SetShield(int amount) {
        shieldHitAmount = amount;

        shield.SetActive(true);
        var effect = shield.GetComponent<ParticleSystem>();
        effect.Play();

        StartCoroutine(NoticePanel.instance.ShowNotice(name + " formed a shield!", 0.5f));
    }

    public bool ShieldUsed;

    public void UpdateShield() {
        if(ShieldUsed) {
            shieldHitAmount--;
                Debug.Log("shield");

            if(shieldHitAmount == 0) {
                Debug.Log("shield");
                var effect = shield.GetComponent<ParticleSystem>();
                shieldHitAmount = 0;
                effect.Stop();
            }
            
            ShieldUsed = false;
        }
    }

    public int acidDuration;
    public int acidDamage;
    public void SetAcid(int duration, int amount) {
        acidDuration = duration;
        acidDamage = amount;

        if(combatData.entityType == EntityStatData.EntityType.Golem)
            acidDamage *= 4;
        if(combatData.entityType == EntityStatData.EntityType.Shade)
            electricDamage = 1;

        StartCoroutine(NoticePanel.instance.ShowNotice(name + " was damaged by acid!", 0.5f));
        Acid();
    }

    public void Acid() {
        acid.SetActive(true);
        var effect = acid.GetComponent<ParticleSystem>();
        effect.Play();

        StartCoroutine(IsAttacked(acidDamage));
    }


    public int distractedDuration;
    public void SetDistraction(int amount) {
        distractedDuration = amount;
        Distracted();
    }

    public void Distracted() {
        distract.SetActive(true);
        var effect = distract.GetComponent<ParticleSystem>();
        effect.Play();
    }

    public bool isAlive = true;
    public void CheckHealth() {
        if(gameObject.tag == "Hero") {
            if(combatData.currentHealth <= 0) {
                if(gameObject.name.Contains("Pik"))
                    transform.Find("Ray").GetComponent<Light2D>().enabled = false;

                reanimator.Set("State", 4); //dead
                reanimator.Renderer.color = Color.white;
                isAlive = false;
                combatData.currentEnergy = 0;
            }
            else {
                if(gameObject.name.Contains("Pik"))
                    transform.Find("Ray").GetComponent<Light2D>().enabled = true;

                reanimator.Set("State", 0); 
                isAlive = true;
            }
        }
        if(gameObject.tag == "Enemy") {
            if(healthEnemy <= 0) {

                Destroy(GetComponent<Collider2D>());
                Destroy(GetComponent<ShadowCaster2D>());
                reanimator.Renderer.color = Color.clear;
                isAlive = false;
            }
        }
    }
}
