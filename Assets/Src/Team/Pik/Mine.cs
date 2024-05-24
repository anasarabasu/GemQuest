using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using VolFx;

public class Mine : MonoBehaviour, ISaveable {
    [SerializeField] EntityStatData pikStats;
    [SerializeField] RectTransform noticePanel;

    public static Mine instance;

    private void Awake() => instance = this;
    private void Update() {
        UpdateObjectInRange();

        if(pikStats.currentEnergy < 0) {
            mineRequest = false;
        }
    }

    private bool mineRequest = false;
    [HideInInspector] public bool StartMining;
    
    public void OnMine(InputAction.CallbackContext context) {
        mineRequest = context.performed;

        if(mineRequest) {
            if(pikStats.currentEnergy > 0) {
                Joystick.MovementState(false);
                StartMining = true;
            }
            else {
                if(tired != null)
                    StopCoroutine(tired);
                
                tired = StartCoroutine(NoticePanel.instance.ShowNotice("Pik is tired..."));
            }
        }
        
    }

    Coroutine tired;

    private RaycastHit2D hit;
    private LevelObject objInRange;
    private void UpdateObjectInRange() {
        Vector2 rayOrigin = new(transform.position.x, transform.position.y -1);
        hit = Physics2D.Raycast(rayOrigin, Joystick.UpdateRay(), 3, LayerMask.GetMask("Destroyable"));
        
        Debug.DrawRay(rayOrigin, Joystick.UpdateRay());
        
        if(hit.collider != null)
            objInRange = hit.collider.gameObject.GetComponent<LevelObject>();
        else
            objInRange = null;
    }

    private void OnTriggerEnter2D(Collider2D obj) {
        if(obj.tag == "Destroyable")
            objInRange = obj.gameObject.GetComponent<LevelObject>();
    }

    private void OnTriggerExit2D(Collider2D obj) {
        if(obj.tag == "Destroyable")
            objInRange = null;
    }

    public void FinishMining() {
        if(objInRange != null) {
            objInRange.GetMined(pikStats.toolLevel);
            pikStats.currentEnergy--;
            if(pikStats.currentEnergy < 0)
                pikStats.currentEnergy = 0;
        }

        if(!mineRequest) {
            Joystick.MovementState(true);
            StartMining = false;
        }
    }

    public void Save(DataRoot data) {}

    public void Load(DataRoot data) {
        // pickaxeLevel = data.characterStats.pickaxeLevel;
    }
}
