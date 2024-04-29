using Pathfinding;
using UnityEngine;

public class HelsAni : AnimateBase {

    [SerializeField]AIPath AIPath;

    private void Update() {
        UpdateDirection(AIPath.desiredVelocity);
        UpdateState();
    }
}
