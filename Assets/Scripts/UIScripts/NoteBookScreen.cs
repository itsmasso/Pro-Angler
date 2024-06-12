using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBookScreen : MonoBehaviour
{
    [SerializeField] private GameObject notebookParent;

    [SerializeField] private GameObject fishPage;
    [SerializeField] private GameObject fishTabInactive;


    [SerializeField] private GameObject rodTab;
    [SerializeField] private GameObject rodTabInactive;


    [SerializeField] private GameObject baitTab;
    [SerializeField] private GameObject baitTabInactive;


    void Start()
    {
        notebookParent.SetActive(false);
    }

    public void OpenFishPage()
    {
        fishPage.SetActive(true);
        rodTab.SetActive(false);
        baitTab.SetActive(false);

        fishTabInactive.SetActive(false);
        rodTabInactive.SetActive(true);
        baitTabInactive.SetActive(true);
    }

    public void OpenRodPage()
    {
        fishPage.SetActive(false);
        rodTab.SetActive(true);
        baitTab.SetActive(false);

        fishTabInactive.SetActive(true);
        rodTabInactive.SetActive(false);
        baitTabInactive.SetActive(true);

    }

    public void OpenBaitPage()
    {
        fishPage.SetActive(false);
        rodTab.SetActive(false);
        baitTab.SetActive(true);

        fishTabInactive.SetActive(true);
        rodTabInactive.SetActive(true);
        baitTabInactive.SetActive(false);
    }

    public void OpenNoteBook()
    {
        notebookParent.SetActive(!notebookParent.activeSelf);
        if (!notebookParent.activeSelf)
        {
            OpenFishPage();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSource, "OpeningBookSFX", false);
        }
        else
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSource, "ClosingBookSFX", false);
        }
    }
    void Update()
    {
        
    }
}
