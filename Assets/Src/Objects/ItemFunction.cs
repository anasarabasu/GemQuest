using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class ItemFunction : MonoBehaviour {
    public string itemName;
    [TextArea] public string itemDescription;

    private void Update() {

        // if(Input.GetMouseButtonDown(0)) {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //         if(Physics.Raycast(ray, out RaycastHit hitInfo, math.INFINITY, LayerMask.GetMask("UseRaycast")))
        // }
    }

    public void _Selected() {
        InventorySystem.instance.ShowInfo(itemName, itemDescription);
    }

    //select item
    //show description
    //enebale use buttons or something
}
