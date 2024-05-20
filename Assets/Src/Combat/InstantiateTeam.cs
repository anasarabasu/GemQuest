using System.Collections.Generic;
using UnityEngine;

public class InstantiateTeam : MonoBehaviour, ISaveable {
    public List<string> teamComposition = new();
    
    [SerializeField] List<GameObject> teamPrefabs;
    
    private List<GameObject> team = new();
    private List<GameObject> shadows = new();

    private enum Members {PIK, HELS, ISKA, POM}

    float radius = 2;
    private void Awake() {     
        Vector3[] spawn = {
            Random.insideUnitSphere * radius + transform.position + new Vector3(0, 4),
            Random.insideUnitSphere * radius + transform.position + new Vector3(0, -4),
            Random.insideUnitSphere * radius + transform.position + new Vector3(4, 0),
            Random.insideUnitSphere * radius + transform.position + new Vector3(-4, 0)
        };

        for(int i = 0; i < teamComposition.Count; i++) 
            team.Add(Instantiate(teamPrefabs[i], spawn[i], Quaternion.identity, transform));
        
    }

    public void Load(DataRoot data) {
        teamComposition = data.gameData.teamComposition;
    }

    public void Save(DataRoot data) {
    }
}
