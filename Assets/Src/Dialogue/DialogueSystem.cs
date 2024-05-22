using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour, ISaveable {
    [SerializeField] GameObject dialogueBoxPrefab;
    public List<GameObject> dialogueBoxes;
    private TextMeshProUGUI dialogueContent;
    private (string character, string message)[] dialogueList;
    private int currentDialogueContext = 1;

    private void Awake() {
        dialogueList = DialogueLibrary.GetDialogue(currentDialogueContext);

        foreach ((string character, string message) in dialogueList) {
            GameObject dialogueBox = Instantiate(dialogueBoxPrefab, transform);

            dialogueContent = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
            dialogueContent.SetText(character +"\n"+ message);

            dialogueBoxes.Add(dialogueBox);
        }
    }

    private void Start() {
        if(!isMoving)
            StartCoroutine(MovePanel());
    }

    int panelFocus = 0;
    bool isMoving = false;
    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            if(panelFocus < dialogueBoxes.Count) {
                if(!isMoving)
                    StartCoroutine(MovePanel());
            }
            else {
                SceneHandler.LoadScene("Level1");
                DataManager.instance.WriteSaveFile();
            }
        }
    }

    private IEnumerator MovePanel() {
        yield return new WaitForEndOfFrame();
        RectTransform top = gameObject.GetComponent<RectTransform>();
        RectTransform nextPanel = dialogueBoxes[panelFocus].GetComponent<RectTransform>();
        float nextPosition = top.anchoredPosition.y + nextPanel.rect.height + 10;
        Debug.Log(nextPosition);

        yield return null;
        isMoving = true;
        transform.DOLocalMoveY(nextPosition, 1);
        panelFocus++;

        yield return new WaitUntil(() => (int)gameObject.GetComponent<RectTransform>().anchoredPosition.y == (int)nextPosition);
        isMoving = false;
    }

    public void Load(DataRoot data) {
    }

    public void Save(DataRoot data) {
        data.gameData.newGame = false;
    }
}
