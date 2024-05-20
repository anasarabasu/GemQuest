using Aarthificial.Reanimation;
using UnityEngine;

public class AnimateRoam : MonoBehaviour {
    protected Reanimator reanimator;
    [SerializeField] Rigidbody2D body;

    private enum Facing {FRONT = 0, BACK = 1, LEFT = 2, RIGHT = 3}
    private int facingDirection;

    private void Awake() {
        reanimator = GetComponent<Reanimator>();
    }

    protected void UpdateDirection(Vector3 velocity) {
        if(velocity.y < -0.1) 
            facingDirection = (int)Facing.FRONT;
        else if(velocity.y > 0.1) 
            facingDirection = (int)Facing.BACK;

        if(velocity.x < -0.8) 
            facingDirection = (int)Facing.LEFT;
        else if(velocity.x > 0.8) 
            facingDirection = (int)Facing.RIGHT;

        reanimator.Set("Direction", facingDirection);
    }

    private enum State {IDLE = 0, WALK = 1};
    internal virtual void UpdateState() {   
        if(body.IsAwake()) 
            reanimator.Set("AnimationState", (int)State.WALK);
        else
            reanimator.Set("AnimationState", (int)State.IDLE);
    }

    private void Step() {

    }
}
