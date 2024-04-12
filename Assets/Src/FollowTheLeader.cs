using UnityEngine;

public class FollowTheLeader : MonoBehaviour {

    GameObject leader;
    Vector2 leaderPos;
    private float maxDistance = 3;
    private float apeed = 34;

    private void Awake() {
        leader = GameObject.FindGameObjectWithTag("Player"); //or leader
    }
    void Start() {
        
    }


    // Update is called once per frame
    void Update() {
        if(Vector2.Distance(this.transform.position, leader.transform.position) > maxDistance)
            this.transform.position = Vector2.MoveTowards(this.transform.position, leader.transform.position, apeed * Time.deltaTime);
    }
}
