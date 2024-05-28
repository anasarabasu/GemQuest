using Pathfinding;
using UnityEngine;

public class HelsAI : AIWander {
    [SerializeField] EntityStatData helsStats;
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

    private void Update() {
        UpdateStuff();
        Timer();
    }


    float timer = 1;
    private void Timer() {
        timer -= Time.deltaTime;
        if(timer <= 0) {
            if(GetComponent<Rigidbody2D>().IsSleeping())
                if(helsStats.currentEnergy < helsStats.energy)
                    helsStats.currentEnergy ++;
                if(helsStats.currentHealth < helsStats.health)
                    helsStats.currentHealth += 4;
            timer = 1;
        }
    }
}
