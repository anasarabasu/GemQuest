using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    // Set the speed at which the chat box moves
    public float moveSpeed = 5f;

    // Set the smoothness of the movement
    public float smoothness = 0.1f;

    // Reference to the second panel
    public GameObject secondPanel;

    // Reference to the TextMeshPro component
    public TextMeshProUGUI textMeshProText;

    // Reference to the RectTransform of the panel
    public RectTransform panelRectTransform;

    // Flag to track if the panel is moving
    private bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
        // Check if the chat box is clicked and not currently moving
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            // Calculate the target position for the panel to move off-screen
            Vector3 targetPosition = transform.position + Vector3.up * 250;

            // Start the movement coroutine
            StartCoroutine(MovePanel(targetPosition));
        }
    }

    // Coroutine for smooth movement
    private IEnumerator MovePanel(Vector3 targetPosition)
    {
        // Set moving flag to true
        isMoving = true;

        // Smoothly move the panel towards the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
            yield return null;

            // Check if the chat box is clicked again while moving
            if (Input.GetMouseButtonDown(0))
            {
                // Set the target position to the current position to stop movement smoothly
                targetPosition = transform.position;
            }
        }

        // Set moving flag to false
        isMoving = false;

        // Activate the second panel when the first panel is clicked
        secondPanel.SetActive(true);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Get the size of the text
        Vector2 textSize = textMeshProText.GetPreferredValues();

        // Adjust the panel size based on the text size
        //panelRectTransform.sizeDelta = new Vector2(textSize.x, textSize.y);
    }
}