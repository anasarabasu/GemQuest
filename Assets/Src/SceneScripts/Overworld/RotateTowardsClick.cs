using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsClick : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Set z to 0 since we're working in 2D

            // Get the direction from the sprite to the mouse position
            Vector2 direction = (mousePosition - transform.position).normalized;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}