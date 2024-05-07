using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BaitType
{
    Worms,
    Grubs,
    AdvancedLure,
    MasterLure,
    OmniBait
}
public class BaitHolder : MonoBehaviour
{
    public BaitType currentBait;
    public void Start()
    {
        currentBait = BaitType.Worms;
        BaitSelectionScreen.onChangeBait += ChangeBait;
    }

    private void ChangeBait(BaitType bait)
    {
        currentBait = bait;
    }
}
