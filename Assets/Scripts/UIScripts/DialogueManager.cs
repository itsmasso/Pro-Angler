using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject eatOrKeep;
    [SerializeField] private GameObject fishingResults;

    public void ActivateEatOrKeepOption(FishScriptableObject fishScriptable, FishValue valueStats)
    {
        eatOrKeep.GetComponent<EatOrKeepDialogue>().fishScriptable = fishScriptable;
        eatOrKeep.GetComponent<EatOrKeepDialogue>().fishValue = valueStats;
        //Debug.Log(fishScriptable != null);
        eatOrKeep.SetActive(true);
        //fishingResults
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
