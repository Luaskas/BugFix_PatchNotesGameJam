using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using System.Collections;

public class UI_Manager : MonoBehaviour
{
    //public RectTransform[] panels;
    
    public Transform panel;

    public Image HpBar;
    
    public Button[] abilityButtons = new Button[4];
    
    public static UI_Manager Instance;
    
    public float speed = 1f;
    public float hpSmooth = 0.0f;
    public float smoothFade = 0.0f;

    public string scene;
    
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
        
        HpBar.fillAmount = hpSmooth;
        if (HpBar.fillAmount >= 0.6f)
        {
            HpBar.color = Color.green;
        }
            
        else if (HpBar.fillAmount >= 0.3f && HpBar.fillAmount < 0.6f)
        {
            HpBar.color = Color.yellow;
        }

        else
        {
            HpBar.color = Color.red;
        }
    }
    public void OnSceneButtonClicked(int sceneIndex)
    {
        GameScene scene = (GameScene)sceneIndex;
        SceneLoader.LoadScene(scene);
    }
    
    [SerializeField] private Image[] panels;

    // Fade In
    public IEnumerator FadeIn(int index, float duration)
    {
        Image panel = panels[index];
        panel.gameObject.SetActive(true);
        Color c = panel.color;
        c.a = 0f;
        panel.color = c;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / duration); // 0 → 1
            panel.color = c;
            yield return null;
        }

        c.a = 1f;
        panel.color = c;
    }

    // Fade Out
    public IEnumerator FadeOut(int index, float duration)
    {
        Image panel = panels[index];
        Color c = panel.color;
        c.a = 1f;
        panel.color = c;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / duration); // 1 → 0
            panel.color = c;
            yield return null;
        }

        c.a = 0f;
        panel.color = c;
        panel.gameObject.SetActive(false);
    }

    public void ShowAbilitieButton(AbilitiesGeneral unlockedAbility)
    {
        Debug.Log($"AbilityButton {unlockedAbility.abilitieName} will be shown.");
        switch (unlockedAbility.abilitieName)
        {
            case "Teleport":
                abilityButtons[0].gameObject.SetActive(true);
                break;
            case "Sprint":
                abilityButtons[1].gameObject.SetActive(true);
                break;
            case "Double Jump":
                abilityButtons[2].gameObject.SetActive(true);
                break;
            case "Shrink":
                abilityButtons[3].gameObject.SetActive(true);
                break;
        }
    }
    
}
