using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MPJamPack;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] menuItems;

    [SerializeField]
    private GameObject[] tutorialItems;

    [SerializeField]
    private LanguageData languageData;

    Animator animator;

    private void Awake() {
        LanguageMgr.AssignLanguageData(languageData);

        animator = GetComponent<Animator>();
    }

    // public void StartTutorial() {
    //     for (int i = 0; i < menuItems.Length; i++) menuItems[i].SetActive(false);
    //     for (int i = 0; i < tutorialItems.Length; i++) tutorialItems[i].SetActive(true);
    // }

    public void Next() {
        animator.SetTrigger("Next");
    }

    public void StartGame() {
        SceneManager.LoadScene("Game");
    }
}
