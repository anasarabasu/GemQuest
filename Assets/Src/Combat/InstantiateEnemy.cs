using System.Collections.Generic;
using UnityEngine;

public class InstantiateEnemies : MonoBehaviour {

    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] int amountToSpawn;

    float radius = 2;
    private void Awake() {
        Vector3[] spawn = {
            Random.insideUnitSphere * radius + transform.position + new Vector3(0, 4),
            Random.insideUnitSphere * radius + transform.position + new Vector3(0, -4),
            Random.insideUnitSphere * radius + transform.position + new Vector3(4, 0),
            Random.insideUnitSphere * radius + transform.position + new Vector3(-4, 0)
        };

        Instantiate(amountToSpawn);
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
}
