using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchTableUI : MonoBehaviour
{
    static public ResearchTableUI ins;

    private Canvas canvas;
    private GraphicRaycaster raycaster;

    [SerializeField]
    private RectTransform scrollViewContent;

    [SerializeField]
    private TextMeshProUGUI textPrefab;

    [SerializeField]
    private float padding;
    private float y = 0;

    private void Awake() {
        ins = this;

        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();

        textPrefab.gameObject.SetActive(false);
    }

    private void Start() {
        Deactivate();
        // float y = 0;

        // for (int i = 0; i < 10; i++) {
        //     TextMeshProUGUI text = Instantiate(textPrefab, scrollViewContent);
        //     text.text = string.Format("Line. {0}", i);
        //     text.rectTransform.anchoredPosition = new Vector2(0, y);
        //     text.gameObject.SetActive(true);

        //     y -= text.rectTransform.sizeDelta.y + padding;
        // }

        // scrollViewContent.sizeDelta = new Vector2(scrollViewContent.sizeDelta.x, Mathf.Abs(y));
    }

    public void Activate() {
        raycaster.enabled = canvas.enabled = enabled = true;
        GameManager.ins.onGUI = true;
    }

    public void Deactivate() {
        raycaster.enabled = canvas.enabled = enabled = false;
        GameManager.ins.onGUI = false;
        MainCharacter.ins.GUIOff();
    }

    public void FeedNewClue() {
        TextMeshProUGUI text = Instantiate(textPrefab, scrollViewContent);
        text.text = GameManager.NextClue();
        text.rectTransform.anchoredPosition = new Vector2(0, y);
        text.gameObject.SetActive(true);

        y -= text.rectTransform.sizeDelta.y + padding;
    }
}
