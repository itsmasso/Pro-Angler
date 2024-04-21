using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_manager : Singleton<UI_manager>
{
    public TMP_Text dayText;
    public TMP_Text moneyText;
    public TMP_Text timeText;
    public TMP_Text bucketText;

    public int totalMoney;

    public PlayerScriptableObject playerScriptable;
    [SerializeField] private BucketScript bucket;
    [SerializeField] private ShopManager ShopManager;

    public bool isCountDown;
    public float time;
    public float timeLimit;
    bool isTimerRunning = true;
    public TimeSpan timespan;
    // Start is called before the first frame update
    void Start()
    {

        totalMoney = playerScriptable.money;
    }

    // Update is called once per frame
    void Update()
    {
       moneyText.text = string.Format("Money: {0}", totalMoney);
        bucketText.text = string.Format("Bucket: {0}", bucket.currentFishAmount);
     
      if (isTimerRunning)
        {
            if (isCountDown)
                {
                    time -= Time.deltaTime;
                }
            else 
                {
                    time +=Time.deltaTime;
                }
             if (time >= timeLimit)
                {
                    time = timeLimit;
                    isTimerRunning = false;
                }   
            timespan = TimeSpan.FromSeconds(time);
            timeText.text = string.Format("Time: {0:D2}:{1:D2}" ,timespan.Minutes,timespan.Seconds);
        }
        
    }
    
    /*public void StartTimer()
        {
            isRunning = true;

        }
    */
 

}

