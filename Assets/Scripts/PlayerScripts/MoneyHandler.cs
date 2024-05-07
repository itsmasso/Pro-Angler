using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] private int totalMoney;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private PlayerScriptableObject playerScriptable;
    public event EventHandler<int> onUpdateProfit;
    public static event Action<UpgradeScriptableObj> onTransact; //this event is not necessary but to ensure that we make sure player has enough money
    public static event Action<int> onUpdateMoney;
    private void Awake()
    {
        ShopManager.onBuyUpgrade += Transact;
        BucketScript.onSellFish += UpdateMoneyCount;
        if (totalMoney <= 0)
            totalMoney = 0;
        totalMoney = playerScriptable.money;

    }


    private void Start()
    {
        onUpdateMoney?.Invoke(totalMoney);
    }

    //change back to private function later
    private void UpdateMoneyCount(int sellAmount)
    {
        totalMoney += sellAmount;
        playerScriptable.money = totalMoney;
        onUpdateProfit?.Invoke(this, sellAmount);
        onUpdateMoney?.Invoke(totalMoney);
    }

    private void Transact(UpgradeScriptableObj upgrade)
    {
        if(totalMoney >= upgrade.cost)
        {
            totalMoney -= upgrade.cost;
            if (totalMoney <= 0)
                totalMoney = 0;
            playerScriptable.money = totalMoney;
            onTransact?.Invoke(upgrade);
            onUpdateMoney?.Invoke(totalMoney);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(totalMoney > 0)
        {
            moneyText.text = string.Format("${0:#,#}", totalMoney);
        }
        else
        {
            moneyText.text = string.Format("$0");
        }
    }

    private void OnDestroy()
    {
        ShopManager.onBuyUpgrade -= Transact;
        BucketScript.onSellFish -= UpdateMoneyCount;

    }
}
