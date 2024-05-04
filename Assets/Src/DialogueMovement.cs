using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueMovement : MonoBehaviour
{
    public float moveDistance = 250f; // Distance to move each panel
    public float moveSpeed = 5f;
    public RectTransform[] panels; // Array of panel RectTransforms

    private bool isMoving = false;
    private int currentPanelIndex = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            if (currentPanelIndex < panels.Length)
            {
                // Move all panels up that are at or below current index
                for (int i = 0; i <= currentPanelIndex; i++)
                {
                    MovePanelUp(panels[i]);
                }
                currentPanelIndex++;
            }
        }
    }

    private void MovePanelUp(RectTransform panel)
    {
        Vector3 targetPosition = panel.position + Vector3.up * moveDistance;
        StartCoroutine(MovePanel(panel, targetPosition));
    }

    private IEnumerator MovePanel(RectTransform panel, Vector3 targetPosition)
    {
        isMoving = true;
        float distance = Vector3.Distance(panel.position, targetPosition);
        float duration = distance / moveSpeed;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            panel.position = Vector3.Lerp(panel.position, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.position = targetPosition;
        isMoving = false;
    }
}