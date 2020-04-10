using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [System.NonSerialized]
    public BodyBox Box;

    public GameObject Body;

    [SerializeField]
    private Color hoveredColor;

    [System.NonSerialized]
    public bool ClueCollected;

    private new SpriteRenderer renderer;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void PlayerIn() {
        if (Box.BedCanClick)
            renderer.color = hoveredColor;
    }

    public void PlayerOut() {
        renderer.color = Color.white;
    }
}
