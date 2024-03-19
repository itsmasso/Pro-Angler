using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookIndicatorScript : MonoBehaviour
{
    [SerializeField] private Transform centerPoint;
    //[SerializeField] private float rotationSpeed = 1.5f;
    public float defaultSpeedMultiplier = 1.5f;
    [SerializeField] private float speedMultplier;
    
    [SerializeField] private float radius = 2f; // Distance from the pivot point
    [SerializeField] private float increaseSpeedMultiplier = 0.5f;
    private float angle;
    private bool reverse;
    private float time;

    [SerializeField] private float timeBeforeStart = 1f;
    private bool canStart = false;
    void Start()
    {
        BarIndicatorScript.onSuccessfulHit += OnSuccessfulHit;
    }

    private void OnEnable()
    {
        //start in the top middle
        //stop moving when mechanic finished
        canStart = false;
        StartCoroutine(DelayedStart());
        speedMultplier = defaultSpeedMultiplier;
        reverse = false;
        time = Mathf.PI/ 2f;
        angle = time;
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius; //equation for circle (look up in google for better understanding)
        transform.position = centerPoint.position + offset;

        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); // Convert to degrees

    }


    private void OnSuccessfulHit()
    {
        reverse = !reverse; //reverse direction
        speedMultplier += increaseSpeedMultiplier;
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(timeBeforeStart);
        canStart = true;
    }
    
    void Update()
    {
        
        if (canStart)
        {
            if (reverse)
                time += speedMultplier * Time.deltaTime;
            else
                time -= speedMultplier * Time.deltaTime;
 
            angle = time;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius; //equation for circle (look up in google for better understanding)
            transform.position = centerPoint.position + offset;

            transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); // Convert to degrees
        }
    }

    private void OnDisable()
    {
        speedMultplier = defaultSpeedMultiplier;
        canStart = false;
    }

    private void OnDestroy()
    {
        BarIndicatorScript.onSuccessfulHit -= OnSuccessfulHit;
    }
}
