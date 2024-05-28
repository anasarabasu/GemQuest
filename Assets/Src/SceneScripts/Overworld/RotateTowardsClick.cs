using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsClick : MonoBehaviour
{
    Vector3 mousePosition;
    Vector3 direction;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            direction = (mousePosition - transform.position).normalized;
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.y));

        }
        transform.rotation = Quaternion.LookRotation(direction, transform.forward);
    }

}