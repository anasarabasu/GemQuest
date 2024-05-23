using System.Collections;
using Aarthificial.Reanimation;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Combat : MonoBehaviour {
    [SerializeField] GameObject shield;
    [SerializeField] GameObject electro;
    [SerializeField] GameObject heal;
    [SerializeField] GameObject hit;


    public EntityStatData combatData;
    [SerializeField] GameObject floatTextPrefab;
    [SerializeField] float healthEnemy = 1; //for enemy
    [SerializeField] Reanimator reanimator;
    string name;

    private void Awake() {
        name = gameObject.name.Replace("(Clone)", "");

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
    public void SetTarget(GameObject target, int damage) {
        this.target = target.GetComponent<Combat>();

        if(this.target.shieldHitAmount > 0) 
            damageFromAttacker = 0;
        
        else
            damageFromAttacker = damage;
    }

    public void PerformAttack(int attackKey) {
        combatData.currentEnergy -= combatData.movesets[attackKey].energyCost;
        reanimator.Set("State", attackKey+1);
    }

    private void AnimationAttackHit() {
        hit.SetActive(true);
        var effect = hit.GetComponent<ParticleSystem>();
        effect.Play();

        StartCoroutine(target.IsAttacked(damageFromAttacker));
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

        if(damageAmount <= 4) {
            GameObject textFloat = Instantiate(floatTextPrefab, transform.position, Quaternion.identity,GameObject.FindWithTag("Floating Text").transform);
            textFloat.GetComponentInChildren<TextMeshProUGUI>().SetText("Not effective...");
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

    IEnumerator IsHealed(int healAmount) {
        GameObject effectGO = Instantiate(CombatEffects.instance.heal, transform.position, Quaternion.identity, transform);
        var effect = effectGO.GetComponent<ParticleSystem>();
        
        effect.Play();
        Destroy(effectGO, effect.main.duration);

        if(CompareTag("Enemy")) 
            healthEnemy += healAmount;
        else 
            combatData.currentHealth += healAmount;

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

    public int stunDuration;
    public void SetStun(int duration) {
        stunDuration = duration;
        StartCoroutine(CombatUI.instance.ShowNotice(name + " was dazed from the impact!"));
    }

    public int electricDuration;
    private int electricDamage;
    public void SetElectrocute(int duration, int amount) {
        electricDuration = duration;
        electricDamage = amount;

        StartCoroutine(CombatUI.instance.ShowNotice(name + " was electrocuted!"));
        Electrocuted();
    }

    public void Electrocuted() {
        GameObject effectGO = Instantiate(CombatEffects.instance.electrocute, transform.position, Quaternion.identity, transform);
        var effect = effectGO.GetComponent<ParticleSystem>();
        
        effect.Play();
        Destroy(effectGO, effect.main.duration);

        StartCoroutine(IsAttacked(electricDamage));
    }


    public int shieldHitAmount;
    public void SetShield(int amount) {
        shieldHitAmount = amount;

        shield.SetActive(true);
        var effect = shield.GetComponent<ParticleSystem>();
        effect.Play();

        StartCoroutine(CombatUI.instance.ShowNotice(name + " formed a shield!"));
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

    public int acidAmount;
    public void SetAcid(int amount) {
        acidAmount = amount;

        StartCoroutine(CombatUI.instance.ShowNotice(name + " was damaged by acid!"));
        
    }

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
