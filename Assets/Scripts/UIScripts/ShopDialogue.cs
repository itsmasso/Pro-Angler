using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopDialogue : DialogueBox
{
    public static event Action onSellFishOption;
    public event EventHandler<bool> onOpenShopWindow;
    public static event Action onExitShop;

    protected bool onChoosenScreen;
    [SerializeField] protected GameObject chooseScreen;
    protected int currentChooseIndex;
    [SerializeField] protected int indexBeforeChooseScreen;
    [SerializeField] protected RectTransform chooseIndicator;
    [SerializeField] protected float indicatorMoveAmount = 35f;
    private bool chosenOption;
    [SerializeField] private Vector2 startingPos;
    [SerializeField] private GameObject shopScreen;

    
    [SerializeField] private GameObject shopParent;

    
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
        onChoosenScreen = false;
        chooseIndicator.anchoredPosition = startingPos;
        chooseScreen.SetActive(false);
        chosenOption = false;
        onOpenShopWindow?.Invoke(this, false);
        currentChooseIndex = 0;
    }
    public void OnTextUp(InputAction.CallbackContext ctx)
    {
        if (onChoosenScreen && ctx.performed && currentChooseIndex != 0)
        {
            AudioManager.Instance.PlaySFX("ChoosingSFX", false);
            chooseIndicator.anchoredPosition = new Vector2(chooseIndicator.anchoredPosition.x, chooseIndicator.anchoredPosition.y + indicatorMoveAmount);
            currentChooseIndex--;
        }
    }
    public void OnTextDown(InputAction.CallbackContext ctx)
    {
        if (onChoosenScreen && ctx.performed && currentChooseIndex != 2)
        {
            AudioManager.Instance.PlaySFX("ChoosingSFX", false);
            chooseIndicator.anchoredPosition = new Vector2(chooseIndicator.anchoredPosition.x, chooseIndicator.anchoredPosition.y - indicatorMoveAmount);
            currentChooseIndex++;
        }
    }

    public void OnNext(InputAction.CallbackContext ctx)
    {
        if (!onChoosenScreen && ctx.performed)
        {
            AudioManager.Instance.PlaySFX("NextDialogueSFX", false);
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
            AudioManager.Instance.PlaySFX("NextDialogueSFX", false);
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


    }

    protected override void Update()
    {
        base.Update();
        if (index >= indexBeforeChooseScreen && dialogueText.text == lines[index])
        {
            onChoosenScreen = true;
            chooseScreen.SetActive(true);
        }

        //Debug.Log(currentChooseIndex);
        if (onChoosenScreen)
        {
            if (chosenOption && currentChooseIndex == 0)
            {
                onOpenShopWindow?.Invoke(this, true);
                gameObject.SetActive(false);

            }
            else if (chosenOption && currentChooseIndex == 1)
            {
                onSellFishOption?.Invoke();
                chosenOption = false;


            }
            else if (chosenOption && currentChooseIndex == 2)
            {
                shopParent.SetActive(false);
                onExitShop?.Invoke();

            }
        }
    }
}

