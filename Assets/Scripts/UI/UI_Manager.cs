using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class UI_Manager : MonoBehaviour
{
    public RectTransform[] panels;
    
    public Transform panel;

    public Image HpBar;
    
    public static UI_Manager Instance;
    
    public float speed = 1f;
    public float hpSmooth = 0.0f;
    
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
    
    void Update()
    {
        float target = PlayerData.Instance.currentHp/PlayerData.Instance.maxHp;
        
        hpSmooth = Mathf.MoveTowards(hpSmooth, target, speed * Time.deltaTime);
        
        //hpBarFillAmount = Mathf.MoveTowards(currentPlayerHpInPercent, targetValue, Time.deltaTime * speed);
        
        HpBar.fillAmount = hpSmooth;
        if (HpBar.fillAmount >= 0.6f)
        {
            HpBar.color = Color.green;
            //HpBarBG.color = HpBar.color;
        }
            
        else if (HpBar.fillAmount >= 0.3f && HpBar.fillAmount < 0.6f)
        {
            HpBar.color = Color.yellow;
            //HpBarBG.color = HpBar.color;
        }

        else
        {
            HpBar.color = Color.red;
            //HpBarBG.color = HpBar.color;
        }
        
    }
    
    
    
}
