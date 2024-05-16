using UnityEngine;
using UnityEngine.InputSystem;

public class Mine : MonoBehaviour {

    private void Update() {
        UpdateObjectInRange();
    }

    private static bool mineRequest = false;
    [HideInInspector] public static bool StartMining;
    
    public void OnMine(InputAction.CallbackContext context) {
        mineRequest = context.performed;

        if(mineRequest) {
            Joystick.MovementState(false);
            StartMining = true;
        }
    }

    private RaycastHit2D hit;
    private static Destroyable objInRange;
    private void UpdateObjectInRange() {
        Vector2 rayOrigin = new(transform.position.x, transform.position.y -1);
        hit = Physics2D.Raycast(rayOrigin, Joystick.UpdateRay(), 3, LayerMask.GetMask("Destroyable"));
        
        Debug.DrawRay(rayOrigin, Joystick.UpdateRay());
        
        if(hit.collider != null)
            objInRange = hit.collider.gameObject.GetComponent<Destroyable>();
        else
            objInRange = null;
    }

    public static void FinishMining() {
        if(objInRange != null)
            objInRange.GetMined();

        if(!mineRequest) {
            Joystick.MovementState(true);
            StartMining = false;
        }
    }    
}
