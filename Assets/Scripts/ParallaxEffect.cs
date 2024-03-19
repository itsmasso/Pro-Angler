using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxSpeed;
    private Transform cameraTransform;
    private float startPosX;
    [SerializeField] private float xMapSize;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        startPosX = transform.position.x;

    }

    private void Update()
    {
        float relativedist = cameraTransform.position.x * parallaxSpeed;
        transform.position = new Vector3(startPosX + relativedist, transform.position.y, transform.position.z);

        //loop effect
        float relativeCamDist = cameraTransform.position.x * (1 - parallaxSpeed);
        if (relativedist > startPosX + xMapSize)
        {
            startPosX += xMapSize;
        }
        else if(relativeCamDist < startPosX - xMapSize)
        {
            startPosX -= xMapSize;
        }
    }
}
