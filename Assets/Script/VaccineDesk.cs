using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPJamPack;

public class VaccineDesk : MonoBehaviour
{
    static public VaccineDesk ins;

    [SerializeField]
    private Color hoveredColor;

    [SerializeField]
    private Timer makeVaccineTimer;

    [SerializeField]
    private ParticleSystem smokeParticle;

    private new SpriteRenderer renderer;

    private void Awake()
    {
        ins = this;

        renderer = GetComponent<SpriteRenderer>();
        makeVaccineTimer.Running = false;
    }

    private void Start() {
        smokeParticle.Stop();
    }

    private void Update()
    {
        if (makeVaccineTimer.Running)
        {
            if (makeVaccineTimer.UpdateEnd)
            {
                makeVaccineTimer.Running = false;
                smokeParticle.Stop();
                MainCharacter.ins.AnalizeFinished(VaccineTableUI.ins.CalculateTubeCombination());
            }
        }
    }

    public void PlayerIn()
    {
        renderer.color = hoveredColor;
    }

    public void PlayerOut()
    {
        renderer.color = Color.white;
    }

    public void StartMakeVaccine() {
        makeVaccineTimer.Reset();
        smokeParticle.Play();
        MainCharacter.ins.PlayAnalyzeAnimation();
    }
}
