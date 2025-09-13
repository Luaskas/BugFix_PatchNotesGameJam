using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerData : MonoBehaviour
{
    //public GameObject player;
    
    private int maxHP = 10;
    private int currentHP;
    private int dmg = 5;


    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDmg(int enemyDmg)
    {
        currentHP -= enemyDmg;
        Debug.Log($"{currentHP} HP left from {maxHP} HP.");
    }

    public int MakeDmg()
    {
        return dmg;
    }
}
