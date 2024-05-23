using System.Collections.Generic;
using UnityEngine;

public class InstantiateEnemies : MonoBehaviour {

    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] int amountToSpawn;

    float radius = 2;
    private void Awake() {
        

        Instantiate(Random.Range(1, 5));
    }

    private int EnemyRNG() {
        int randomPick = Random.Range(0, enemyPrefabs.Count);
        return randomPick;
    }

    private void Instantiate(int enemyAmount) {
        Vector3[] spawn = {
            Random.insideUnitSphere * radius + transform.position + new Vector3(0, 4),
            Random.insideUnitSphere * radius + transform.position + new Vector3(0, -4),
            Random.insideUnitSphere * radius + transform.position + new Vector3(4, 0),
            Random.insideUnitSphere * radius + transform.position + new Vector3(-4, 0)
        };

        for(int i = 0; i < enemyAmount; i++) {
            Instantiate(enemyPrefabs[EnemyRNG()], spawn[i], Quaternion.identity, transform);
        }
    }
}
