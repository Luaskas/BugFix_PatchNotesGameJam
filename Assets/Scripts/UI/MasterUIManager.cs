using System.Collections.Generic;
using System.Linq;
using Controller;
using UnityEngine;
using UnityEngine.Rendering;

public enum panelNames
{
    InGameHUD,
    DialogHUD
}

[System.Serializable]
public struct ListAllPanels
{
    public panelNames name;
    public RectTransform panel;
}

public class MasterUIManager : MonoBehaviour
{
    public List<ListAllPanels> panelList = new List <ListAllPanels>();
    
    private Dictionary<panelNames, RectTransform> panelsDict = new();
    private RectTransform activePanel;

    
    void Awake()
    {
        panelsDict = panelList.ToDictionary(p => p.name, p => p.panel);
    }
    
    void OnEnable()
    {
        PlayerController.OnDialogStarted += DialogHUDOnEvent;
        PlayerController.OnDialogEnded += InGameHUDOnEvent;
        
    }
    
    void OnDisable()
    {
        PlayerController.OnDialogStarted -= DialogHUDOnEvent;
        PlayerController.OnDialogEnded -= InGameHUDOnEvent;
    }
    
    void ActivateDialogHUD()
    {
        foreach (var element in panelList)
        {
            element.panel.gameObject.SetActive(false);
        }
    }

    public void ShowPanel(panelNames name)
    {
        foreach (var element in panelList)
        {
            element.panel.gameObject.SetActive(false);
        }
        
        if (panelsDict.TryGetValue(name, out var panel))
        {
            activePanel = panel;
            activePanel.gameObject.SetActive(true);
        }
    }

    private void DialogHUDOnEvent()
    {
        ShowPanel(panelNames.DialogHUD);
    }

    private void InGameHUDOnEvent()
    {
        ShowPanel(panelNames.InGameHUD);
    }
    
}
