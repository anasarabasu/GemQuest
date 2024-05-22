using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mine : MonoBehaviour {
    [SerializeField] EntityStatData pikStats;
    [SerializeField] RectTransform noticePanel;

    private void Update() {
        UpdateObjectInRange();
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
                
                tired = StartCoroutine(PikIsTired());
            }
        }
    }

    Coroutine tired;
    IEnumerator PikIsTired() { //me too
        noticePanel.DOAnchorPosY(12.75f, 0.25f);
        yield return new WaitForSeconds(1);
        
        noticePanel.DOAnchorPosY(-12.2f, 0.5f);
    }

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

    public void FinishMining() {
        if(objInRange != null) {
            pikStats.currentEnergy--;
            objInRange.GetMined();
        }

        if(!mineRequest) {
            Joystick.MovementState(true);
            StartMining = false;
        }
    }    
}
