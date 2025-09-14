using System;
using Controller;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 10;
    public int damage = 3;
    public Vector3 knockBack = new Vector3(0f, 0f, 0f);
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} got {damage} damage.");
        if (health <= 0)
            Die();
    }

    public int MakeDamage()
    {
        return damage;
    }
    
    void Die()
    {
        Destroy(gameObject);
    }


    /*public void OnCollisionEnter(Collision other)
    {
        PlayerData player = other.gameObject.GetComponent<PlayerData>();
        if (player != null)
        {
            player.TakeDmg(damage);
        }
    }*/

    public void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

        if (playerController != null)
        {
            Vector3 knockBackDir = playerController.transform.position - transform.position;
            playerController.ApplyKnockBack(knockBackDir, damage);
        }
        
    }
    
}
