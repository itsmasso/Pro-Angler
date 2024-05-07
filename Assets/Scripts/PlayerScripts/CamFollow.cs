using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public Transform target; // The object the camera will follow
    [SerializeField] private float smoothSpeed = 0.125f; // How quickly the camera catches up to its target


    [SerializeField] private GameObject level1Confiner;
    [SerializeField] private GameObject lobbyConfiner;

    private BoxCollider2D level1BoxCollider;
    private BoxCollider2D lobbyBoxCollider;

    [SerializeField] private Vector2 lobbyMinBounds, lobbyMaxBounds;
    [SerializeField] private Vector2 level1MinBounds, level1MaxBounds;

    private float clampedX;
    private float clampedY;

    private Vector2 camOffset;
    [SerializeField] private bool onLobby, onLevel1;
    void Start()
    {
        
        onLobby = true;
        onLevel1 = false;
        Level1Trigger.onEnterLevel1 += OnLevel1;
        LobbyTrigger.onEnterLobby += OnLobby;
        WorldTime.onResetDay += OnLobby;
        FishingRodBaseScript.onFishing += SwitchCameraTarget;
        level1BoxCollider = level1Confiner.GetComponent<BoxCollider2D>();
        lobbyBoxCollider = lobbyConfiner.GetComponent<BoxCollider2D>();

        lobbyMinBounds = (Vector2)lobbyBoxCollider.transform.position - lobbyBoxCollider.size / 2f + lobbyBoxCollider.offset;
        lobbyMaxBounds = (Vector2)lobbyBoxCollider.transform.position + lobbyBoxCollider.size / 2f + lobbyBoxCollider.offset;

        level1MinBounds = (Vector2)level1BoxCollider.transform.position - level1BoxCollider.size / 2f + level1BoxCollider.offset;
        level1MaxBounds = (Vector2)level1BoxCollider.transform.position + level1BoxCollider.size / 2f + level1BoxCollider.offset;

        float cameraHalfWidth = cam.aspect * cam.orthographicSize; // Half of the camera's width
        float cameraHalfHeight = cam.orthographicSize; // Half of the camera's height
        camOffset = new Vector3(cameraHalfWidth, cameraHalfHeight, 0f);


    }

    private void SwitchCameraTarget(bool isFishing)
    {
        if (isFishing)
        {
            target = DataPersistenceManager.Instance.hook.transform;

        }
        else
        {
            target = DataPersistenceManager.Instance.fisherMan.transform;
        }
    }


    private void OnLevel1()
    {
        onLevel1 = true;
        onLobby = false;
    }

    private void OnLobby()
    {
        onLevel1 = false;
        onLobby = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (onLobby)
        {
            clampedX = Mathf.Clamp(target.position.x, lobbyMinBounds.x + camOffset.x, lobbyMaxBounds.x - camOffset.x);
            clampedY = Mathf.Clamp(target.position.y, lobbyMinBounds.y + camOffset.y, lobbyMaxBounds.y - camOffset.y);

        }
        else if (onLevel1)
        {
            clampedX = Mathf.Clamp(target.position.x, level1MinBounds.x + camOffset.x, level1MaxBounds.x - camOffset.x);
            clampedY = Mathf.Clamp(target.position.y, level1MinBounds.y + camOffset.y, level1MaxBounds.y - camOffset.y);

        }

        Vector3 desiredPosition = new Vector2(clampedX, clampedY);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void OnDestroy()
    {
        Level1Trigger.onEnterLevel1 -= OnLevel1;
        LobbyTrigger.onEnterLobby -= OnLobby;
        WorldTime.onResetDay -= OnLobby;
    }
}
