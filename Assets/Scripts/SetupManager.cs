using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform resetPosition;
    [SerializeField] private GameObject interactButtonUI;
    [SerializeField] private Transform canFishPosition;
    [SerializeField] private CamFollow camFollow;
    void Start()
    {
        WorldTime.onResetDay += ResetPlayerPos;
        camFollow.target = player.transform;
        player.transform.position = resetPosition.position;
        player.GetComponentInChildren<FishingRodBaseScript>().interactButtonUI = interactButtonUI;
        player.GetComponentInChildren<FishingRodBaseScript>().canFishPosition = canFishPosition;
    }
    private void ResetPlayerPos()
    {
        player.transform.position = resetPosition.position;
    }

    private void OnDestroy()
    {
        WorldTime.onResetDay -= ResetPlayerPos;
    }


}
