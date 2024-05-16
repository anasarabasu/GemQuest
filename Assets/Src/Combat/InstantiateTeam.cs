using System.Collections.Generic;
using UnityEngine;

public class InstantiateTeam : MonoBehaviour, ISaveable {
    public List<string> teamComposition = new();
    [SerializeField] List<GameObject> teamPrefabs;

    private enum Members {PIK, HELS, ISKA, POK}

    private void Awake() {
        Instantiate(teamComposition.Count);
    }

    private void Instantiate(int teamAmount) {
        switch (teamAmount) {
            case 2:
                Instantiate(teamPrefabs[(int)Members.PIK], new Vector3(7, 1.5f) + transform.position, Quaternion.identity, transform);
                Instantiate(teamPrefabs[(int)Members.HELS], new Vector3(2.5f, -4.5f) + transform.position, Quaternion.identity, transform);
                Debug.Log("team instantiated");
                break;
            case 3:
                
                break;
            case 4:
                
                break;
        }
    }

    public void Load(DataRoot data) {
        teamComposition = data.gameData.teamComposition;
    }

    public void Save(DataRoot data) {
    }
}
