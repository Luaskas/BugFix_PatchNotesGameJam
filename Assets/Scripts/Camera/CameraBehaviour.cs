using System;
using Controller;
using Unity.Cinemachine;
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
    //public CameraStates currentCameraState;

    
    public LetterboxBars letterboxBars;

    public CinemachineCamera[] cameras;
    public CinemachineCamera activeCamera;
    
    public Vector3 cameraOffset = new Vector3(0, 0, 0);

    private bool dialogActive;
    
    private void Awake()
    {
        //currentCameraState = CameraStates.ActivePlayScene;
        
    }

    void Start()
    {
        FollowCamera();
    }
    /*
    void Update()
    {
        if (dialogActive)
        {
            activeCamera.transform.position = DialogTargetController.Instance.transform.right + cameraOffset;
        }
    }
    */
    void OnEnable()
    {
        PlayerController.OnDialogStarted += DialogCamera;
        PlayerController.OnDialogEnded += FollowCamera;
    }

    void OnDisable()
    {
        PlayerController.OnDialogStarted -= DialogCamera;
        PlayerController.OnDialogEnded -= FollowCamera;
    }
    
    /*
    void LateUpdate()
    {
        switch (currentCameraState)
        {
            case CameraStates.ActivePlayScene:
                GoToOffset(cameraPlayerOffset);
                letterboxBars.HideBars();
                break;
            case CameraStates.Dialog:
                letterboxBars.ShowBars();
                break;
            case CameraStates.Inactive:
                break;
        }
    }*/
    

    void DialogCamera()
    {
        dialogActive = true;
        DeactivateCameras();
        FindCameraInArray("DialogCam");
        activeCamera.Priority = 20;
        letterboxBars.ShowBars();
        
    }

    void FollowCamera()
    {
        dialogActive = false;
        DeactivateCameras();
        FindCameraInArray("FollowCam");
        activeCamera.Priority = 20;
        letterboxBars.HideBars();
    }

    void DeactivateCameras()
    {
        foreach (var camera in cameras)
        {
            camera.Priority = 0;
        }
    }

    void FindCameraInArray(string cameraName)
    {
        foreach (var cam in cameras)
        {
            string tempName;
            tempName = cam.gameObject.name;
            if(tempName == cameraName) activeCamera = cam;
        }
    }
    
}
