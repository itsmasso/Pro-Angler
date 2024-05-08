using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishResults : MonoBehaviour
{
    [SerializeField] private TMP_Text fishName;
    [SerializeField] private TMP_Text weightText;
    [SerializeField] private TMP_Text worthText;

    public FishInfo fishInfo;
    private void OnEnable()
    {
        if(fishInfo != null)
        {
            fishName.text = string.Format("{0}", fishInfo.fishName);
            weightText.text = string.Format("{0} lbs", fishInfo.weight);
            worthText.text = string.Format("Worth: ${0}", fishInfo.worth);
        }
        else
        {
            fishName.text = "???";
            weightText.text = "???";
            worthText.text = "???";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
