using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private Slider staminaBar;
    [SerializeField] private PlayerScriptableObject playerScriptable;
    public float currentStamina;
    private float maxStamina;
    public bool outOfStamina;

    [SerializeField] private TMP_Text staminaText;
    void Start()
    {
        //add to save system
        maxStamina = playerScriptable.maxStamina;      
        currentStamina = maxStamina;
        playerScriptable.currentStamina = currentStamina;
        outOfStamina = false;
        EatOrKeepDialogue.onEatFish += RestoreStamina;
        EnergyDrinkScript.onIncreaseStaminaCap += IncreaseStaminaCap;
    }

    private void IncreaseStaminaCap(int increaseAmount)
    {
        maxStamina += increaseAmount;
        currentStamina = maxStamina;
        //playerScriptable.maxStamina = (int)maxStamina;
    }

    public void DrainStamina(float drainAmount)
    {
        currentStamina -= drainAmount;
        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }
        playerScriptable.currentStamina = currentStamina;
        //Debug.Log(currentStamina);
    }

    private void RestoreStamina(int amount)
    {
        currentStamina += amount;
        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
        playerScriptable.currentStamina = currentStamina;
    }



    void Update()
    {
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
        EatOrKeepDialogue.onEatFish -= RestoreStamina;
        EnergyDrinkScript.onIncreaseStaminaCap -= IncreaseStaminaCap;
    }
}
