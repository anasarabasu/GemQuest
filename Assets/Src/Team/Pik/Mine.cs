using UnityEngine;
using UnityEngine.InputSystem;

public class Mine : MonoBehaviour {
    [SerializeField] Joystick controller;

    private void Update() {
        UpdateObjectInRange();

    }
    private bool mineRequest;
    [HideInInspector] public bool StartMining;
    public void OnMine(InputAction.CallbackContext context) {
        mineRequest = context.performed;

        if(mineRequest) {
            controller.canMove = false;
            StartMining = true;
        }
    }

    private RaycastHit2D hit;
    private Interactable objInRange;
    private void UpdateObjectInRange() {
        Vector2 rayOrigin = new(transform.position.x, transform.position.y -1);
        hit = Physics2D.Raycast(rayOrigin, controller.UpdateRay(), 3, LayerMask.GetMask("Destroyable"));
        
        Debug.DrawRay(rayOrigin, controller.UpdateRay());
        
        if(hit.collider != null)
            objInRange = hit.collider.gameObject.GetComponent<Interactable>();
        else
            objInRange = null;
    }

    public void FinishMining() {
        if(objInRange != null)
            objInRange.GetMined();

        if(!mineRequest) {
            controller.canMove = true;
            StartMining = false;
        }
    }    
}
