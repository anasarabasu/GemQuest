using Aarthificial.Reanimation;
using UnityEngine;

public class Animate : MonoBehaviour {
    [SerializeField] Joystick controller; //make instance 
    [SerializeField] Interact interactor;  //make instance
    [SerializeField] Reanimator reanimator;

    private enum State {IDLE = 0, WALK = 1, MINE = 2};

    private static class Drivers {
        public const string AnimationState = "AnimationState";
        public const string Direction = "Direction";
        //Footstep
        public const string PickaxeHit = "MiningEndFrame";
    }

    private void Start() {
        reanimator.AddListener(Drivers.PickaxeHit, PickaxeHit);
    }

    private void Update() {
        reanimator.Set(Drivers.Direction, controller.FacingDirection());
        UpdateState();
    }

    private void UpdateState() {   
        if(interactor.StartMining)
            reanimator.Set(Drivers.AnimationState, (int)State.MINE);
        else if(controller.IsMoving()) 
           reanimator.Set(Drivers.AnimationState, (int)State.WALK);
        else
           reanimator.Set(Drivers.AnimationState, (int)State.IDLE);
    }

    private void PickaxeHit() {
        //play sound

        interactor.FinishMining();
    }


     //playstepsound
    //playminesound
}
