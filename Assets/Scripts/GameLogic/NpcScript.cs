using System;
using Controller;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class NpcScript : MonoBehaviour
{
    [Header("Rotation Elements")]
    private Vector3 defaultRotation;
    private Quaternion targetRotation;
    private bool isRotating;
    [SerializeField] private float rotationSpeed = 10f;
    
    [Header("Dialog Elements")]
    [SerializeField] private Collider distanceKeeper;
    [SerializeField] private Dialogs initialDialog;
    
    
    // --- Subscribes the DialogStarted and DialogEnded Event from PlayerController,
    // -- when the script is enabled.
    void OnEnable()
    {
        PlayerController.OnDialogStarted += StartConversation;
        PlayerController.OnDialogEnded += EndConversation;
    }

    // --- Saves the initial rotation of the npc, so it is able to get back to it, after ending a dialog.
    void Start()
    {
        defaultRotation = transform.forward;
    }

    // --- Descubscribes the DialogStarted and DialogEnded events from PlayerController, 
    // -- when script is being disabled.
    void OnDisable()
    {
        PlayerController.OnDialogStarted -= StartConversation;
        PlayerController.OnDialogEnded -= EndConversation;
    }
    
    // --- When the player collides with the Trigger Area of the npc-object, it gets the PlayerController-script
    // -- and switches the collisionWithNpc-bool to true. With that, the PlayerController can fire the 
    // -- DialogStarted-Event when the player hits the Interact-button,
    // -- as there is a interactible NPC in reach of the player.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) PlayerController.Instance.collisionWithNpc = true;
        Debug.Log($"{other.name} collided with {gameObject.name}");
    }
    
    
    // --- Analog to the TriggerEnter method, this method sets the CollisionWithNpc-bool to false,
    // -- so the PlayerController script won't fire the DialogStarted-Event, when player is pressing 
    // -- the Interact-button
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")) PlayerController.Instance.collisionWithNpc = false;
    }

    
    
    void StartConversation()
    {
        // --- Gets the direction vector from NPC-position to Player-position
        // and sets the y coordinate to 0, so the NPC won't rotate up or down.
        Vector3 lookDirection = PlayerController.Instance.transform.position - transform.position;
        lookDirection.y = 0;
        
        // --- Transfers the direction vector to a Quaternion and sets isRotating-bool to true,
        // -- so the Update-loop can start rotating the npc towards the player.
        targetRotation = Quaternion.LookRotation(lookDirection);
        isRotating = true;
        
        // --- DistanceKeeper is changed to a trigger, so it won't push the player away
        // -- when rotation towards them.
        distanceKeeper.isTrigger = true;
        
        // --- locks the dialogTarget object from player to the position of npc
        // --- Adds the npc-instance to the targetGroup unity-function, to get the
        // -- dialog-cam to look at the average position from the dialogTarget and the npc
        // --- Calls the ShowDialog() Method with the initalDialig ScriptableObject, attached
        // -- to the npc
        DialogTargetController.Instance.transform.position = transform.position;
        AddNpcToTarget.Instance.AddNpcTpoTargetGroup(gameObject);
        DialogManager.Instance.ShowDialog(initialDialog);
    }

    // --- Rotates the npc to targetRotation
    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
            {
                isRotating = false;
            }
        }
    }

    // --- Changes the targetRotation to default and removes the npc from the dialog camera target group.
    void EndConversation()
    {
        Debug.Log("Look normal");
        targetRotation = Quaternion.LookRotation(defaultRotation);
        isRotating = true;
        distanceKeeper.isTrigger = false;
        AddNpcToTarget.Instance.RemoveNpcFromTargetGroup(gameObject);
    }
    
    
}
