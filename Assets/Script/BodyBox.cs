using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPJamPack;

public class BodyBox : MonoBehaviour
{
    [SerializeField]
    private Sprite openedSprite;
    private Sprite closedSprite;

    [SerializeField]
    private Vector3LerpTimer openTimer;

    [SerializeField]
    private Bed bed;

    private bool opened = false;
    private bool loaded = false;

    [SerializeField]
    private Color hoveredColor, disableColor;
    private new SpriteRenderer renderer;

    public bool CanOpen {
        get {
            return !opened && loaded && !openTimer.Timer.Running && !bed.ClueCollected;
        }
    }

    public bool BedCanClick {
        get {
            return opened && loaded && !openTimer.Timer.Running;
        }
    }

    public bool CanPlayBody {
        get {
            return !loaded;
        }
    }

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();

        closedSprite = renderer.sprite;
        bed.gameObject.SetActive(false);
        bed.Box = this;
    }

    private void Start() {
        Vector3 pos = transform.position;
        pos.y = 3.5f;
        transform.position = pos;

        openTimer.Timer.Running = false;

        loaded = bed.Body.activeSelf;
        if (!loaded) {
            renderer.color = disableColor;
            renderer.sprite = openedSprite;
        }
    }

    private void Update() {
        if (openTimer.Timer.Running) {
            if (openTimer.Timer.UpdateEnd) {
                openTimer.Timer.Running = false;
                bed.transform.localPosition = openTimer.Value;

                if (!opened) {
                    renderer.sprite = closedSprite;
                    bed.gameObject.SetActive(false);

                    VirtualAudioManager.ins.PlayOneShot(AudioIDEnum.BoxDoorOpen);
                }
            }
            else bed.transform.localPosition = openTimer.Value;
        }
    }

    public void PlayerIn() {
        if (!loaded) return;
        if (GameManager.ins.onGUI) return;

        renderer.color = hoveredColor;
    }

    public void PlayerOut() {
        if (!loaded) return;
        renderer.color = Color.white;
    }

    public void Open() {
        if (!loaded) return;
        if (openTimer.Timer.Running) return;

        opened = true;
        renderer.sprite = openedSprite;
        openTimer.Timer.Reset();
        openTimer.Timer.ReverseMode = false;
        bed.gameObject.SetActive(true);

        renderer.color = Color.white;

        VirtualAudioManager.ins.PlayOneShot(AudioIDEnum.BoxDoorOpen);
    }

    public void Close() {
        if (!loaded) return;
        if (openTimer.Timer.Running) return;

        opened = false;
        openTimer.Timer.Running = true;
        openTimer.Timer.ReverseMode = true;

        renderer.color = disableColor;
    }

    public void LoadBody(Sprite body) {
        bed.Body.GetComponent<SpriteRenderer>().sprite = body;
        bed.Body.SetActive(true);

        renderer.color = Color.white;
        renderer.sprite = closedSprite;

        loaded = true;
        opened = false;

        VirtualAudioManager.ins.PlayOneShot(AudioIDEnum.BoxDoorOpen);
    }
}
