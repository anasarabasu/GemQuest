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
    private List<GameObject> heroes = new();
    public List<GameObject> remainingHeroes = new();
    private List<GameObject> enemies = new();
    public List<GameObject> remainingEnemies = new();

    private void Start() {
        heroes = GameObject.FindGameObjectsWithTag("Hero").ToList();
        remainingHeroes = heroes;
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        remainingEnemies = enemies;

        SortTurnOrder();
        StartCoroutine(CombatLoop());
    }

    private void SortTurnOrder() {
        List<GameObject> combinedList = enemies.Concat(heroes).ToList(); //so that enemies will always go first
        turnOrder = combinedList.OrderByDescending(r => r.GetComponent<Combat>().combatData.speed).ToList();
    }

    private GameObject target;
    private Vector3 targetPos;

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

            if(hit.collider != null && hit.collider.GetComponent<Combat>().isAlive){
                target = hit.collider.gameObject;

                targetCircle.GetComponent<Image>().enabled = true;
                targetCircle.position = target.transform.position;

                pulsate = StartCoroutine(CombatUI.instance.PulsatingSkillButton());
            }
        }
    }

    private Vector3 targetOffset = new(1.5f, 0);
    [SerializeField] float dashSpeed = 0.17f;
    [SerializeField] float setRangeDistance = 4;
    internal static bool waitingForPlayerInput;
    
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
                    yield return StartCoroutine(CombatUI.instance.NoEnergy(entity.name.Replace("(Clone)", "")));

                else {
                    if(entity.CompareTag("Enemy")) {
                        turn = Turn.Enemy;

                        SelectRandomTarget();
                        moveKey = Random.Range(0, entity.combatData.movesets.Length);
                        Vector3 attackDistance = new(entity.combatData.movesets[moveKey].attackDistance, 0);
                        targetPos = new Vector3(target.transform.position.x + attackDistance.x, target.transform.position.y);
                    }
                    if(entity.CompareTag("Hero")) {
                        turn = Turn.Hero;

                        waitingForPlayerInput = true;

                        CombatUI.instance.UpdateLabelsToEntity(entity);
                        CombatUI.instance._ToggleSIRSelectionPanel();

                        entity.transform.Find("Sprite").DOJump(entity.transform.position, 2, 1, 0.5f);
                        yield return new WaitWhile(WaitForPlayerInput);
                        
                        moveKey--;
                        Vector3 attackDistance = new(entity.combatData.movesets[moveKey].attackDistance, 0);
                        targetPos = new Vector3(target.transform.position.x - attackDistance.x, target.transform.position.y);
                        targetCircle.GetComponent<Image>().enabled = false;
                    }

                    if(entity.combatData.movesets[moveKey].type == CombatData.Moveset.Type.MeleeAttack 
                    || entity.combatData.movesets[moveKey].type == CombatData.Moveset.Type.RangedAttack) {
                        
                        entity.transform.DOMove(targetPos, dashSpeed); 
                        yield return new WaitForSeconds(0.5f); //after moving to postionn

                        entity.SetTarget(target, entity.combatData.movesets[moveKey].damage);
                        entity.PerformAttack(moveKey);
                        yield return new WaitUntil(() => entity.isActionFinished);

                        entity.isActionFinished = false;
                        moveType = MoveType.None;
                        yield return new WaitForSeconds(0.5f);

                        entity.transform.DOMove(returnPos, 0.5f);  
                    }
                    if(entity.combatData.movesets[moveKey].type == CombatData.Moveset.Type.Skill)
                        Debug.Log("skill");

                    yield return new WaitForSeconds(1);
                }
            }            
            i++;
            if(i == turnOrder.Count)
                i = 0;
        }
    }

    public enum MoveType {None, SelectTarget, Item, Flee};
    private MoveType moveType;
    public void SetMoveType(MoveType type) => moveType = type;

    private bool WaitForPlayerInput() {
        if(waitingForPlayerInput) {
            if(moveType == MoveType.SelectTarget) {
                SelectTarget();
            }
            if(moveType == MoveType.Item) {
            }
            if(moveType == MoveType.Flee) {}
            
            return true;
        }
        else 
            return false;
    }

    int rememberAttackKey;
    int moveKey = 0;
    Coroutine pulsate;
    Coroutine notice;
    public void _SkillButton(int attackKey) {
        SetMoveType(MoveType.SelectTarget);

        if(pulsate != null) StopCoroutine(pulsate);
        if(notice != null) StopCoroutine(notice);

        if(target == null) {
            if(rememberAttackKey == attackKey)
                notice = StartCoroutine(CombatUI.instance.NullTarget());
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
            }
        }
    }

    ItemData selectedItem;
    internal void SelectItem(ItemData itemData) {
        selectedItem = itemData;
        CombatUI.instance.ToggleUseItemButton();
    }

    public void _UseItemButton() {
        selectedItem.inventory.amount--;
        InventorySystem.instance.UpdateInventoryUI();  
        CombatUI.instance._ToggleItemPanel();
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
