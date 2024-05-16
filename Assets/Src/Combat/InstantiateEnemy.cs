using System.Collections.Generic;
using UnityEngine;

public class InstantiateEnemies : MonoBehaviour, ISaveable {

    private int chapter;
    public List<string> enemyTypes = new();
    
    [SerializeField] List<GameObject> enemyPrefabs;

    private void Awake() {
        switch (chapter) {
            case 1:
                enemyTypes = new List<string>{"Rock", "Shadow"};
                break;
            default :
                
                break;
        }

        Instantiate(3);
    }

    private int EnemyRNG() {
        int randomPick = Random.Range(0, enemyPrefabs.Count);
        return randomPick;
    }

    private void Instantiate(int enemyAmount = 1) {
        switch (enemyAmount) {
            case 1:
                Instantiate(enemyPrefabs[EnemyRNG()], new Vector3(-7.5f, 0.6f) + transform.position, Quaternion.identity, transform);
                
                break;
            case 2:
                Instantiate(enemyPrefabs[EnemyRNG()], new Vector3(-7.5f, 1.6f) + transform.position, Quaternion.identity, transform);
                Instantiate(enemyPrefabs[EnemyRNG()], new Vector3(-7.5f, -1.6f) + transform.position, Quaternion.identity, transform);
                break;
            case 3:
                Instantiate(enemyPrefabs[EnemyRNG()], new Vector3(-7.5f, 1.6f) + transform.position, Quaternion.identity, transform);
                Instantiate(enemyPrefabs[EnemyRNG()], new Vector3(-7.5f, -1.6f) + transform.position, Quaternion.identity, transform);
                Instantiate(enemyPrefabs[EnemyRNG()], new Vector3(-7.5f, -4.6f) + transform.position, Quaternion.identity, transform);
                
                break;
            case 4:
                
                break;
        }
    }

    public void Save(DataRoot data) {
    }

    public void Load(DataRoot data) {
        chapter = data.gameData.chapter;
    }
}
