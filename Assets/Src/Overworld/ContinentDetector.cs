using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinentDetector : MonoBehaviour, ISaveLoad
{
    [SerializeField] private string targetCountry;

    //1 czech - EUROPE
    //2 turkey - ASIA
    //any order: egypt - AFRICA, mexico - N.AMERICA, italy - EUROPE


    private void OnTriggerEnter(Collider continent) {
        Debug.Log($"Travel at {continent}?");
        //pop up text "Travel here?"

        //confirm
            //go to continent-country picker

        if(continent.name == targetCountry) {
            //proceed to proper level
            //target continent = next target continent
            //sceneloader
        }
        else {
            //cutscene: wrong!!! -money
            //sceneloader
        }
    }

    public void OnConfirm (InputAction.CallbackContext context) {
        //switch cam
    }

    public void Load(DataRoot data) {
        this.targetCountry = data.gamePlayData.targetCountry;
    }

    public void Save(ref DataRoot data) {
        throw new System.NotImplementedException();
    }
}
