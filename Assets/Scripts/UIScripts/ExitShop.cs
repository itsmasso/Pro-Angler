using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShop : MonoBehaviour
{
    [SerializeField] private GameObject shopDialogue;
    public void Exit_Shop()
    {
        shopDialogue.SetActive(true);
        gameObject.SetActive(false);
    }
}
