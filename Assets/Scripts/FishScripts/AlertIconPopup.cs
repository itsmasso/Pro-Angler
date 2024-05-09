using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertIconPopup : MonoBehaviour
{
    [SerializeField] private float timeBeforeDestroy = 1f;
    private float timer;
    [SerializeField] private float fadeDuration = 0.25f;
    public Image sprite;

    private bool callOnce;
    void Start()
    {
        callOnce = false;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeBeforeDestroy)
        {
            if (!callOnce)
            {
                StartCoroutine(FadeSprite());

            }
        }
    }

    private IEnumerator FadeSprite()
    {
        // Fade out
        callOnce = true;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration); // Calculate alpha value for fading out
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            yield return null;
        }

        //perform any action after fading out
        gameObject.SetActive(false);

    }
}
