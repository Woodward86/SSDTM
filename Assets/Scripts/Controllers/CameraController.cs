using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(GameState))]
public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public GameState gameState;

    //Movement
    public bool followPlayer;
    public Vector3 cameraOffset;
    public float closeYOffset;
    public float farYOffset;
    public float yOffsetLimiter;
    public float smoothTime = .5f;
    private Vector3 velocity;

    //Zoom
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private void Awake()
    {
        gameState = GetComponent<GameState>();
    }


    void LateUpdate()
    {
        if (followPlayer)
        {
            MoveCamera();
            ZoomCamera();
        }
    }


    void MoveCamera()
    {
        Vector3 centerPoint = GetCenterPoint();

        float newYoffset = Mathf.Lerp(closeYOffset, farYOffset, GetGreatestDistance() / yOffsetLimiter);
        //Debug.Log(newXoffset);
        cameraOffset = new Vector3(cameraOffset.x, newYoffset, cameraOffset.z);
        Vector3 newPosition = centerPoint + cameraOffset;

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, newPosition, ref velocity, smoothTime);
    }


    void ZoomCamera()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, newZoom, Time.deltaTime);
    }


    public float GetGreatestDistance()
    {
        var bounds = new Bounds(gameState.players[0].transform.position, Vector3.zero);
        for (int i = 0; i < gameState.players.Count; i++)
        {
            bounds.Encapsulate(gameState.players[i].transform.position);
        }

        return bounds.size.x;
    }


    public Vector3 GetCenterPoint()
    {
        if (gameState.players.Count == 1)
        {
            return gameState.players[0].transform.position;
        }

        var bounds = new Bounds(gameState.players[0].transform.position, Vector3.zero);
        for (int i = 0; i < gameState.players.Count; i++)
        {
            bounds.Encapsulate(gameState.players[i].transform.position);
        }

        return bounds.center;
    }
}
