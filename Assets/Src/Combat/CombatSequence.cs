using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aarthificial.Reanimation;
using DG.Tweening;
using UnityEngine;

public class CombatSequence : MonoBehaviour {
    public bool battleOver = false;
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
        turnOrder = combinedList.OrderByDescending(r => r.GetComponent<CombatStats>().speed).ToList();
    }

    private GameObject target;
    private Vector3 targetPos;

    private void UpdateTargets() {
        remainingHeroes.RemoveAll(hero => !hero.GetComponent<CombatStats>().isAlive);
        remainingEnemies.RemoveAll(enemy => !enemy.GetComponent<CombatStats>().isAlive);
    }

    private void SelectRandomTarget() {
        int randomPick = Random.Range(0, remainingHeroes.Count);
        target = remainingHeroes[randomPick];    
    }

    private void SelectTarget() {
        Debug.Log("waiting");
        if(target == null) {
            foreach(GameObject entity in enemies) {
                if(entity.GetComponent<CombatStats>().isAlive) {
                    target = entity;
                    target.GetComponent<Reanimator>().Renderer.color = Color.green; 
                    break;
                }
            }
        }
        
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if(hit.collider != null && hit.collider.GetComponent<CombatStats>().isAlive){
                foreach(GameObject entity in enemies) { //change to some sort of pointer instead of colour
                    if(entity.GetComponent<CombatStats>().isAlive)
                        entity.GetComponent<Reanimator>().Renderer.color = Color.white;
                }
                target = hit.collider.gameObject;
                target.GetComponent<Reanimator>().Renderer.color = Color.green;
            }
        }
    }

    private Vector3 stepForwardOffset = new(1.5f, 0);
    private Vector3 targetOffset = new(1.5f, 0);
    private float dashSpeed = 0.17f;
    private bool waitingForPlayerInput;

    IEnumerator CombatLoop() {
        int i = 0;
        //play ambush cutscene
        
        yield return new WaitForSeconds(1);

        Debug.Log("Combat start");
        while(true) {    
            CombatStats entity = turnOrder[i].GetComponent<CombatStats>();
            Vector3 returnPos = entity.transform.position;
            
            UpdateTargets();
            target = null;
            yield return null;

            if(entity.isAlive) {
                if(entity.CompareTag("Enemy")) {
                    turn = Turn.Enemy;

                    SelectRandomTarget();
                    targetPos = target.transform.position + targetOffset;
                }
                
                if(entity.CompareTag("Hero")) {
                    turn = Turn.Hero;

                    waitingForPlayerInput = true;

                    Vector3 stepForward = entity.transform.position + stepForwardOffset;
                    entity.transform.DOMoveX(stepForward.x, dashSpeed);
                    yield return null;

                    while(waitingForPlayerInput) {
                        //show buttons v
                        
                        //skill v
                        //action v
                        //attack v
                        SelectTarget();
                        //etc...

                        //item v
                        //consumeitem

                        //flee v
                        //apply penalty

                        yield return null;
                    }
                    targetPos = target.transform.position - targetOffset;

                    waitingForPlayerInput = false;
                }

                if(entity.attackTypes[0].type == AttackTypes.Type.Melee) //change attack
                    entity.transform.DOMove(targetPos, dashSpeed); 

                if(entity.attackTypes[0].type == AttackTypes.Type.Ranged) 
                    entity.transform.DOMoveY(targetPos.y, dashSpeed);    

                yield return new WaitForSeconds(0.14f); 
                entity.PerformAttack(target, entity.attackTypes[0]);

                yield return new WaitForSeconds(0.5f); //wait for ani finish

                entity.transform.DOMove(returnPos, 0.5f);  
                yield return new WaitForSeconds(1);
            }            
            i++;
            if(i == turnOrder.Count)
                i = 0;

            yield return null;
        }
    }

    private void Update() {
        CheckBattleOutcome();
    }

    private void CheckBattleOutcome() {
        if(remainingHeroes.Count == 0) {
            StopAllCoroutines();
            SceneHandler.LoadScene("Overworld");

            Debug.Log("battle lose");
        }
        if(remainingEnemies.Count == 0) {
            StopAllCoroutines();
            SceneHandler.LoadScene("Czechia1"); //load previous scene

            Debug.Log("battle win");
        }
    }

    public void attackbutton() {
        waitingForPlayerInput = false;
    }

    public void Item() {}

    public void Flee() {}

}
