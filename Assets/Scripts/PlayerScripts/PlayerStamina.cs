using System.Collections;
using System.Collections.Generic;
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
        maxStamina = playerScriptable.maxStamina;
        currentStamina = maxStamina;
        outOfStamina = false;
    }

    public void DrainStamina(float drainAmount)
    {
        currentStamina -= drainAmount;
        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }
        //Debug.Log(currentStamina);
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
}