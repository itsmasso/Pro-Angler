using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class EatOrKeepDialogue : DialogueBox
{
    public static event Action<int> onEatFish;
    public static event Action<FishInfo> onKeepFish;
    public static event Action<bool> onEatOrKeepOption;

    protected bool onChoosenScreen;
    [SerializeField] protected GameObject chooseScreen;
    [SerializeField] protected int indexBeforeChooseScreen;
    protected int currentChooseIndex;
    [SerializeField] protected RectTransform chooseIndicator;
    [SerializeField] protected float indicatorMoveAmount = 35f;
    private bool chosenOption;
    [SerializeField] private Vector2 startingPos;

    public FishInfo fishInfo;

    protected override void Start()
    {
        base.Start();
        startingPos = chooseIndicator.anchoredPosition;
        chooseScreen.SetActive(false);
        chosenOption = false;
 
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if(fishInfo != null)
        {
            onChoosenScreen = false;
            chooseIndicator.anchoredPosition = startingPos;
            chooseScreen.SetActive(false);
            chosenOption = false;
            currentChooseIndex = 0;
            onEatOrKeepOption?.Invoke(true);
        }
    }
    public void OnTextUp(InputAction.CallbackContext ctx)
    {
        if (onChoosenScreen && ctx.performed && currentChooseIndex == 1)
        {
            chooseIndicator.anchoredPosition = new Vector2(chooseIndicator.anchoredPosition.x, chooseIndicator.anchoredPosition.y + indicatorMoveAmount);
            currentChooseIndex--;
        }
    }
    public void OnTextDown(InputAction.CallbackContext ctx)
    {
        if (onChoosenScreen && ctx.performed && currentChooseIndex == 0)
        {
            chooseIndicator.anchoredPosition = new Vector2(chooseIndicator.anchoredPosition.x, chooseIndicator.anchoredPosition.y - indicatorMoveAmount);
            currentChooseIndex++;
        }
    }

    public void OnNext(InputAction.CallbackContext ctx)
    {
        if(!onChoosenScreen && ctx.performed)
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
        else if(onChoosenScreen && ctx.performed)
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
        if (index >= indexBeforeChooseScreen && dialogueText.text == lines[index])
        {
            onChoosenScreen = true;
            chooseScreen.SetActive(true);
        }

        if (onChoosenScreen)
        {
            if(chosenOption && currentChooseIndex == 0)
            {
                //if choose eat
                onEatFish?.Invoke(fishInfo.stamRestored);
                onEatOrKeepOption?.Invoke(false);
                gameObject.SetActive(false);

            }
            else if (chosenOption && currentChooseIndex == 1)
            {
                //if choose sell
                onKeepFish?.Invoke(fishInfo);
                onEatOrKeepOption?.Invoke(false);
                gameObject.SetActive(false);
            }
        }
    }
}
