using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Aarthificial.Reanimation;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    IEnumerator MoveTowardsHeroes() {
        foreach(var enemy in remainingEnemies) {
            enemy.transform.DOMove(remainingHeroes[0].transform.position, 100);
        }

        yield return null;
    }

    private void SortTurnOrder() {
        List<GameObject> combinedList = enemies.Concat(heroes).ToList(); //so that enemies will always go first
        turnOrder = combinedList.OrderByDescending(r => r.GetComponent<Combat>().combatData.speed).ToList();
    }

    public GameObject target;

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
    Combat entity;
    IEnumerator CombatLoop() {
        //ambush ani or something
        yield return new WaitForSeconds(1);

        int i = 0;

        while(true) {    
            UpdateTargets();

            entity = turnOrder[i].GetComponent<Combat>();
            Vector3 returnPos = entity.transform.position;
            target = null;

            yield return null;


            if(!entity.isAlive) {
                if(entity.CompareTag("Hero")) {
                    entity.CheckHealth();
                    yield return StartCoroutine(CombatUI.instance.HeroDown(entity.name.Replace("(Clone)", "")));
                    entity.stunDuration = 0;
                    entity.combatData.currentEnergy++;
                }
                
                i++;
                if(i == turnOrder.Count) i = 0;
                continue;
            }

            if(entity.stunDuration != 0) {
                entity.transform.DOShakePosition(1);
                yield return StartCoroutine(CombatUI.instance.EntityIsStunned(entity.name.Replace("(Clone)", "")));
                entity.stunDuration--;

                i++;
                if(i == turnOrder.Count) i = 0;
                continue;
            }

            if(entity.combatData.currentEnergy <= 0) {
                yield return StartCoroutine(CombatUI.instance.NoEnergyNotice(entity.name.Replace("(Clone)", "")));
                
                i++;
                if(i == turnOrder.Count) i = 0;
                continue;
            }

            if(entity.CompareTag("Enemy")) {
                turn = Turn.Enemy;

                SelectRandomTarget();
                entityState = State.UseSkill;

                moveKey = Random.Range(0, entity.combatData.movesets.Length);
                attackDistance = new(entity.combatData.movesets[moveKey].attackDistance, 0);
                targetPos = new Vector3(target.transform.position.x + attackDistance.x, target.transform.position.y);
            }
            if(entity.CompareTag("Hero")) {
                // foreach(var enemy in remainingEnemies) {
                //     enemy.transform.DOBlendableMoveBy(entity.transform.position, 180);
                // }

                turn = Turn.Hero;
                CombatUI.instance.UpdateLabelsToEntity(entity);
                CombatUI.instance._ToggleSIRSelectionPanel();

                entity.transform.Find("Sprite").DOJump(entity.transform.position, 2, 1, 0.5f);
                entity.transform.Find("Turn Indicator").gameObject.SetActive(true);

                waitingForPlayerInput = true;
                entityState = State.WaitingForAction;

                yield return new WaitWhile(WaitForPlayerInput);

                if(entityState == State.UseSkill) {
                    moveKey--;
                    attackDistance = new(-entity.combatData.movesets[moveKey].attackDistance, 0);
                }

                entity.transform.Find("Turn Indicator").gameObject.SetActive(false);
                targetCircle.GetComponent<Image>().enabled = false;
            }

            if(entityState == State.UseSkill) {
                targetPos = new Vector3(target.transform.position.x + attackDistance.x, target.transform.position.y);
                
                entity.transform.DOMove(targetPos, dashSpeed); 
                yield return new WaitForSeconds(0.5f); //after moving to postionn

                entity.SetTarget(target, entity.combatData.movesets[moveKey].damage, 0); //set effects

                entity.PerformAttack(moveKey);

                yield return new WaitUntil(() => entity.isActionFinished);

                entity.isActionFinished = false;
                entityState = State.WaitingForAction;
                yield return new WaitForSeconds(0.5f);

                entity.transform.DOMove(returnPos, 0.5f);  
            }
            
            yield return new WaitForSeconds(1);

            i++;
            if(i == turnOrder.Count) i = 0;
        }
    }

    public enum MoveType {SKILL, ITEM}
    public MoveType moveType;

    public void _SetMoveTypeToSKill() => moveType = MoveType.SKILL;
    public void _SetMoveTypeToItem() => moveType = MoveType.ITEM;

    public enum State {WaitingForAction, SelectTarget, SelectItem, UseSkill, UseItem, Flee};
    public State entityState;
    private bool WaitForPlayerInput() {
        if(waitingForPlayerInput) {
            if(entityState == State.SelectTarget) {
                SelectTarget();
            }

            if(target && moveType == MoveType.SKILL && entityState == State.SelectTarget) { //showonly if there are items
                CombatUI.instance._ToggleUSWIButton(true);
                entityState = State.WaitingForAction;
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
    public int moveKey = 0;
    Coroutine pulsate;
    Coroutine notice;
    public void _SkillButton(int attackKey) {
        ChangeTeamTargetSelection(false);

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
                CombatUI.instance._ToggleUSWIButton(false);

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

    public void _UseSkillWithItem() {
        CombatUI.instance._ToggleItemPanel();
        CombatUI.instance._ToggleSkillPanel();
        CombatUI.instance._ToggleUSWIButton(false);
        entityState = State.SelectItem;
    }

    public void _CancelSKill() {
        target = null;
        targetCircle.GetComponent<Image>().enabled = false;
        
        CombatUI.instance._ToggleUSWIButton(false);
        CombatUI.instance._ToggleSkillPanel();
        CombatUI.instance._ToggleSIRSelectionPanel();
    }

    public ItemData selectedItem;
    internal void SelectItem(ItemData itemData) {
        ChangeTeamTargetSelection(true);

        if(selectedItem != itemData) {
            DeleteItemTemp();

            // targetCircle.GetComponent<Image>().enabled = false;
            // target = null;
            selectedItem = itemData;

            Combat.instance.UpItemAni();
            CombatUI.instance.UpdateItemText(itemData.name, "TBA");
            return;
        }

        selectedItem = itemData;
        
        Combat.instance.UpItemAni();
        CombatUI.instance.UpdateItemText(itemData.name, "TBA");
    }

    public void _CancelItemSelection() {
        if(moveType == MoveType.ITEM) {
            selectedItem = null;
            DeleteItemTemp();

            Combat.instance.HelsBackToIdle();
            CombatUI.instance._ToggleItemPanel();
            CombatUI.instance._MoveCameraDown(false);
            CombatUI.instance._ToggleSIRSelectionPanel();
        }
    }

    public void _UseItemOnHeroThenAttack() {
        if(moveType != MoveType.SKILL) 
            return;
        
        CombatUI.instance._ToggleItemPanel();
        moveKey = rememberAttackKey;

        StartCoroutine(ItemTossThenSkill(entity.transform.position)); //make a new one
    }

     public IEnumerator ItemTossThenSkill(Vector3 targetToTossItem) {
        targetCircle.GetComponent<Image>().enabled = false;
        floatingTweenTemp.Kill();

        tempraryItemOBject.GetComponent<SpriteRenderer>().sortingOrder = 4;
        tempraryItemOBject.transform.DOJump(targetToTossItem, 10, 1, 0.5f);
        yield return new WaitForSeconds(1);

        Combat.instance.HelsBackToIdle();
        DeleteItemTemp();
        // selectedItem.UseItem(target.GetComponent<Combat>());
        selectedItem.inventoryAmount--;
        waitingForPlayerInput = false;

        entityState = State.UseSkill;
        //set effects
    }

    public void _ItemSelectTarget() {
        if(moveType != MoveType.ITEM)
            return;

        if(notice != null) StopCoroutine(notice);

        if(selectedItem != null) {
            entityState = State.SelectTarget;
            notice = StartCoroutine(CombatUI.instance.SelectTargetNotice());
            CombatUI.instance._MoveCameraDown(false);
            CombatUI.instance._ToggleSelectTargetToUseItemOn();
        }
        else {
            notice = StartCoroutine(CombatUI.instance.NoItemSelectedNotice());
            entityState = State.WaitingForAction;
        }
    }

    public void _CancelItemTargetSelection () {
        entityState = State.WaitingForAction;
        selectedItem = null;
        target = null;
        targetCircle.GetComponent<Image>().enabled = false;
        DeleteItemTemp();
        CombatUI.instance._ToggleItemPanel();
        CombatUI.instance._ToggleSelectTargetToUseItemOn();
    }

    public void DeleteItemTemp() {
        Destroy(tempraryItemOBject);
    }

    [SerializeField] GameObject combatItemViewTemp;
    GameObject tempraryItemOBject;
    Tween floatingTweenTemp;
    public void CreateItem(bool HelsIsAlive) {
        tempraryItemOBject = Instantiate(combatItemViewTemp);

        tempraryItemOBject.transform.localScale = new Vector3(0.5f, 0.5f);
        tempraryItemOBject.GetComponent<SpriteRenderer>().sprite = selectedItem.sprite;

        if(HelsIsAlive) {
            tempraryItemOBject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            tempraryItemOBject.transform.position = heroes.Find(hero => hero.name == "Hels(Clone)").transform.position + new Vector3(-0.85f, 4.32f);
        }
        else {
            tempraryItemOBject.transform.position = heroes.Find(hero => hero.name == "Hels(Clone)").transform.position + new Vector3(-2.41f, 2.72f);
            tempraryItemOBject.GetComponent<SpriteRenderer>().sortingOrder = 4;
            floatingTweenTemp = tempraryItemOBject.transform.DOMoveY(tempraryItemOBject.transform.position.y - 1, 1).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void _UseItemButton() {
        entityState = State.UseItem;

        if(target == null) {
            notice = StartCoroutine(CombatUI.instance.NoTargetNotice()); 
            entityState = State.SelectTarget;
            return;
        }

        CombatUI.instance._ToggleSelectTargetToUseItemOn();
        InventorySystem.instance.UpdateInventoryUI();  

        StartCoroutine(ItemTossThenUse(target.transform.position));
    }

    public IEnumerator ItemTossThenUse(Vector3 targetToTossItem) {
        targetCircle.GetComponent<Image>().enabled = false;
        floatingTweenTemp.Kill();

        tempraryItemOBject.GetComponent<SpriteRenderer>().sortingOrder = 4;
        tempraryItemOBject.transform.DOJump(targetToTossItem, 10, 1, 0.5f);
        yield return new WaitForSeconds(1);

        Combat.instance.HelsBackToIdle();
        DeleteItemTemp();
        selectedItem.UseItem(target.GetComponent<Combat>());
        selectedItem.inventoryAmount--;
        waitingForPlayerInput = false;
    }

    public void _Flee() {}

    private void Update() {
        CheckBattleOutcome();
        Debug.Log(entityState);
    }

    private void CheckBattleOutcome() {
        if(remainingHeroes.Count == 0) {
            StopAllCoroutines();
            SceneManager.LoadSceneAsync("Overworld");

            Debug.Log("battle lose");
        }
        if(remainingEnemies.Count == 0) {
            StopAllCoroutines();
            SceneHandler.LoadScene("Czechia"); //load previous scene

            Debug.Log("battle win");
        }
    }

   
}
