using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RodSelectionScreen : MonoBehaviour
{
    [SerializeField] private RodScriptableObject oldRodScriptable;
    [SerializeField] private RodScriptableObject sturdyRodScriptable;
    [SerializeField] private RodScriptableObject apprenticeRodScriptable;
    [SerializeField] private RodScriptableObject veteranRodScriptable;
    [SerializeField] private RodScriptableObject masterRodScriptable;
    [SerializeField] private RodScriptableObject icyRodScriptable;
    [SerializeField] private RodScriptableObject superChargedRodScriptable;

    [SerializeField] private GameObject oldRod;
    [SerializeField] private GameObject sturdyRod;
    [SerializeField] private GameObject apprenticeRod;
    [SerializeField] private GameObject veteranRod;
    [SerializeField] private GameObject masterRod;
    [SerializeField] private GameObject icyRod;
    [SerializeField] private GameObject superChargedRod;

    [SerializeField] private GameObject rodSelectScreen;


    public static event Action<RodScriptableObject> onChangeRod;

    private bool sturdyRodLocked;
    private bool apprenticeRodLocked;
    private bool veteranRodLocked;
    private bool masterRodLocked;
    private bool icyRodLocked;
    private bool superChargedRodLocked;
    void Start()
    {
        UnlockFishingRodScript.onUnlockFishingRod += UnlockNewRod;
        //implement into savesystem
        sturdyRodLocked = true;
        apprenticeRodLocked = true;
        veteranRodLocked = true;
        masterRodLocked = true;
        icyRodLocked = true;
        superChargedRodLocked = true;
        LockRods();
    }

    private void LockRods()
    {
        
        //implement into savesystem

        if (sturdyRodLocked)
        {
            sturdyRod.transform.GetComponent<Button>().interactable = false;
            sturdyRod.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (apprenticeRodLocked)
        {
            apprenticeRod.transform.GetComponent<Button>().interactable = false;
            apprenticeRod.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (veteranRodLocked)
        {
            veteranRod.transform.GetComponent<Button>().interactable = false;
            veteranRod.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (masterRodLocked)
        {
            masterRod.transform.GetComponent<Button>().interactable = false;
            masterRod.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (icyRodLocked)
        {
            icyRod.transform.GetComponent<Button>().interactable = false;
            icyRod.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (superChargedRodLocked)
        {
            superChargedRod.transform.GetComponent<Button>().interactable = false;
            superChargedRod.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        LockRods();
    }

    private void UnlockNewRod(int id)
    {
        switch (id)
        {
            case 1:
                //sturdy
                sturdyRodLocked = false;
                sturdyRod.transform.GetComponent<Button>().interactable = true;
                sturdyRod.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 2:
                //apprentice
                apprenticeRodLocked = false;
                apprenticeRod.transform.GetComponent<Button>().interactable = true;
                apprenticeRod.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 3:
                //veteran
                veteranRodLocked = false;
                veteranRod.transform.GetComponent<Button>().interactable = true;
                veteranRod.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 4:
                //master
                masterRodLocked = false;
                masterRod.transform.GetComponent<Button>().interactable = true;
                masterRod.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 5:
                //icy
                icyRodLocked = false;
                icyRod.transform.GetComponent<Button>().interactable = true;
                icyRod.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 6:
                //super charged
                superChargedRodLocked = false;
                superChargedRod.transform.GetComponent<Button>().interactable = true;
                superChargedRod.transform.GetChild(0).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ChooseRod(RodScriptableObject rodChosen)
    {
        onChangeRod?.Invoke(rodChosen);
        rodSelectScreen.SetActive(false);
    }
    public void Cancel()
    {
        rodSelectScreen.SetActive(false);
    }

    public void SwitchToOldRod()
    {
        ChooseRod(oldRodScriptable);


    }

    public void SwitchToSturdy()
    {
        if (!sturdyRodLocked)
        {
            ChooseRod(sturdyRodScriptable);

        }
    }

    public void SwitchToApprentice()
    {
        if (!apprenticeRodLocked)
        {
            ChooseRod(apprenticeRodScriptable);

        }
    }

    public void SwitchToVeteran()
    {
        if (!veteranRodLocked)
        {
            ChooseRod(veteranRodScriptable);

        }
    }

    public void SwitchToMaster()
    {
        if (!masterRodLocked)
        {
            ChooseRod(masterRodScriptable);

        }
    }

    public void SwitchToIcy()
    {
        if (!icyRodLocked)
        {
            ChooseRod(icyRodScriptable);

        }
    }

    public void SwitchToSuperCharged()
    {
        if (!superChargedRodLocked)
        {
            ChooseRod(superChargedRodScriptable);

        }
    }

    private void OnDestroy()
    {
        UnlockFishingRodScript.onUnlockFishingRod -= UnlockNewRod;
    }

}
