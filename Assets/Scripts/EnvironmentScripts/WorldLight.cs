using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class WorldLight : MonoBehaviour
{
    private Light2D _light;
    [SerializeField] private WorldTime worldTime;
    [SerializeField] private Gradient gradient;

    private void Awake()
    {
        _light = GetComponent<Light2D>();

    }

    private void GradientChange()
    {
        _light.color = gradient.Evaluate(worldTime.currentTime % worldTime.totalSecondsInADay / worldTime.totalSecondsInADay);
    }

    private void Update()
    {
        GradientChange();
    }
}
