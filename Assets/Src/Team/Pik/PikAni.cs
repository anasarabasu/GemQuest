using UnityEngine;

public class PikAni : AnimateRoam {
    private void Start() {
        reanimator.AddListener("MiningEndFrame", PickaxeHit);
    }

    private void Update() {
        UpdateDirection(Joystick.GetDirection());
        UpdateState();
    }

    private const int MINE = 2;

    internal override void UpdateState() {
        if(Mine.StartMining)
            reanimator.Set("AnimationState", MINE);
        else 
            base.UpdateState();
    }

    private void PickaxeHit() {
        Mine.FinishMining();
    }
}
