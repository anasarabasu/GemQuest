using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public TextMeshProUGUI textMeshProText;

    private bool isMoving = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Vector3 targetPosition = transform.position + Vector3.up * 250;
            StartCoroutine(MovePanel(targetPosition));
        }
    }

    private IEnumerator MovePanel(Vector3 targetPosition)
    {
        isMoving = true;
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / moveSpeed;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }

  /*  void LateUpdate()
    {
        Vector2 textSize = textMeshProText.GetPreferredValues();
    }*/
}