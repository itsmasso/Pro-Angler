using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FishIconPopup : MonoBehaviour
{
    [SerializeField] private float moveAmount = 3f;
    [SerializeField] private float lerpSpeed = 2f;
    [SerializeField] private float screenTime = 2f;
    [SerializeField] private float fadeDuration = 2f;
    public SpriteRenderer spriteRenderer;

    private float screenTimer;
    private Vector2 targetPosition;
    private bool callOnce;
    private void Start()
    {
        callOnce = false;
        targetPosition = new Vector2(transform.position.x, transform.position.y + moveAmount);
    }
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position, targetPosition) <= 0.5f)
        {
            screenTimer += Time.deltaTime;
            if(screenTimer >= screenTime)
            {
                if (!callOnce)
                {
                    StartCoroutine(FadeSprite());
                    
                }
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
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }

        //perform any action after fading out
        Destroy(gameObject);

    }
}
