using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RodTypeButton : MonoBehaviour
{
    [SerializeField] private Image currentRodImage;
    [SerializeField] private Sprite oldRod;
    private void Start()
    {
  
        currentRodImage.sprite = oldRod;
        //save system needed
        
        RodSelectionScreen.onChangeRod += ChangeRod;
      

    }


    private void ChangeRod(RodScriptableObject rod)
    {
        currentRodImage.sprite = rod.rodSprite;
    }

    private void OnDestroy()
    {
        RodSelectionScreen.onChangeRod -= ChangeRod;
       
    }
}
