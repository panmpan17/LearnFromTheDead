using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MPJamPack;
// using Cinemacine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    static public GameManager ins;

    static private int[] clues = new int[] {
        51,
        52,
        53,
        54,
        55,
        56,
        57,
        58,
        59,
        60,
        61,
    };

    static private int clueIndex = 0;

    static public string NextClue() {
        string clue = LanguageMgr.GetTextById(clues[clueIndex]);
        clueIndex++;
        return clue;
    }

    // [SerializeField]
    // private TextMeshPro clockText;
    // private int clockTime;
    // private Timer clockAddTimer = new Timer(1f);

    [SerializeField]
    private CinemachineVirtualCamera playerVCam, bedVCam;


    [System.NonSerialized]
    public bool onGUI;

    [SerializeField]
    private LanguageData languageData;

    [SerializeField]
    private TextMeshPro countText;
    [SerializeField]
    private float intervalMin, intervalMax;
    private Timer interval;
    private int count = 0;

    [SerializeField]
    private Timer testingTimer;

    [SerializeField]
    private BodyBox[] bodyBoxes;

    [SerializeField]
    private Sprite[] bodies;

    [SerializeField]
    private GameObject doorOpened, doorClosed;
    [SerializeField]
    private GameObject horizontalBed;
    [SerializeField]
    private SpriteRenderer horizontalBedBody;

    [SerializeField]
    private GameObject finalPanel;
    [SerializeField]
    private TextMeshProUGUI finalDeathCount;

    private Timer bedApearTimer = new Timer(1), bedDisapearTimer = new Timer(1);
    private Vector3LerpTimer bedMoveTimer;
    private BodyBox bodyBox;

    [SerializeField]
    private bool test;

    public bool DoorOpen {
        set {
            doorOpened.SetActive(value);
            doorClosed.SetActive(!value);
        }
    }

    public bool BedVCam {
        set {
            bedVCam.enabled = value;
            playerVCam.enabled = !value;
        }
    }

    private VaccineTube.TubeType[] rightCombination = new VaccineTube.TubeType[] {
        VaccineTube.TubeType.Blue,
        VaccineTube.TubeType.Red,
        VaccineTube.TubeType.Purple,
        VaccineTube.TubeType.Orange,
        VaccineTube.TubeType.Green,
        VaccineTube.TubeType.Yellow,
    };

    private void Awake() {
        ins = this;
        clueIndex = 0;

        LanguageMgr.AssignLanguageData(languageData);

        interval = new Timer(Random.Range(intervalMin, intervalMax));

        testingTimer.Running = false;

        DoorOpen = false;
        horizontalBed.SetActive(false);

        bedMoveTimer = new Vector3LerpTimer();
        bedMoveTimer.Timer.Running = false;

        BedVCam = false;

        bedApearTimer.Running = false;
        bedDisapearTimer.Running = false;
    }

    private void Update() {
        if (bedApearTimer.Running) {
            if (bedApearTimer.UpdateEnd) {
                bedApearTimer.Running = false;
                bedMoveTimer.Timer.Running = true;

                VirtualAudioManager.ins.PlayOneShot(AudioIDEnum.Slidding, 0.1f);
            }
        }
        if (bedMoveTimer.Timer.Running) {
            if (bedMoveTimer.Timer.UpdateEnd) {
                bedMoveTimer.Timer.Running = false;
                horizontalBed.transform.position = bedMoveTimer.Value;
                bedDisapearTimer.Reset();
            }
            else {
                horizontalBed.transform.position = bedMoveTimer.Value;
            }
        }
        if (bedDisapearTimer.Running) {
            if (bedDisapearTimer.UpdateEnd) {
                bedDisapearTimer.Running = false;

                horizontalBed.SetActive(false);
                DoorOpen = false;
                VirtualAudioManager.ins.PlayOneShot(AudioIDEnum.DoorOpen, 0.2f);

                BedVCam = false;

                bodyBox.LoadBody(horizontalBedBody.sprite);
            }
        }

        if (testingTimer.Running) {
            if (testingTimer.UpdateEnd) {
                testingTimer.Running = false;

                for (int i = 0; i < bodyBoxes.Length; i++) {
                    if (bodyBoxes[i].CanPlayBody) {
                        bodyBox = bodyBoxes[i];
                        DoorOpen = true;
                        VirtualAudioManager.ins.PlayOneShot(AudioIDEnum.DoorOpen, 0.2f);


                        Vector3 pos = horizontalBed.transform.position;
                        pos.x = doorOpened.transform.position.x;
                        horizontalBed.transform.position = pos;
                        horizontalBedBody.sprite = bodies[Random.Range(0, bodies.Length)];
                        horizontalBed.SetActive(true);

                        pos.x = bodyBox.transform.position.x;
                        float dis = pos.x - horizontalBed.transform.position.x;
                        bedApearTimer.Reset();

                        bedMoveTimer = new Vector3LerpTimer(horizontalBed.transform.position, pos, 3);
                        bedMoveTimer.Timer.Running = false;

                        BedVCam = true;
                        break;
                    }
                }
            }
            else {
                if (interval.UpdateEnd)
                {
                    interval.Reset(Random.Range(0.7f, 0.8f), true);
                    interval.TargetTime = Random.Range(intervalMin, intervalMax);
                    count += Random.Range(10, 20);
                    countText.text = count.ToString();
                }
            }
        }

        if (test) {
            SubmitVaccine(VaccineTableUI.ins.CalculateTubeCombination());
            test = false;
        }
    }

    private void FixedUpdate() {
        if (interval.FixedUpdateEnd) {
            interval.Reset();
            interval.TargetTime = Random.Range(intervalMin, intervalMax);

            if (testingTimer.Running)
                count += Random.Range(10, 20);
            else
                count += Random.Range(1, 3);
            countText.text = count.ToString();
        }
    }

    public void SubmitVaccine(VaccineTube.TubeType[] _vaccine)
    {
        if (TestCombination(_vaccine)) {
            finalPanel.SetActive(true);
            finalDeathCount.text = count.ToString();
            Time.timeScale = 0;
        }
        else {
            testingTimer.Reset();
        }
    }

    public bool TestCombination(VaccineTube.TubeType[] combination) {
        for (int i = 0; i < combination.Length; i++) {
            if (rightCombination[i] != combination[i]) {
                return false;
            }
        }

        return true;
    }
}
