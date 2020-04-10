using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPJamPack;

public class TestBox : MonoBehaviour
{
    [SerializeField]
    private Color hoveredColor;

    private new SpriteRenderer renderer;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void PlayerIn() {
        renderer.color = hoveredColor;
    }

    public void PlayerOut() {
        renderer.color = Color.white;
    }
}
