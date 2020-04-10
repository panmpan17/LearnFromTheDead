using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private new Camera camera;

    private void Awake() {
        camera = Camera.main;
    }
}
