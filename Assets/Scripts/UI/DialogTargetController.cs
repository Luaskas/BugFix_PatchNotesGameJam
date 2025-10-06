using System;
using Controller;
using UnityEngine;

public class DialogTargetController : MonoBehaviour
{
    public static DialogTargetController Instance;
    
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
}
