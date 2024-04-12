using UnityEngine;

public class PointClickMovement : MonoBehaviour, ISaveLoad {
    [SerializeField] float speed = 4f;

    private bool active = true;
    public void SetActive(bool value) {
        this.active = value;
    }

    private Vector3 targetPosition;
    private void Update() {  
        if(active) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if(Input.GetMouseButtonDown(0)) {
                Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(mouse, out RaycastHit hitInfo, 10, layerMask: LayerMask.NameToLayer("Useraycast")))
                    targetPosition = hitInfo.point;   
            }
        }
    }

    public void Save(ref DataRoot data) {
        data.gameData.overworldCoordinates = transform.position;
    }

    public void Load(DataRoot data) {
        transform.position = data.gameData.overworldCoordinates;
        targetPosition = data.gameData.overworldCoordinates;
    }
}