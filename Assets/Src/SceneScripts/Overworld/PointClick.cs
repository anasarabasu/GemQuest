using Unity.Mathematics;
using UnityEngine;

public class PointClick : MonoBehaviour, ISaveable {
    [SerializeField] float speed = 4f;
    private Vector3 targetPosition;
    private void Update() {  
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if(Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, math.INFINITY, LayerMask.GetMask("UseRaycast")))
                targetPosition = hitInfo.point;   
        }
    }

    public void Save(DataRoot data) => data.overworldData.overworldCoordinates = transform.position;

    public void Load(DataRoot data) => (transform.position, targetPosition) = (data.overworldData.overworldCoordinates, data.overworldData.overworldCoordinates);
}