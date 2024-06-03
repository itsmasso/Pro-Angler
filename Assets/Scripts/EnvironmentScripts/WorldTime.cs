using System;

using TMPro;

using UnityEngine;


public enum DayPeriod
{
    DayTime,
    NightTime
}

public class WorldTime : MonoBehaviour
{
    public static event Action onResetDay;
    public static event Action<bool> onSleepDeprived;
    public static event Action<DayPeriod> onTimeChange;
    private bool dayTimeFlag;

    private bool checkIfSleepDeprived;

    public float currentTime { private set; get; }
    public int day { private set; get; }
    public float totalSecondsInADay;
    private bool isTimerRunning = true;
    private bool canReset;
    //private int lastDisplayedSecond = -1;
    private int intHours;
    //private int minutesIncrement = 0;
    private bool canTransition;

    [Header("Text")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Animator iconAnim;

    private bool sleeping;
    private int daysAwake;
    void Start()
    {
        dayTimeFlag = false;
        Time.timeScale = 1;
        sleeping = false;
        canTransition = true;
        day = 1;
        intHours = 6;
        SleepTrigger.onSleep += SkipDay;
        QoutaSystem.onContinueToNextDay += UnpauseTime;
        shoptrigger.onShopEnter += PauseTime;
        ShopDialogue.onExitShop += UnpauseTime;
        canReset = true;
    }

    private void SkipDay()
    {
        daysAwake = 0;
        sleeping = true;
        currentTime = totalSecondsInADay - 1f;
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
        if (checkIfSleepDeprived)
        {
            if (daysAwake > 1)
            {
                onSleepDeprived?.Invoke(true);
            
            }
            else
            {
                onSleepDeprived?.Invoke(false);
            }
            checkIfSleepDeprived = false;
        }

        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            
            if (canTransition && currentTime >= totalSecondsInADay - 1f)
            {
                if (!sleeping)
                {
                    TransitionManager.Instance.ApplyCircleTransition();

                }
                else
                {
                    TransitionManager.Instance.ApplySleepTransition();
                }

                canTransition = false;
            }

            if (currentTime > totalSecondsInADay / 2 && !dayTimeFlag)
            {
                onTimeChange?.Invoke(DayPeriod.NightTime);
                TransitionManager.Instance.ApplyNightTimeTransition();
                dayTimeFlag = true;
            }

            if (currentTime >= totalSecondsInADay && canReset)
            {
                onTimeChange?.Invoke(DayPeriod.DayTime);
                currentTime = totalSecondsInADay;
                isTimerRunning = false;
                day++;
                daysAwake++;
                checkIfSleepDeprived = true;
                canTransition = true;
                onResetDay?.Invoke();
                dayTimeFlag = false;
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
        SleepTrigger.onSleep -= SkipDay;
        shoptrigger.onShopEnter -= PauseTime;
        ShopDialogue.onExitShop -= UnpauseTime;
        QoutaSystem.onContinueToNextDay -= UnpauseTime;
    }
}
