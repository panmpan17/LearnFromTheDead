using UnityEngine;
using MPJamPack;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CharacterMovingControl))]
public class MainCharacter : MonoBehaviour {
    static public MainCharacter ins;

    private Bed bed;
    private BodyBox box;
    private TestBox testBox;
    private ResearchDesk researchDesk;
    private VaccineDesk vaccineDesk;

    CharacterMovingControl movingControl;
    AbstractCharacterInput input;

    [SerializeField]
    private LayerMask interactiveLayer;

    [SerializeField]
    private Rect sideRect, headRect;

    private GameObject clueIndicator, vaccineIndicator;
    private VaccineTube.TubeType[] vaccine;

    private Vector2 sideRectCenter {
        get {
            Vector2 pos = transform.position;
            pos.y += sideRect.center.y;
            if (transform.localScale.x > 0)
                pos.x += sideRect.center.x;
            else
                pos.x -= sideRect.center.x;
            return pos;
        }
    }

    [SerializeField]
    private Timer collectTimer, analizeTimer;


    private CharacterAnimator animator;

    private void Awake() {
        ins = this;

        movingControl = GetComponent<CharacterMovingControl>();
        input = GetComponent<AbstractCharacterInput>();
        animator = GetComponent<CharacterAnimator>();

        clueIndicator = transform.GetChild(0).gameObject;
        clueIndicator.SetActive(false);

        vaccineIndicator = transform.GetChild(1).gameObject;
        vaccineIndicator.SetActive(false);

        collectTimer.Running = false;
        analizeTimer.Running = false;
    }

    private void Start() {
        movingControl.SetInput(input);
    }

    private void Update() {
        if (GameManager.ins.onGUI) return;

        if (collectTimer.Running) {
            if (collectTimer.UpdateEnd) {
                movingControl.enabled = true;
                animator.SetAnimation(CharacterAnimator.State.Idle);

                collectTimer.Running = false;

                bed.ClueCollected = true;
                bed.Box.Close();
                bed = null;
                box = null;

                clueIndicator.SetActive(true);
            }
        }
        else if (analizeTimer.Running) {
            if (analizeTimer.UpdateEnd) {
                // movingControl.enabled = true;
                animator.SetAnimation(CharacterAnimator.State.Idle);

                analizeTimer.Running = false;

                ResearchTableUI.ins.FeedNewClue();
                ResearchTableUI.ins.Activate();
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (bed != null) {
                    if (!bed.ClueCollected) {
                        collectTimer.Reset();
                        movingControl.enabled = false;
                        movingControl.Stop();
                        animator.SetAnimation(CharacterAnimator.State.Collect);

                        VirtualAudioManager.ins.PlayOneShot(AudioIDEnum.Exam, 0.2f);
                    }
                }

                if (box != null) {
                    if (box.CanOpen) {
                        box.Open();
                    }
                }

                if (researchDesk != null) {
                    if (clueIndicator.activeSelf) {
                        analizeTimer.Reset();

                        movingControl.enabled = false;
                        movingControl.Stop();
                        animator.SetAnimation(CharacterAnimator.State.Analize);
                        clueIndicator.SetActive(false);

                        VirtualAudioManager.ins.PlayOneShot(AudioIDEnum.Typing, 0.2f);
                    }
                    else {
                        movingControl.enabled = false;
                        movingControl.Stop();
                        animator.SetAnimation(CharacterAnimator.State.Idle);
                        ResearchTableUI.ins.Activate();
                    }
                }

                if (vaccineDesk != null) {
                    movingControl.enabled = false;
                    movingControl.Stop();
                    animator.SetAnimation(CharacterAnimator.State.Idle);
                    VaccineTableUI.ins.Activate();
                }

                if (testBox != null) {
                    if (vaccineIndicator.activeSelf) {
                        GameManager.ins.SubmitVaccine(vaccine);
                        vaccineIndicator.SetActive(false);
                    }
                }
            }
        }
    }

    private void FixedUpdate() {
        if (vaccineIndicator.activeSelf && clueIndicator.activeSelf)
            vaccineIndicator.SetActive(false);

        RaycastHit2D sideHit = Physics2D.BoxCast(sideRectCenter, sideRect.size, 0, Vector2.up, 0, interactiveLayer);
        if (sideHit.collider != null) {
            Bed _bed = sideHit.collider.GetComponent<Bed>();

            if (!clueIndicator.activeSelf && !vaccineIndicator.activeSelf && _bed != null && _bed != bed) {
                if (bed != null)
                    bed.PlayerOut();
                
                if (!_bed.ClueCollected) {
                    _bed.PlayerIn();
                    bed = _bed;
                }
            }

            TestBox _tBox = sideHit.collider.GetComponent<TestBox>();
            if (!clueIndicator.activeSelf && vaccineIndicator.activeSelf && testBox == null && _tBox != null) {
                _tBox.PlayerIn();
                testBox = _tBox;
            }

            return;
        }

        if (bed != null) {
            bed.PlayerOut();
            bed = null;
        }

        if (testBox != null) {
            testBox.PlayerOut();
            testBox = null;
        }

        RaycastHit2D headHit = Physics2D.BoxCast((Vector2)transform.position + headRect.center, headRect.size, 0, Vector2.up, 0, interactiveLayer);
        if (headHit.collider != null)
        {
            BodyBox _box = headHit.collider.GetComponent<BodyBox>();
            if (_box != null && _box != box) {
                if (box != null)
                    box.PlayerOut();

                if (!clueIndicator.activeSelf && !vaccineIndicator.activeSelf &&  _box.CanOpen) {
                    _box.PlayerIn();
                    box = _box;
                }
            }

            ResearchDesk _rDesk = headHit.collider.GetComponent<ResearchDesk>();
            if (!vaccineIndicator.activeSelf && researchDesk == null && _rDesk != null) {
                _rDesk.PlayerIn();
                researchDesk = _rDesk;
            }

            VaccineDesk _vDesk = headHit.collider.GetComponent<VaccineDesk>();
            if (!clueIndicator.activeSelf && !vaccineIndicator.activeSelf && vaccineDesk == null && _vDesk != null) {
                _vDesk.PlayerIn();
                vaccineDesk = _vDesk;
            }

            return;
        }

        if (box != null) {
            box.PlayerOut();
            box = null;
        }

        if (researchDesk != null) {
            researchDesk.PlayerOut();
            researchDesk = null;
        }

        if (vaccineDesk != null) {
            vaccineDesk.PlayerOut();
            vaccineDesk = null;
        }
    }

    public void PlayAnalyzeAnimation() {
        animator.SetAnimation(CharacterAnimator.State.Analize);
        movingControl.enabled = false;
        movingControl.Stop();
        VirtualAudioManager.ins.PlayOneShot(AudioIDEnum.Boil, 0.2f);
    }

    public void AnalizeFinished(VaccineTube.TubeType[] _vaccine) {
        vaccineIndicator.SetActive(true);
        movingControl.enabled = true;

        vaccine = _vaccine;
    }

    public void GUIOff() {
        movingControl.enabled = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        // sideRect
        Rect _r = sideRect;
        _r.center += (Vector2)transform.position;
        Handles.DrawSolidRectangleWithOutline(_r, new Color(0, 1, 0, 0.4f), new Color(0, 1, 0, 0.5f));

        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawCube(transform.position + (Vector3)headRect.center, headRect.size);
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(transform.position + (Vector3)headRect.center, headRect.size);
    }
#endif
}