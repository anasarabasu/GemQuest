using TMPro;
using UnityEngine;

public class Penalty : MonoBehaviour, ISaveLoad {
    [SerializeField] TextMeshProUGUI text;
    int penalty;

    private void Start() {
        text.SetText(penalty.ToString());
    }



    public void Save(ref DataRoot data) {
    }
    public void Load(DataRoot data) {
        this.penalty = data.gameData.penaltyWrongCountry;
    }
}
