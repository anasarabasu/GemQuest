using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Interactables : MonoBehaviour {
    public static Interactables instance;

    [SerializeField] Tilemap tilemap;

    public Interactables() {
        instance = this;
    }

    private void Start() {
        // bounds
        
        // Debug.Log(tilemap.GetTile(new Vector3Int(10, -13, 0)));

        // tilemap.SetTile(new Vector3Int(10, -13, 0), null);
    }

    Vector3 hitPos = Vector3.zero;

    private void OnCollisionEnter2D(Collision2D block) { //oncollide set tile to null

        if(block.gameObject.name == "Rocks")
            foreach(ContactPoint2D contact in block.contacts) {
                hitPos.x = contact.point.x - 0.01f * contact.normal.x;
                hitPos.y = contact.point.y - 0.01f * contact.normal.y;

                Debug.Log(tilemap.WorldToCell(hitPos));
                Mine(tilemap.WorldToCell(hitPos));
            }
                
    }

    public void Mine(Vector3Int position) {
        tilemap.SetTile(tilemap.WorldToCell(hitPos), null);
        
    }
    
}
