using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPJamPack;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField]
    private Sprite idle;
    [SerializeField]
    private Sprite[] walkSprites, collectSprite, analizeSprite;

    [SerializeField]
    private Timer interval;
    private int index;

    public enum State {
        Idle,
        Walk,
        Collect,
        Analize,
    }

    private State state;

    private new SpriteRenderer renderer;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = idle;
    }

    private void Update() {
        switch(state) {
            case State.Walk:
                if (interval.UpdateEnd)
                {
                    VirtualAudioManager.ins.PlayOneShot(AudioIDEnum.Step, 0.2f);
                    interval.Reset();

                    renderer.sprite = walkSprites[index];
                    index++;
                    if (index >= walkSprites.Length)
                        index = 0;
                }
                break;
            case State.Collect:
                if (interval.UpdateEnd)
                {
                    interval.Reset();

                    renderer.sprite = collectSprite[index];
                    index++;
                    if (index >= collectSprite.Length)
                        index = 0;
                }
                break;
            case State.Analize:
                if (interval.UpdateEnd)
                {
                    interval.Reset();

                    renderer.sprite = analizeSprite[index];
                    index++;
                    if (index >= analizeSprite.Length)
                        index = 0;
                }
                break;
        }
    }

    public void SetAnimation(State _state) {
        if (state == _state) return;

        state = _state;
        index = 1;

        interval.Reset();

        switch (state)
        {
            case State.Idle:
                renderer.sprite = idle;
                break;
            case State.Walk:
                renderer.sprite = walkSprites[0];
                break;
            case State.Collect:
                renderer.sprite = collectSprite[0];
                break;
            case State.Analize:
                renderer.sprite = analizeSprite[0];
                break;
        }
    }
}
