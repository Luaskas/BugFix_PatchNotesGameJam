using System;
using Controller;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerData : MonoBehaviour
{
    //public GameObject player;

    public static PlayerData Instance;
    
    public float maxHp = 15;
    public float currentHp;
    public int dmg = 5;

    public bool canTakeDamage = true;

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

    private void Die()
    {
        Debug.Log("Player Died.");
        //PlayerController.Instance.enabled = false;
        StartCoroutine(UI_Manager.Instance.FadeIn(0, 3f));
        GameManager.Instance.Respawn();
        StartCoroutine(UI_Manager.Instance.FadeOut(0, 3f));
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Deadzone"))
            GameManager.Instance.Respawn();
            
    }
    
}
