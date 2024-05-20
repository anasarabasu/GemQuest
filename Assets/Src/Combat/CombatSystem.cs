using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour {
    public static CombatSystem instance;
    private void Awake() => instance = this;

    public enum Turn {Hero, Enemy};
    public Turn turn = Turn.Enemy;

    public List<GameObject> turnOrder;
    private List<GameObject> heroes;
    public List<GameObject> remainingHeroes;
    private List<GameObject> enemies;
    public List<GameObject> remainingEnemies;

    private void Start() {
        heroes = GameObject.FindGameObjectsWithTag("Hero").ToList();
        remainingHeroes = new(heroes);
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        remainingEnemies = new(enemies);

        SortTurnOrder();
        StartCoroutine(CombatLoop());
    }

    private void SortTurnOrder() {
        List<GameObject> combinedList = enemies.Concat(heroes).ToList(); //so that enemies will always go first
        turnOrder = combinedList.OrderByDescending(r => r.GetComponent<Combat>().combatData.speed).ToList();
    }

    private GameObject target;

    private void UpdateTargets() {
        remainingHeroes.RemoveAll(hero => !hero.GetComponent<Combat>().isAlive);
        remainingEnemies.RemoveAll(enemy => !enemy.GetComponent<Combat>().isAlive);
    }

    private void SelectRandomTarget() {
        int randomPick = Random.Range(0, remainingHeroes.Count);
        target = remainingHeroes[randomPick];    
    }

    [SerializeField] Transform targetCircle;
    private void SelectTarget() {
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if(hit.collider != null && hit.collider.GetComponent<Combat>()){
                target = hit.collider.gameObject;

                targetCircle.GetComponent<Image>().enabled = true;
                targetCircle.position = target.transform.position;

                if(moveType == MoveType.SKILL) 
                    pulsate = StartCoroutine(CombatUI.instance.PulsatingSkillButton());
            }
        }
    }

    [SerializeField] float dashSpeed = 0.17f;
    [SerializeField] float setRangeDistance = 4;
    internal static bool waitingForPlayerInput;
    
    Vector3 targetPos;
        Vector3 attackDistance;
    IEnumerator CombatLoop() {
        //ambush ani or something
        yield return new WaitForSeconds(1);

        int i = 0;
        while(true) {    
            UpdateTargets();

            Combat entity = turnOrder[i].GetComponent<Combat>();
            Vector3 returnPos = entity.transform.position;

            target = null;

            yield return null;

            if(entity.isAlive) {
                if(entity.combatData.currentEnergy <= 0)
                    yield return StartCoroutine(CombatUI.instance.NoEnergyNotice(entity.name.Replace("(Clone)", "")));

                else {
                    if(entity.CompareTag("Enemy")) {
                        turn = Turn.Enemy;

                        SelectRandomTarget();
                        entityState = State.UseSkill;

                        moveKey = Random.Range(0, entity.combatData.movesets.Length);
                        attackDistance = new(entity.combatData.movesets[moveKey].attackDistance, 0);
                        targetPos = new Vector3(target.transform.position.x + attackDistance.x, target.transform.position.y);
                    }
                    if(entity.CompareTag("Hero")) {
                        turn = Turn.Hero;
                        CombatUI.instance.UpdateLabelsToEntity(entity);
                        CombatUI.instance._ToggleSIRSelectionPanel();

                        entity.transform.Find("Sprite").DOJump(entity.transform.position, 2, 1, 0.5f);

                        waitingForPlayerInput = true;
                        entityState = State.WaitingForAction;

                        yield return new WaitWhile(WaitForPlayerInput);
                        
                        if(entityState == State.UseSkill) {
                            moveKey--;
                            attackDistance = new(-entity.combatData.movesets[moveKey].attackDistance, 0);
                        }

                        targetCircle.GetComponent<Image>().enabled = false;
                    }

                    if(entityState == State.UseSkill) {
                        targetPos = new Vector3(target.transform.position.x + attackDistance.x, target.transform.position.y);
                        
                        entity.transform.DOMove(targetPos, dashSpeed); 
                        yield return new WaitForSeconds(0.5f); //after moving to postionn

                        entity.SetTarget(target, entity.combatData.movesets[moveKey].damage);
                        entity.PerformAttack(moveKey);
                        yield return new WaitUntil(() => entity.isActionFinished);

                        entity.isActionFinished = false;
                        entityState = State.WaitingForAction;
                        yield return new WaitForSeconds(0.5f);

                        entity.transform.DOMove(returnPos, 0.5f);  
                    }

                    if(entityState == State.UseItem) {
                        //do the animation or something like throw or heal

                    }

                    yield return new WaitForSeconds(1);
                }
            }            
            i++;
            if(i == turnOrder.Count)
                i = 0;
        }
    }

    public enum MoveType {SKILL, ITEM}
    MoveType moveType;

    public enum State {WaitingForAction, SelectTarget, UseSkill, UseItem, Flee};
    private State entityState;
    private bool WaitForPlayerInput() {
        if(waitingForPlayerInput) {
            if(entityState == State.SelectTarget) {
                SelectTarget();
            }
            if(entityState == State.UseItem) {
                ///player will have to learn if it can attack or heal
                
            }
            if(entityState == State.Flee) {}
            
            return true;
        }
        else 
            return false;
    }

    private void ChangeTeamTargetSelection(bool value) {
        foreach(GameObject hero in heroes) 
            hero.GetComponent<Collider2D>().enabled = value;
        
    }

    int rememberAttackKey;
    int moveKey = 0;
    Coroutine pulsate;
    Coroutine notice;
    public void _SkillButton(int attackKey) {
        ChangeTeamTargetSelection(false);

        moveType = MoveType.SKILL;
        entityState = State.SelectTarget;

        if(pulsate != null) StopCoroutine(pulsate);
        if(notice != null) StopCoroutine(notice);

        if(target == null) {
            if(rememberAttackKey != attackKey) {
                notice = StartCoroutine(CombatUI.instance.SelectTargetNotice());
            }
            else 
                notice = StartCoroutine(CombatUI.instance.NoTargetNotice());

            rememberAttackKey = attackKey;
        }
        else {
            if(rememberAttackKey != attackKey) {
                targetCircle.GetComponent<Image>().enabled = false;
                target = null;
                rememberAttackKey = attackKey;
            }
            else {
                moveKey = attackKey;
                rememberAttackKey = 0;
    
                CombatUI.instance._ToggleSkillPanel();

                waitingForPlayerInput = false;
                entityState = State.UseSkill;
            }
        }
    }

    ItemData selectedItem;
    GameObject combatItemViewTemp;

    internal void SelectItem(ItemData itemData) {
        ChangeTeamTargetSelection(true);

        notice = StartCoroutine(CombatUI.instance.SelectTargetNotice());
        moveType = MoveType.ITEM;
        entityState = State.SelectTarget;

        if(selectedItem != itemData) {
            Destroy(combatItemViewTemp);

            targetCircle.GetComponent<Image>().enabled = false;
            target = null;
            selectedItem = itemData;

            Combat.instance.UpItemAni();
            return;
        }

        selectedItem = itemData;
        
        Combat.instance.UpItemAni();
        CombatUI.instance.UpdateItemText(itemData.name);
    }

    Tween floatingTweenTemp;
    public void CreateItem(bool HelsIsAlive) {
        combatItemViewTemp = new GameObject("Item", typeof(SpriteRenderer));
        combatItemViewTemp.transform.localScale = new Vector3(0.5f, 0.5f);
        combatItemViewTemp.GetComponent<SpriteRenderer>().sprite = selectedItem.sprite;

        if(HelsIsAlive) 
            combatItemViewTemp.transform.position = heroes.Find(hero => hero.name == "Hels(Clone)").transform.position + new Vector3(-0.85f, 4.32f);
        else {
            combatItemViewTemp.transform.position = heroes.Find(hero => hero.name == "Hels(Clone)").transform.position + new Vector3(-2.41f, 2.72f);
            combatItemViewTemp.GetComponent<SpriteRenderer>().sortingOrder = 4;
            floatingTweenTemp = combatItemViewTemp.transform.DOMoveY(combatItemViewTemp.transform.position.y - 1, 1).SetLoops(-1, LoopType.Yoyo);
        }
        
    }



    public void _UseItemButton() {
        entityState = State.UseItem;

        if(target == null) {
            notice = StartCoroutine(CombatUI.instance.NoTargetNotice()); 
            entityState = State.SelectTarget;
            return;
        }
        Combat.instance.HelsBackToIdle();
        CombatUI.instance._ToggleItemPanel();
        InventorySystem.instance.UpdateInventoryUI();  

        StartCoroutine(ItemToss());
    }

    public IEnumerator ItemToss() {
        floatingTweenTemp.Kill();
        combatItemViewTemp.GetComponent<SpriteRenderer>().sortingOrder = 4;
        combatItemViewTemp.transform.DOJump(target.transform.position, 10, 1, 0.5f);
        yield return new WaitForSeconds(1);

        Destroy(combatItemViewTemp);
        selectedItem.UseItem(target.GetComponent<Combat>());
        selectedItem.inventoryAmount--;
        waitingForPlayerInput = false;
    }

    public void _Flee() {}

    private void Update() => CheckBattleOutcome();

    private void CheckBattleOutcome() {
        if(remainingHeroes.Count == 0) {
            StopAllCoroutines();
            SceneHandler.LoadScene("Overworld"); //cutscene -> overworld -> first level

            Debug.Log("battle lose");
        }
        if(remainingEnemies.Count == 0) {
            StopAllCoroutines();
            SceneHandler.LoadScene("Czechia"); //load previous scene

            Debug.Log("battle win");
        }
    }

   
}
