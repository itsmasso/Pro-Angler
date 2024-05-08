using System;
using TMPro;
using UnityEngine;


public class WorldTime : MonoBehaviour
{
    public static event Action onResetDay;

    [Header("Time Properties")]
    private bool isCountDown;
    public float currentTime { private set; get; }
    private int day;
    public float totalSecondsInADay;
    private bool isTimerRunning = true;
    private bool canReset;
    //private int lastDisplayedSecond = -1;
    private int intHours;
    //private int minutesIncrement = 0;

    [Header("Text")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Animator iconAnim;


    void Start()
    {
        day = 1;
        intHours = 6;
        QoutaSystem.onContinueToNextDay += UnpauseTime;
        shoptrigger.onShopEnter += PauseTime;
        ShopDialogue.onExitShop += UnpauseTime;
        canReset = true;
    }


    private void PauseTime()
    {
        isTimerRunning = false;
    }

    private void UnpauseTime()
    {
        isTimerRunning = true;
    }

    private void ScaleTime()
    {
        float scaledTime = Mathf.Repeat(currentTime, totalSecondsInADay) / totalSecondsInADay;
        int hours = Mathf.FloorToInt(scaledTime * 24) + 6; // Scale from 6 AM to 12 AM (6 hours)
        int displayHours = hours % 12;
        if (displayHours == 0) displayHours = 12; // 12-hour clock
        //int minutes = Mathf.FloorToInt((scaledTime % 3600f) / 60f); // 60 seconds in a minute

        //timeText.text = string.Format("{0:00}:{1:D2} {2}", hours, minutes, hours >= 12f ? "PM" : "AM");
        timeText.text = string.Format("{0:00}:00 {1}", displayHours, (hours < 12 || hours >= 24) ? "AM" : "PM");

    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (isCountDown)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime += Time.deltaTime;
            }
            if (currentTime >= totalSecondsInADay && canReset)
            {
                currentTime = totalSecondsInADay;
                isTimerRunning = false;
                day++;

                onResetDay?.Invoke();
                currentTime = 0;

                //maybe later if on another island, reset by scene reset instead of event
            }
            ScaleTime();
        }

        

        if (intHours >= 6 && intHours <= 12)
        {
            iconAnim.SetBool("IsDayTime", true);
            iconAnim.SetBool("IsNightTime", false);
        }
        else
        {
            iconAnim.SetBool("IsDayTime", false);
            iconAnim.SetBool("IsNightTime", true);
        }

        dayText.text = string.Format("{0}", day);
    }

    private void OnDestroy()
    {
        shoptrigger.onShopEnter -= PauseTime;
        ShopDialogue.onExitShop -= UnpauseTime;
        QoutaSystem.onContinueToNextDay -= UnpauseTime;
    }
}
