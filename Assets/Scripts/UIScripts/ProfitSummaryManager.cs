using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfitSummaryManager : MonoBehaviour
{
    [System.NonSerialized] public int profitMade;
    [System.NonSerialized] public int profitNeededLeft;
    [System.NonSerialized] public int newProfitGoal;
    [System.NonSerialized] public int daysRemaining;
    [System.NonSerialized] public int newDeadline;

    [Header("Daily Summary")]
    [SerializeField] private GameObject dailySummary; //shows profit made and how much profit needed left
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text profitMadeText;
    [SerializeField] private TMP_Text proftNeededLeftText;

    [Header("Profit Not Complete")]
    [SerializeField] private GameObject goalIncomplete;
    [SerializeField] private TMP_Text daysRemainingText;
    public bool goalNotCompleted;

    [Header("Profit Completed")]
    [SerializeField] private GameObject goalComplete;
    [SerializeField] private TMP_Text newProfitGoalText;
    public bool goalCompleted;

    [Header("Terminated")]
    [SerializeField] private GameObject termination;
    public bool terminated;

    [SerializeField] private WorldTime worldTimer;

    public void Restart()
    {
        GameManager.Instance.UpdateScene(2);
    }

    public void MainMenu()
    {
        GameManager.Instance.UpdateScene(1);
    }

    private void OnEnable()
    {
        dayText.text = string.Format("Day {0}", worldTimer.day);
       if(!goalCompleted && goalNotCompleted && !terminated)
        {
            //goal not completed, but days remaining
            proftNeededLeftText.text = string.Format("Profit Needed Left: ${0}", profitNeededLeft);
            profitMadeText.text = string.Format("${0}", profitMade);
            daysRemainingText.text = string.Format("{0} days remaining", daysRemaining);

            dailySummary.SetActive(true);
            goalIncomplete.SetActive(true);
            goalComplete.SetActive(false);
            termination.SetActive(false);

        }
        else if(goalCompleted && !goalNotCompleted && !terminated)
        {
            //goal completed, new qouta
            proftNeededLeftText.text = string.Format("Profit Needed Left: ${0}", profitNeededLeft);
            profitMadeText.text = string.Format("${0}", profitMade);
            newProfitGoalText.text = string.Format("New Profit Goal: ${0}\nDeadline: {1} days", newProfitGoal, newDeadline);

            dailySummary.SetActive(true);
            goalIncomplete.SetActive(false);
            goalComplete.SetActive(true);
            termination.SetActive(false);
        }else if(!goalCompleted && !goalNotCompleted && terminated)
        {
            //you lose, you're terminated
            dailySummary.SetActive(false);
            goalIncomplete.SetActive(false);
            goalComplete.SetActive(false);
            termination.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
