using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour {
    [SerializeField] Joystick controller;

    private void Update() {
        UpdateRay();

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

    private Vector3 rayDirection;
    private RaycastHit2D hit;
    private Interactable objInRange;
    private enum Facing {FRONT = 0, BACK = 1, LEFT = 2, RIGHT = 3}
    private float offset = 1.75f;
    private void UpdateRay() {
        switch (controller.FacingDirection()) {
            case (int)Facing.FRONT: 
                rayDirection = new Vector3(0, -1.5f + -offset);
                break;
            case (int)Facing.BACK: 
                rayDirection = new Vector3(0, -1.5f + offset);
                break;
            case (int)Facing.LEFT: 
                rayDirection = new Vector3(0 + -offset, -1.5f);
                break;
            case (int)Facing.RIGHT: 
                rayDirection = new Vector3(0 + offset, -1.5f);
                break;
        }
        
        hit = Physics2D.Raycast(transform.position, rayDirection, 3, LayerMask.GetMask("UseRaycast"));
        // Debug.DrawRay(transform.position, rayDirection);
        
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
