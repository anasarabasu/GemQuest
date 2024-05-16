using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
    public string itemName = "no name";
    public enum ItemType {Mineral, Regen}
    public ItemType itemType;
    public Sprite sprite;
    public Color color;
    [TextArea] public string description = "no description";
    internal int amount = 1;

    private void Start() {
        gameObject.tag = "Loot";

        Vector3 dropDirection = new Vector2(Random.Range(-1, 1), Random.Range(0, 1));
        transform.DOJump(transform.position + dropDirection, 1, 2, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collider) { //onworld
        if(collider.CompareTag("Hels")) {
            InventorySystem.instance.AddItem(new Item {
                itemName = itemName, 
                itemType = itemType,
                sprite = gameObject.GetComponent<SpriteRenderer>().sprite,
                color = gameObject.GetComponent<SpriteRenderer>().color,
                description = description,
                amount = amount
            });

            Destroy(gameObject);
        }
    }
}
