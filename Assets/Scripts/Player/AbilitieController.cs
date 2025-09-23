using UnityEngine;
using System.Collections.Generic;
public class AbilitieController : MonoBehaviour
{
    public List<AbilitiesGeneral> unlockedAbilities = new();

    public bool HasAbility(AbilitiesGeneral ability)
    {
        return unlockedAbilities.Contains(ability);
    }

    public void UnlockAbility(AbilitiesGeneral ability)
    {
        if (!unlockedAbilities.Contains(ability))
        {
            unlockedAbilities.Add(ability);
            Debug.Log($"Ability {ability.abilitieName} freigeschaltet!");
        }
    }

}
