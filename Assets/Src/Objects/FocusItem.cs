using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FocusItem : MonoBehaviour {
    public ItemData itemData;
    public void _ItemSelected() {
        if(SceneManager.GetActiveScene().name == "Combat") 
            CombatSystem.instance.SelectItem(itemData);
        else 
            ItemInfo.instance.SelectItem(itemData);
    }
}
