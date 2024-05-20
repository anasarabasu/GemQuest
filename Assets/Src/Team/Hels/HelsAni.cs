using Pathfinding;
using UnityEngine;

public class HelsAni : AnimateRoam {

    [SerializeField]AIPath AIPath;

    private void Update() {
        UpdateDirection(AIPath.desiredVelocity);
        UpdateState();
    }
}
