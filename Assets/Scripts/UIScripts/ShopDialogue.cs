using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopDialogue : DialogueBox
{

    protected bool onChoosenScreen;
    [SerializeField] protected GameObject chooseScreen;
    protected int currentChooseIndex;
    [SerializeField] protected RectTransform chooseIndicator;
    [SerializeField] protected float indicatorMoveAmount = 35f;
    private bool chosenOption;
    [SerializeField] private Vector2 startingPos;
    [SerializeField] private GameObject shopScreen;
    
    public BucketScript bucket;
    [SerializeField] private GameObject shop;

    [SerializeField] private UI_manager uiManager;

    protected override void Start()
    {
        base.Start();
        startingPos = chooseIndicator.anchoredPosition;
        chooseScreen.SetActive(false);
        chosenOption = false;
        shopScreen.SetActive(false) ;

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        shop.SetActive(true);
        onChoosenScreen = false;
        chooseIndicator.anchoredPosition = startingPos;
        chooseScreen.SetActive(false);
        chosenOption = false;
        shopScreen.SetActive(false);
        currentChooseIndex = 0;
    }
    public void OnTextUp(InputAction.CallbackContext ctx)
    {
        if (onChoosenScreen && ctx.performed && currentChooseIndex != 0)
        {
            chooseIndicator.anchoredPosition = new Vector2(chooseIndicator.anchoredPosition.x, chooseIndicator.anchoredPosition.y + indicatorMoveAmount);
            currentChooseIndex--;
        }
    }
    public void OnTextDown(InputAction.CallbackContext ctx)
    {
        if (onChoosenScreen && ctx.performed && currentChooseIndex != 2)
        {
            chooseIndicator.anchoredPosition = new Vector2(chooseIndicator.anchoredPosition.x, chooseIndicator.anchoredPosition.y - indicatorMoveAmount);
            currentChooseIndex++;
        }
    }

    public void OnNext(InputAction.CallbackContext ctx)
    {
        if (!onChoosenScreen && ctx.performed)
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
            }

        }
        else if (onChoosenScreen && ctx.performed)
        {
            chosenOption = true;
        }
    }

    protected override void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            onChoosenScreen = true;
            chooseScreen.SetActive(true);
        }

    }

    protected override void Update()
    {
        base.Update();
        //Debug.Log(currentChooseIndex);
        if (onChoosenScreen)
        {
            if (chosenOption && currentChooseIndex == 0)
            {
                shopScreen.SetActive(true);
                gameObject.SetActive(false);

            }
            else if (chosenOption && currentChooseIndex == 1)
            {
                bucket.SellFish();
                chosenOption = false;


            }
            else if (chosenOption && currentChooseIndex == 2)
            {
                shop.SetActive(false);
                uiManager.isTimerRunning = true;
            }
        }
    }
}

