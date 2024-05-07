using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitTypeButton : MonoBehaviour
{
    [SerializeField] private Sprite wormsSprite;
    [SerializeField] private Sprite grubsSprite;
    [SerializeField] private Sprite advancedLureSprite;
    [SerializeField] private Sprite masterLureSprite;
    [SerializeField] private Sprite omniBaitSprite;

    [SerializeField] private Image currentBaitImage;

    private void Start()
    {

        currentBaitImage.sprite = wormsSprite;
        //save system needed
        BaitSelectionScreen.onChangeBait += ChangeBaitSprite;
        
    }


    private void ChangeBaitSprite(BaitType bait)
    {
        switch (bait)
        {
            case BaitType.Worms:
                currentBaitImage.sprite = wormsSprite;
                break;
            case BaitType.Grubs:
                currentBaitImage.sprite = grubsSprite;
                break;
            case BaitType.AdvancedLure:
                currentBaitImage.sprite = advancedLureSprite;
                break;
            case BaitType.MasterLure:
                currentBaitImage.sprite = masterLureSprite;
                break;
            case BaitType.OmniBait:
                currentBaitImage.sprite = omniBaitSprite;
                break;
        }
    }

    private void OnDestroy()
    {
        BaitSelectionScreen.onChangeBait -= ChangeBaitSprite;

    }
}
