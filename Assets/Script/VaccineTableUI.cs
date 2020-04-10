using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaccineTableUI : MonoBehaviour
{
    static public VaccineTableUI ins;

    public VaccineTube[] Tubes;

    [SerializeField]
    private RectTransform[] TubePositions;

    private Canvas canvas;
    private GraphicRaycaster raycaster;

    private void Awake() {
        ins = this;

        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start() {
        List<int> indexes = new List<int>(new int[] {
            0,
            1,
            2,
            3,
            4,
            5,
        });

        for (int i = 0; i < Tubes.Length; i++)
        {
            int index = indexes[Random.Range(0, indexes.Count)];
            indexes.Remove(index);

            Tubes[i].Index = index;
            Tubes[i].transform.position = PositionOfTube(index);
        }

        Deactivate();
    }

    public void Activate()
    {
        raycaster.enabled = canvas.enabled = enabled = true;
        GameManager.ins.onGUI = true;
    }

    public void Deactivate()
    {
        raycaster.enabled = canvas.enabled = enabled = false;
        GameManager.ins.onGUI = false;
        MainCharacter.ins.GUIOff();
    }

    public Vector3 PositionOfTube(int index) {
        return TubePositions[index].position;
    }

    public bool FindSwitchTube(VaccineTube tube) {
        for (int i = 0; i < Tubes.Length; i++) {
            if (Tubes[i] == tube) continue;

            if (tube.Rect.Overlaps(Tubes[i].Rect)) {
                int otherIndex = Tubes[i].Index;

                Tubes[i].Index = tube.Index;
                Tubes[i].transform.position = PositionOfTube(tube.Index);

                tube.Index = otherIndex;
                tube.transform.position = PositionOfTube(otherIndex);

                return true;
            }
        }
        return false;
    }

    public VaccineTube.TubeType[] CalculateTubeCombination() {
        List<VaccineTube> tubes = new List<VaccineTube>(Tubes);
        tubes.Sort((tube1, tube2) => {
            return tube1.transform.position.x.CompareTo(tube2.transform.position.x);
        });

        VaccineTube.TubeType[] combination = new VaccineTube.TubeType[tubes.Count];
        for (int i = 0; i < tubes.Count; i++) {
            combination[i] = tubes[i].Type;
        }

        return combination;
    }

    public void SubmitCombination() {
        Deactivate();
        VaccineDesk.ins.StartMakeVaccine();
    }
}
