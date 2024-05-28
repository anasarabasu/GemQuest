using System.Collections.Generic;
using Aarthificial.Reanimation;
using UnityEngine;

public class PikAni : AnimateRoam {
    [SerializeField] Mine mine;
    [SerializeField] AudioClip[] MineSounds;
    private void Start() {
        reanimator.AddListener("MiningEndFrame", PickaxeHit);
    }

    private void Update() {
        UpdateDirection(Joystick.GetDirection());
        UpdateState();

        if(ItemInfo.instance.itemsSold >= 10)  //add some sort of effect
            reanimator.fps = 12;
        else
            reanimator.fps = 6;

    }

    private const int MINE = 2;

    internal override void UpdateState() {
        if(mine.StartMining)
            reanimator.Set("AnimationState", MINE);
        else 
            base.UpdateState();
    }

    private void PickaxeHit() {
        mine.FinishMining();
        audioSource.PlayOneShot(MineSounds[Random.Range(0, MineSounds.Length)]);
    }
}
