using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject eatOrKeep;
    [SerializeField] private FishResults fishingResults;

    public void ActivateEatOrKeepOption(FishInfo _fishInfo)
    {
        eatOrKeep.GetComponent<EatOrKeepDialogue>().fishInfo = _fishInfo;
        fishingResults.fishInfo = _fishInfo;
        eatOrKeep.SetActive(true);
    }
    void Start()
    {
        FishBaseScript.onDestroyFish += ActivateEatOrKeepOption;
        eatOrKeep.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        FishBaseScript.onDestroyFish -= ActivateEatOrKeepOption;
    }
}
