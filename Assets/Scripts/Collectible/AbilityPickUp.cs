using Controller;
using UnityEngine;

public class AbilityPickUp : MonoBehaviour
{
    public AbilitiesGeneral abilityToUnlock;
    private int unlockCounter = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (unlockCounter)
            {
                case 1:
                    PlayerController.Instance.teleportAbility = abilityToUnlock; 
                    break;
                case 2:
                    PlayerController.Instance.sprintAbility = abilityToUnlock;
                    break;
                case 3:
                    PlayerController.Instance.doubleJumpAbility = abilityToUnlock;
                    break;
                case 4:
                    PlayerController.Instance.shrinkAbility = abilityToUnlock;
                    break;
            }
            
            PlayerData.Instance.UnlockAbility(abilityToUnlock);
            unlockCounter++;
            Destroy(gameObject);
        }
        
    }
}
