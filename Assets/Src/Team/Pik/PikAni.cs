using UnityEngine;

public class PikAni : AnimateRoam {
    [SerializeField] Joystick controller;
    [SerializeField] Mine interactor;  //make instance

    private void Start() {
        reanimator.AddListener("MiningEndFrame", PickaxeHit);
    }

    private void Update() {
        UpdateDirection(controller.direction);
        UpdateState();
    }

    private const int MINE = 2;

    internal override void UpdateState() {
        if(interactor.StartMining)
            reanimator.Set("AnimationState", MINE);
        else 
            base.UpdateState();
    }


    private void PickaxeHit() {
        interactor.FinishMining();
    }
}
