using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitSelectionScreen : MonoBehaviour
{
    [SerializeField] private GameObject worms;
    [SerializeField] private GameObject grubs;
    [SerializeField] private GameObject advancedLure;
    [SerializeField] private GameObject masterLure;
    [SerializeField] private GameObject omniBait;

    public static event Action<BaitType> onChangeBait;

    private bool grubsLocked;
    private bool advancedLureLocked;
    private bool masterLureLocked;
    private bool omniBaitLocked;

    void Start()
    {

        UnlockBait.onUnlockBait += UnlockNewBait;
        //implement into savesystem
        grubsLocked = true;
        advancedLureLocked = true;
        masterLureLocked = true;
        omniBaitLocked = true;
        if (grubsLocked)
        {
            grubs.transform.GetComponent<Button>().interactable = false;
            grubs.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (advancedLureLocked)
        {
            advancedLure.transform.GetComponent<Button>().interactable = false;
            advancedLure.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (masterLureLocked)
        {
            masterLure.transform.GetComponent<Button>().interactable = false;
            masterLure.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (omniBaitLocked)
        {
            omniBait.transform.GetComponent<Button>().interactable = false;
            omniBait.transform.GetChild(0).gameObject.SetActive(false);
        }

    }


    private void OnEnable()
    {
        
        if (grubsLocked)
        {
            grubs.transform.GetComponent<Button>().interactable = false;
            grubs.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (advancedLureLocked)
        {
            advancedLure.transform.GetComponent<Button>().interactable = false;
            advancedLure.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (masterLureLocked)
        {
            masterLure.transform.GetComponent<Button>().interactable = false;
            masterLure.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (omniBaitLocked)
        {
            omniBait.transform.GetComponent<Button>().interactable = false;
            omniBait.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void UnlockNewBait(int id)
    {
        switch (id)
        {
            case 1:
                //grubs
                grubsLocked = false;
                grubs.transform.GetComponent<Button>().interactable = true;
                grubs.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 2:
                //advanced
                advancedLureLocked = false;
                advancedLure.transform.GetComponent<Button>().interactable = true;
                advancedLure.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 3:
                //master
                masterLureLocked = false;
                masterLure.transform.GetComponent<Button>().interactable = true;
                masterLure.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 4:
                //omni
                omniBaitLocked = false;
                omniBait.transform.GetComponent<Button>().interactable = true;
                omniBait.transform.GetChild(0).gameObject.SetActive(true);
                break;
            default:
                break;

        }
        
    }

    public void ChooseBait(BaitType baitChosen)
    {
        onChangeBait?.Invoke(baitChosen);
        gameObject.SetActive(false);
    }
    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    public void SwitchToWorms()
    {
        ChooseBait(BaitType.Worms);

    }

    public void SwitchToGrubs()
    {
        if (!grubsLocked)
        {
            ChooseBait(BaitType.Grubs);
        }
    }

    public void SwitchToAdvanced()
    {
        if (!advancedLureLocked)
        {
            ChooseBait(BaitType.AdvancedLure);
        }
    }

    public void SwitchToMaster()
    {
        if (!masterLureLocked)
        {
            ChooseBait(BaitType.MasterLure);
        }
    }

    public void SwitchToOmni()
    {
        if (!omniBaitLocked)
        {
            ChooseBait(BaitType.OmniBait);
        }
    }

    private void OnDestroy()
    {
        UnlockBait.onUnlockBait -= UnlockNewBait;
    }

}
