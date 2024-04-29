using TMPro;
using UnityEngine;

public class Penalty : MonoBehaviour, ISaveLoad {
    [SerializeField] TextMeshProUGUI text;
    int penalty;

    private void Start() {
        text.SetText(penalty.ToString());
    }



    public void Save(DataRoot data) {
    }
    public void Load(DataRoot data) {
        penalty = data.overworldData.penaltyWrongCountry;
    }
}
