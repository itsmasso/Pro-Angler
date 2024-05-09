using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishResults : MonoBehaviour
{
    [SerializeField] private TMP_Text fishName;
    [SerializeField] private TMP_Text weightText;
    [SerializeField] private TMP_Text worthText;
    [SerializeField] private Image fishIcon;

    public FishInfo fishInfo;
    private void OnEnable()
    {
        if(fishInfo != null)
        {
            fishName.text = string.Format("{0}", fishInfo.fishName);
            weightText.text = string.Format("Weight: {0} lbs", fishInfo.weight);
            worthText.text = string.Format("Worth: ${0}", fishInfo.worth);
            fishIcon.sprite = fishInfo.icon;
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
