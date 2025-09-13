using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum CameraStates
{
    ActivePlayScene,
    Dialog,
    Inactive
}

public class CameraBehaviour : MonoBehaviour
{
    public CameraStates currentCameraState;

    public LetterboxBars letterboxBars;
    
    public Transform player;
    public Transform dialogSnap;
    
    [FormerlySerializedAs("cameraOffset")] public Vector3 cameraPlayerOffset = new Vector3(0f, 6f, -10f);
    public Vector3 cameraDialogOffset = new Vector3(5.67f, 0.97f, 2.15f);
    public float smoothSpeed = 5f;

    private void Awake()
    {
        currentCameraState = CameraStates.Dialog;
    }

    void LateUpdate()
    {
        switch (currentCameraState)
        {
            case CameraStates.ActivePlayScene:
                GoToOffset(cameraPlayerOffset);
                letterboxBars.HideBars();
                break;
            case CameraStates.Dialog:
                GoToOffset(cameraDialogOffset);
                letterboxBars.ShowBars();
                break;
            case CameraStates.Inactive:
                break;
        }
    }

    void GoToOffset(Vector3 offset)
    {
        Vector3 targetPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        
        if(currentCameraState == CameraStates.ActivePlayScene)
            transform.LookAt(player);
        if (currentCameraState == CameraStates.Dialog)
        {
            transform.LookAt(dialogSnap);
        }
        
    }

    void FollowDialog(Vector3 offset)
    {
        Vector3 targetPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
