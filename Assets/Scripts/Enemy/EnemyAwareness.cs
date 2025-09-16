using System;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public event EventHandler onPlayerEnteredReach;
    public event EventHandler onPlayerLeftReach;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && onPlayerEnteredReach != null) onPlayerEnteredReach.Invoke(this, EventArgs.Empty); 
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && onPlayerLeftReach != null) onPlayerLeftReach.Invoke(this, EventArgs.Empty); 
    }
}
