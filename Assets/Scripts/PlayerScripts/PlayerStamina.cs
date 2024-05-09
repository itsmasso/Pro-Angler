
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    public static event Action<bool> onOutOfStamina;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private PlayerScriptableObject playerScriptable;
    public float currentStamina;
    private float maxStamina;
    public bool outOfStamina;

    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private GameObject tiredText;
    private float currentStaminaDrain = 0;
    private bool startDraining;
    private bool isSleepDeprived;

    void Start()
    {
        //add to save system
        tiredText.SetActive(false);
        isSleepDeprived = false;
        startDraining = false;
        maxStamina = playerScriptable.maxStamina;      
        currentStamina = maxStamina;
        playerScriptable.currentStamina = currentStamina;
        outOfStamina = false;
        SleepTrigger.onSleep += ReplenishStamina;
        WorldTime.onSleepDeprived += SleepDeprived;
        FishingRodBaseScript.onStopDrainingStam += StopDraining;
        FishingRodBaseScript.onStartDrainingStam += ContinouslyDrain;
        EatOrKeepDialogue.onEatFish += RestoreStamina;
        EnergyDrinkScript.onIncreaseStaminaCap += IncreaseStaminaCap;
    }

    private void ReplenishStamina()
    {
        currentStamina = maxStamina;
    }

    private void SleepDeprived(bool _isSleepDeprived)
    {
        isSleepDeprived = _isSleepDeprived;
        tiredText.SetActive(isSleepDeprived);
    }

    private void IncreaseStaminaCap(int increaseAmount)
    {
        maxStamina += increaseAmount;
        currentStamina = maxStamina;
        //playerScriptable.maxStamina = (int)maxStamina;
    }

    private void ContinouslyDrain(float amount)
    {
        startDraining = true;
        currentStaminaDrain = amount;
    }
    private void StopDraining()
    {
        startDraining = false;
    }


    public void DrainStamina(float drainAmount)
    {
        currentStamina -= drainAmount;
        if (currentStamina <= 0)
        {
            currentStamina = 0;
            onOutOfStamina?.Invoke(true);
        }
        playerScriptable.currentStamina = currentStamina;
        //Debug.Log(currentStamina);
    }

    private void RestoreStamina(int amount)
    {
        currentStamina += amount;
        onOutOfStamina?.Invoke(false);
        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
        playerScriptable.currentStamina = currentStamina;
    }



    void Update()
    {
        if (isSleepDeprived)
        {
            DrainStamina(2f * Time.deltaTime);
        }


        if (startDraining)
        {
            DrainStamina(currentStaminaDrain * Time.deltaTime);
        }
        if (currentStamina <= 0)
        {
            outOfStamina = true;
        }
        else
        {
            outOfStamina = false;
        }

        staminaBar.value = currentStamina / maxStamina;
        staminaText.text = string.Format("{0} / {1}", (int)Mathf.Round(currentStamina), (int)Mathf.Round(maxStamina));
    }
    private void OnDestroy()
    {
        SleepTrigger.onSleep -= ReplenishStamina;
        FishingRodBaseScript.onStopDrainingStam -= StopDraining;
        FishingRodBaseScript.onStartDrainingStam -= ContinouslyDrain;
        WorldTime.onSleepDeprived -= SleepDeprived;
        EatOrKeepDialogue.onEatFish -= RestoreStamina;
        EnergyDrinkScript.onIncreaseStaminaCap -= IncreaseStaminaCap;
    }
}
