using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NoticePanel : MonoBehaviour {
    public static NoticePanel instance;
    private void Awake() => instance = this;
    [SerializeField] RectTransform noticePanel;
    
    public IEnumerator ShowNotice(string message) { //me too
        noticePanel.DOAnchorPosY(12.75f, 0.25f);
        noticePanel.GetComponentInChildren<TextMeshProUGUI>().SetText(message);
        yield return new WaitForSeconds(1);
        
        noticePanel.DOAnchorPosY(-12.2f, 0.5f);
    }
}
