using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject eatOrKeep;


    public void ActivateEatOrKeepOption(FishScriptableObject fishScriptable)
    {
        eatOrKeep.GetComponent<EatOrKeepDialogue>().fishScriptable = fishScriptable;
        //Debug.Log(fishScriptable != null);
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
