using System;
using System.Collections;
using Controller;
using UnityEditor.UIElements;
using UnityEngine;

public class EnvironmentalController : MonoBehaviour
{
    private Collider collisionCollider;
    //private Collider parentCollider;
    private SphereCollider triggerZone;
    
    private void Start()
    {
        //parentCollider = gameObject.GetComponent<SphereCollider>();
        collisionCollider = gameObject.GetComponent<Collider>();
        triggerZone = gameObject.GetComponent<SphereCollider>();
    }
    
    
    // --- Deactivate the Collider for walkable Ground-GameObjects ---
    // takes string from PlayerController and dynamically takes effect of TeleportAbility to
    // Ground Collider or the ParentCollider in the breachable Walls/Platform
    // and activates both after waiting for 1 sec.
    public void DeactivateColliderAndWaitForActivation()
    {
        collisionCollider.enabled = false;
        StartCoroutine(ReActivateAllCollider());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.teleportTarget = gameObject;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.teleportTarget = null;
        }
    }

    IEnumerator ReActivateAllCollider()
    {
        yield return new WaitForSeconds(1f);
        collisionCollider.enabled = true;
    }

    
    
    
}
