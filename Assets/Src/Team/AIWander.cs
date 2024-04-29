using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]

	public class AIWander : VersionedMonoBehaviour {
		[SerializeField] protected Transform leader;
        [SerializeField] float delay = 0.5f;
        
        [SerializeField] protected List<Vector3> targets = new List<Vector3>();

		private IAstarAI agent;
        protected AIPath path;
		float switchTime = float.PositiveInfinity;
        private int index;
		protected Vector3 wanderAround;

        protected override void Awake () {
			base.Awake();
			agent = GetComponent<IAstarAI>();
            path = GetComponent<AIPath>();
            
            targets.Add(leader.position);
            targets.Add(wanderAround);
		}

		void Update () {
            UpdateTargets();
            UpdateSearchPath();
		}

        private void UpdateSearchPath() {
            bool search = false;

            if(agent.reachedEndOfPath && !agent.pathPending && float.IsPositiveInfinity(switchTime))
                switchTime = Time.time + delay;

            if (Time.time >= switchTime) {
                wanderAround = Random.insideUnitSphere * 10 + transform.position;
				index ++;
				search = true;
				switchTime = float.PositiveInfinity;
			}

			index %= targets.Count;
            if(targets[index] != null)
                agent.destination = targets[index];

            if(search) 
                agent.SearchPath();
        }

        protected virtual void UpdateTargets() {
            targets[0] = leader.position;
            targets[1] = wanderAround;
        }
	}
}
