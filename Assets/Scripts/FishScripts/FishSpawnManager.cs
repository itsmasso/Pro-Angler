using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishSpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> dayTimeSpawners = new List<GameObject>();
    [SerializeField] private GameObject nightTimeSpawner;
    void Start()
    {
        WorldTime.onTimeChange += OnTimeChange;
    }

    private void OnTimeChange(DayPeriod dayPeriod)
    {
        if(dayPeriod == DayPeriod.DayTime)
        {
            foreach(GameObject spawners in dayTimeSpawners)
            {
                spawners.SetActive(true);
            }

            nightTimeSpawner.SetActive(false);
        }
        else if(dayPeriod == DayPeriod.NightTime)
        {
            foreach (GameObject spawners in dayTimeSpawners)
            {
                spawners.SetActive(false);
            }

            nightTimeSpawner.SetActive(true);
        }
    }

    void Update()
    {
        
    }
    private void OnDestroy()
    {
        WorldTime.onTimeChange -= OnTimeChange;
    }
}
