using System;
using System.Collections.Generic;
using Controller;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    
    public float maxHp = 15;
    public float currentHp;
    public int dmg = 5;
    private int errors = 0;

    
    public List<AbilitiesGeneral> abilities = new List<AbilitiesGeneral>();

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

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDmg(int enemyDmg)
    {
        currentHp -= enemyDmg;
        float currentHpInPercent = currentHp / maxHp;
        Debug.Log($"{currentHp} HP left from {maxHp} HP.");
        if (currentHp <= 0)
            Die();
    }

    public int MakeDmg()
    {
        return dmg;
    }

    public void IncreaseErrors()
    {
        errors++;
    }
    
    public int GetErrors() => errors;

    private void Die()
    {
        Debug.Log("Player Died.");
        StartCoroutine(UI_Manager.Instance.FadeIn(0, 3f));
        PlayerController.Instance.Respawn();
        StartCoroutine(UI_Manager.Instance.FadeOut(0, 3f));
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Deadzone"))
        {
            Debug.Log("Deadzone triggered.");
            PlayerController.Instance.Respawn();
        }
    }
    
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
            UI_Manager.Instance.ShowAbilitieButton(ability);
            Debug.Log($"Ability {ability.abilitieName} freigeschaltet!");
        }
    }
    
    
}
