using System;
using Controller;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject spawn;
    public GameObject player;
    public Vector3 spawnPos = new Vector3(-10f, 0.5f, -20f);
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //SetSpawnPoint(GameScene.Level1);
    }

    public void SetSpawnPoint(GameScene gameScene)
    {
        if (gameScene == GameScene.Level1)
            spawnPos = new Vector3(-10f, 0.5f, -20f);
        else if (gameScene == GameScene.Level2)
            spawnPos = new Vector3(10f, 0.5f, -20f);
        else
            spawnPos = new Vector3(-10f, 0.5f, -20f);
    }

    public void Respawn()
    {
        Debug.Log("Respawn");
        PlayerData.Instance.currentHp = PlayerData.Instance.maxHp;
        
        PlayerData.Instance.GetComponent<CharacterController>().enabled = false;
        PlayerData.Instance.transform.position = spawn.transform.position;
        PlayerData.Instance.GetComponent<CharacterController>().enabled = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            Respawn();
            
    }
    
}
