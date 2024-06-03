using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : Singleton<TransitionManager>
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject transitionScreen;

    private void Start()
    {
        transitionScreen.SetActive(false);
    }


    public void ApplyCircleTransition()
    {
        transitionScreen.SetActive(true);
        
        anim.Play("CircleAnimation", -1, 0);
   

    }

    public void ApplySleepTransition()
    {
        transitionScreen.SetActive(true);
        anim.Play("SleepAnimation", -1, 0);
     
 
    }

    public void ApplyDayTimeTransition()
    {
        transitionScreen.SetActive(true);
        anim.Play("DayTimeAnimation", -1, 0);
    }

    public void ApplyNightTimeTransition()
    {
        transitionScreen.SetActive(true);
        anim.Play("NightTimeAnimation", -1, 0);
    }

    private void Update()
    {
     
        if (anim.enabled && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        {
            transitionScreen.SetActive(false);
        }
       
    }


    private void OnDestroy()
    {

    }

}
