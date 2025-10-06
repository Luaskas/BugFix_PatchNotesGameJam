using Controller;
using Unity.Cinemachine;
using UnityEngine;

public class AddNpcToTarget : MonoBehaviour
{
    public static AddNpcToTarget Instance;

    public float targetRadius = 0.5f;
    public float targetWeight = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    
   public void AddNpcTpoTargetGroup(GameObject npc)
    {
        var target = GetComponent<CinemachineTargetGroup>();
        var npcTransform = npc.transform;
        target.AddMember(npcTransform, targetWeight, targetRadius);
    }

    public void RemoveNpcFromTargetGroup(GameObject npc)
    {
        var target = GetComponent<CinemachineTargetGroup>();
        var npcTransform = npc.transform;
        target.RemoveMember(npcTransform);
    }
    
}
