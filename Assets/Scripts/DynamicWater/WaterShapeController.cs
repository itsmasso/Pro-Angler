using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteAlways]
public class WaterShapeController : MonoBehaviour
{
    [SerializeField]
    private float springStiffness = 0.1f;
    [SerializeField]
    private List<WaterSpringMovement> springs = new();
    [SerializeField]
    private float dampening = 0.03f; // Slowing the movement over time
    public float spread = 0.006f; //how much to spread to other springs

    [Header("Splash Wave Properties")]
    [SerializeField] private int cornerCount = 2;
    [SerializeField] private SpriteShapeController spriteShapeController;
    [Range(1,100)]
    [SerializeField] private int waveCount = 6;
    [SerializeField] private GameObject wavePointPrefab;
    [SerializeField] private GameObject wavePointsParent;
  

    [Header("Constant Wave Properties")]
    [SerializeField] private float waveAmplitude = 1f;
    [SerializeField] private float waveFrequency = 1f;
    [SerializeField] private float waveSpeed = 0.5f;

    private void Start()
    {

        if (Application.isPlaying)
        {
            foreach (Transform child in wavePointPrefab.transform)
            {
                Destroy(child.gameObject);
            }
            SetWaves();
        }
    }



    /*
    //Creating and destroying waves in editor
    private void OnValidate()
    {
        StartCoroutine(CreateWavesInEditor());
    }

    IEnumerator CreateWavesInEditor()
    {
        foreach (Transform child in wavePointsParent.transform)
        {
            StartCoroutine(DestroyInEditor(child.gameObject));
        }
        yield return null;
        SetWaves();
        yield return null;
    }
    IEnumerator DestroyInEditor(GameObject go)
    {
        yield return null;
        DestroyImmediate(go);
    }
    */

    private void CreateSprings(Spline waterSpline)
    {
        springs = new();
        for(int i = 0; i <= waveCount + 1; i++)
        {
            int index = i + 1;
            Smoothen(waterSpline, index);

            GameObject wavePoint = Instantiate(wavePointPrefab, wavePointsParent.transform, false);
            wavePoint.transform.localPosition = waterSpline.GetPosition(index);

            WaterSpringMovement waterSpring = wavePoint.GetComponent<WaterSpringMovement>();
            waterSpring.Init(spriteShapeController);
            springs.Add(waterSpring);
        }
    }

    private void SetWaves()
    {
       
        Spline waterSpline = spriteShapeController.spline;
        int waterPointsCount = waterSpline.GetPointCount();

        for(int i = cornerCount; i < waterPointsCount - cornerCount; i++)
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
            waterSpline.SetHeight(index, 0.1f);
            waterSpline.SetCorner(index, false);
            waterSpline.SetTangentMode(index, ShapeTangentMode.Continuous);
        }

        CreateSprings(waterSpline);
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
    private void UpdateSprings()
    {
        int count = springs.Count;
        float[] left_deltas = new float[count];
        float[] right_deltas = new float[count];
        for(int i = 0; i < count; i++)
        {
            if(i > 0)
            {
                left_deltas[i] = spread * (springs[i].height - springs[i - 1].height);
                springs[i - 1].velocity += left_deltas[i];
            }

            if(i < springs.Count - 1)
            {
                right_deltas[i] = spread * (springs[i].height - springs[i + 1].height);
                springs[i + 1].velocity += right_deltas[i];
            }
        }

        
    }
    void FixedUpdate()
    {
     
        foreach (WaterSpringMovement waterSpringComponent in springs)
        {
            
            waterSpringComponent.WaveSpringUpdate(springStiffness, dampening);
            waterSpringComponent.WavePointUpdate();
            
        }
       
        //moving water in wave like formation
        for (int i = 0; i < springs.Count; i++)
        {
            // Calculate the height of each spring using a sine wave function
            float yPos = waveAmplitude * Mathf.Sin((Time.time * waveSpeed) + i * waveFrequency);
            springs[i].transform.position = new Vector3(springs[i].transform.position.x, springs[i].transform.position.y + yPos, springs[i].transform.position.z);
        }

        UpdateSprings();
    }

    private void Splash(int index, float speed)
    {
        if(index >= 0 && index < springs.Count)
        {
            springs[index].velocity += speed;
        }
    }
}
