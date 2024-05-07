using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MoneyHandler))]
public class QoutaSystem : MonoBehaviour
{
    [Header("Qouta System")]
    [SerializeField] private float currentQouta = 130;
    private MoneyHandler moneyHandler;
    private float currentProfit;
    private float nextQoutaIncrease;
    public int timesFulfilled; //save system
    private int remainingDayCounter = 3;

    [Header("Text")]
    [SerializeField] private TMP_Text deadlineText;
    [SerializeField] private TMP_Text profitQoutaText;

    private void Awake()
    {
        WorldTime.onResetDay += OnResetDay;
        moneyHandler = GetComponent<MoneyHandler>();
        moneyHandler.onUpdateProfit += UpdateProfitCount;

    }
    void Start()
    {
        
    }

    private void IncreaseQouta()
    {
        //Adjust if needed
        nextQoutaIncrease = Mathf.CeilToInt(100 * (1 + timesFulfilled ^ 2 / 16) * UnityEngine.Random.value + 1);
        currentQouta += nextQoutaIncrease;
    }

    private void UpdateProfitCount(object sender, int sellAmount)
    {
        currentProfit += sellAmount;
    }

    private void OnResetDay()
    {
        if (currentProfit >= currentQouta)
        {
            //reached qouta, increase qouta

            remainingDayCounter = 3;
            timesFulfilled++;
            IncreaseQouta();
            currentProfit = 0;
        }
        else
        {
            remainingDayCounter--;
            if (remainingDayCounter <= 0)
            {
                //lose
                Debug.Log("You lost!");
            }
        }
    }

    void Update()
    {
        profitQoutaText.text = string.Format("${0}/${1}", currentProfit, currentQouta);
        deadlineText.text = string.Format("{0} days remaining", remainingDayCounter);
    }

    private void OnDestroy()
    {
        WorldTime.onResetDay -= OnResetDay;
        moneyHandler.onUpdateProfit -= UpdateProfitCount;
    }
}
