using System;
using Controller;
using Player;
using UnityEngine;

public class AbilityPickUp : MonoBehaviour
{
    public AbilitiyType abilityType;
    public AbilitiesGeneral abilityToUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (abilityType)
            {
                case AbilitiyType.Teleport:
                    PlayerController.Instance.teleportAbility = abilityToUnlock; 
                    break;
                case AbilitiyType.Sprint:
                    PlayerController.Instance.sprintAbility = abilityToUnlock;
                    break;
                case AbilitiyType.DoubleJump:
                    PlayerController.Instance.doubleJumpAbility = abilityToUnlock;
                    break;
                case AbilitiyType.Shrink:
                    PlayerController.Instance.shrinkAbility = abilityToUnlock;
                    break;
            }
            AudioPlayer.Instance.PlayAbilityPickupSound();
            PlayerData.Instance.UnlockAbility(abilityToUnlock);
            Destroy(gameObject);
        }
        
    }
}
