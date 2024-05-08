using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MoneyHandler))]
public class QoutaSystem : MonoBehaviour
{
    public static event Action onContinueToNextDay;

    [Header("Qouta System")]
    [SerializeField] private int currentQouta = 130;
    private MoneyHandler moneyHandler;
    private int totalProfit;
    private int profitMadeToday;
    private int nextQoutaIncrease;
    public int timesFulfilled { private set; get; } //save system
    private int remainingDayCounter = 3;

    [Header("Text")]
    [SerializeField] private TMP_Text deadlineText;
    [SerializeField] private TMP_Text profitQoutaText;
    [SerializeField] private GameObject profitSummary;

    private void Awake()
    {
        WorldTime.onResetDay += DailyReset;
        moneyHandler = GetComponent<MoneyHandler>();
        moneyHandler.onUpdateProfit += UpdateProfitCount;

    }
    void Start()
    {
        profitSummary.SetActive(false);
    }

    private void IncreaseQouta()
    {
        //Adjust if needed
        nextQoutaIncrease = Mathf.CeilToInt(100 * (1 + timesFulfilled ^ 2 / 16) * UnityEngine.Random.value + 1);
        currentQouta += nextQoutaIncrease;
    }

    private void UpdateProfitCount(object sender, int sellAmount)
    {
        totalProfit += sellAmount;
        profitMadeToday += sellAmount;
    }

    public void Continue()
    {
        onContinueToNextDay?.Invoke();
        profitMadeToday = 0;
        profitSummary.SetActive(false);
    }

    private void DailyReset()
    {
        ProfitSummaryManager profitSumScript = profitSummary.GetComponent<ProfitSummaryManager>();
        if (totalProfit >= currentQouta)
        {
            //reached qouta, increase qouta
            profitSumScript.goalCompleted = true;
            profitSumScript.goalNotCompleted = false;
            profitSumScript.terminated = false;

            profitSumScript.profitMade = profitMadeToday;
            profitSumScript.profitNeededLeft = Mathf.Clamp(currentQouta - totalProfit, 0, currentQouta - totalProfit);
            
            remainingDayCounter = 3;
            timesFulfilled++;
            IncreaseQouta();

            profitSumScript.newProfitGoal = currentQouta;
            profitSumScript.newDeadline = remainingDayCounter;
            totalProfit = 0;

            
            
        }
        else
        {
            remainingDayCounter--;
            if (remainingDayCounter <= 0)
            {
                profitSumScript.goalCompleted = false;
                profitSumScript.goalNotCompleted = false;
                profitSumScript.terminated = true;
                totalProfit = 0;
                profitMadeToday = 0;

            }
            else
            {
                profitSumScript.goalCompleted = false;
                profitSumScript.goalNotCompleted = true;
                profitSumScript.terminated = false;

                profitSumScript.profitMade = profitMadeToday;
                profitSumScript.profitNeededLeft = Mathf.Clamp(currentQouta - totalProfit, 0, currentQouta - totalProfit);
                profitSumScript.daysRemaining = remainingDayCounter;
            }
        }

        profitSummary.SetActive(true);
        
    }

    void Update()
    {
        profitQoutaText.text = string.Format("${0}/${1}", totalProfit, currentQouta);
        deadlineText.text = string.Format("{0} days remaining", remainingDayCounter);
    }

    private void OnDestroy()
    {
        WorldTime.onResetDay -= DailyReset;
        moneyHandler.onUpdateProfit -= UpdateProfitCount;
    }
}
