using Controller;
using UnityEngine;

public class NpcScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) PlayerController.Instance.collisionWithNpc = true;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")) PlayerController.Instance.collisionWithNpc = false;
    }
}
