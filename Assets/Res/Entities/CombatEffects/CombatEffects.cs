using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEffects : MonoBehaviour {
    public static CombatEffects instance;
    private void Awake() => instance = this;

    public GameObject damage;
    public GameObject heal;
    public GameObject electrocute;
}