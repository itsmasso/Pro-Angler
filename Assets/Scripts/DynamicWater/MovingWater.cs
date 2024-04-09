using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MovingWater : MonoBehaviour
{
    [SerializeField]
    private SpriteShapeController spriteShapeController;
    private int cornerCount = 2;

    [Header("Wave Properties")]
    [Range(1, 100)]
    [SerializeField] private int waveCount = 6;
    [SerializeField] private float waveAmplitude = 1f;
    [SerializeField] private float waveFrequency = 1f;
    [SerializeField] private float waveSpeed = 0.5f;
    [SerializeField] private float seaFoamThickness = 1f;


    void Start()
    {
        SetWaves();
   
    }

    void Update()
    {
        if (spriteShapeController != null)
        {
            //moving water in wave like formation
            Spline waterSpline = spriteShapeController.spline;
            int waterPointsCount = waterSpline.GetPointCount();
            for (int i = 1; i <= waterPointsCount - cornerCount; i++)
            {
                // Calculate the height of each spring using a sine wave function
                float yPos = waveAmplitude * Mathf.Sin((Time.time * waveSpeed) + i * waveFrequency);
                Vector3 wavePosition = waterSpline.GetPosition(i);
                Vector3 adjustedWavePos = new Vector3(wavePosition.x, transform.localPosition.y + yPos, wavePosition.z);
                waterSpline.SetPosition(i, adjustedWavePos);           

            }




        }
    }



    private void SetWaves()
    {

        Spline waterSpline = spriteShapeController.spline;

        
        int waterPointsCount = waterSpline.GetPointCount();

        for (int i = cornerCount; i < waterPointsCount - cornerCount; i++)
        {
            waterSpline.RemovePointAt(cornerCount);
        }
        Vector3 waterTopLeftCorner = waterSpline.GetPosition(1);
        Vector3 waterTopRightCorner = waterSpline.GetPosition(2);
        float waterWidth = waterTopRightCorner.x - waterTopLeftCorner.x;
        float spacingPerWave = waterWidth / (waveCount + 1);

        // Set new points for the waves
        for (int i = waveCount; i > 0; i--)
        {
            int index = cornerCount;
            float xPosition = waterTopLeftCorner.x + (spacingPerWave * i);
            Vector3 wavePoint = new Vector3(xPosition, waterTopLeftCorner.y, waterTopLeftCorner.z);
            waterSpline.InsertPointAt(index, wavePoint);
            waterSpline.SetHeight(index, seaFoamThickness);
            waterSpline.SetCorner(index, false);
            waterSpline.SetTangentMode(index, ShapeTangentMode.Continuous);
            Smoothen(waterSpline, index);

        }

        
    }
    private void Smoothen(Spline waterSpline, int index)
    {
        Vector3 position = waterSpline.GetPosition(index);
        Vector3 positionPrev = position;
        Vector3 positionNext = position;
        if (index > 1)
        {
            positionPrev = waterSpline.GetPosition(index - 1);
        }
        if (index - 1 <= waveCount)
        {
            positionNext = waterSpline.GetPosition(index + 1);
        }

        Vector3 forward = gameObject.transform.forward;

        float scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;

        Vector3 leftTangent = (positionPrev - position).normalized * scale;
        Vector3 rightTangent = (positionNext - position).normalized * scale;

        SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent, out leftTangent);

        waterSpline.SetLeftTangent(index, leftTangent);
        waterSpline.SetRightTangent(index, rightTangent);
    }
}
