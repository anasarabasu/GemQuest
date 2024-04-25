using UnityEngine;

public class PointClick : MonoBehaviour, ISaveLoad {
    [SerializeField] float speed = 4f;

    private void Awake() {
        active = true;
    }

    private bool active;
    public void SetActive(bool value) {
        this.active = value;
    }

    private Vector3 targetPosition;
    private void Update() {  
        if(active) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, speed * Time.deltaTime);

            if(Input.GetMouseButtonDown(0)) {
                Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(mouse, out RaycastHit hitInfo, 10, LayerMask.GetMask("UseRaycast")))
                    targetPosition = hitInfo.point;   
            }
        }
    }

    public void Save(ref DataRoot data) {
        data.gameData.overworldCoordinates = this.transform.position;
    }

    public void Load(DataRoot data) {
        this.transform.position = data.gameData.overworldCoordinates;
        targetPosition = data.gameData.overworldCoordinates;
    }
}