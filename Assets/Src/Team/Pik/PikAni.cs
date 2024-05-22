using UnityEngine;

public class PikAni : AnimateRoam {
    [SerializeField] Mine mine;
    private void Start() {
        reanimator.AddListener("MiningEndFrame", PickaxeHit);
    }

    private void Update() {
        UpdateDirection(Joystick.GetDirection());
        UpdateState();
    }

    private const int MINE = 2;

    internal override void UpdateState() {
        if(mine.StartMining)
            reanimator.Set("AnimationState", MINE);
        else 
            base.UpdateState();
    }

    private void PickaxeHit() => mine.FinishMining();
}
