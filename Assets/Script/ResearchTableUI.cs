using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchTableUI : MonoBehaviour
{
    static public ResearchTableUI ins;

    [SerializeField]
    private TextMeshProUGUI clueText;

    private Canvas canvas;
    private GraphicRaycaster raycaster;

    private bool _firstClueGot = false;

    private void Awake() {
        ins = this;

        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();

        clueText.text = "No clue yet.";
    }

    private void Start() {
        Deactivate();
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
        if (_firstClueGot)
        {
            clueText.text += "\n" + GameManager.NextClue();
        }
        else
        {
            clueText.text = GameManager.NextClue();
            _firstClueGot = true;
        }
    }
}
