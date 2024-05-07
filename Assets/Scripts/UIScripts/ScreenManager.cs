using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static event Action<bool> onScreenOpened;
    [SerializeField] private GameObject[] screenArr;

    private bool canOpen;
    private void Start()
    {
        canOpen = true;
        FishingRodBaseScript.onFishing += IsFishing;
    }
    private void IsFishing(bool isFishing)
    {
        if (isFishing)
        {
            canOpen = false;
        }
        else
        {
            canOpen = true;
        }
    }
    public void RodScreenOpened(GameObject screenToOpen)
    {
        if (canOpen)
        {
            if (screenToOpen.activeSelf == true)
            {
                screenToOpen.SetActive(false);
                onScreenOpened?.Invoke(false);
            }
            else
            {
                foreach (GameObject screen in screenArr)
                {
                    screen.SetActive(false);
                }

                screenToOpen.SetActive(true);
                onScreenOpened?.Invoke(true);
            }
        }
        
    }

    private void OnDestroy()
    {
        FishingRodBaseScript.onFishing -= IsFishing;
    }


}
