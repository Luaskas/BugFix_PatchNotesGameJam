using UnityEngine;

public class EnvironmentalController : MonoBehaviour
{
    
    
    public void DeactivateGroundCollider()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public void ActivateGroundCollider()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
