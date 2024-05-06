using Pathfinding;
using UnityEngine;

public class HelsAI : AIWander {
    protected override void UpdateTargets() {
        //pickup items AI
        if(GameObject.FindGameObjectWithTag("Loot") != null) {
                path.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;

                targets[0] = GameObject.FindGameObjectWithTag("Loot").transform.position;
                targets[1] = GameObject.FindGameObjectWithTag("Loot").transform.position;
            }
            else {
                path.whenCloseToDestination = CloseToDestinationMode.Stop;
                
                targets[0] = leader.position;
                targets[1] = wanderAround;
            }       
    }
}
