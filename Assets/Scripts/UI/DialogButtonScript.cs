using System;
using Controller;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogButtonScript :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler

{
    public int buttonID;
    private Button button;
   
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color highlightedColor = Color.green;

    private void Awake()
    {
        if(buttonText == null)
            buttonText = button.GetComponentInChildren<TMP_Text>();
        Debug.Log(buttonText.text);
        buttonText.color = defaultColor;
    }

    public void OnClick()
    {
        if (DialogManager.Instance.currentDialog.canEndDialog)
            DialogManager.Instance.EndDialog();
        else
        {
            DialogManager.Instance.GetNextDialog(buttonID);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Mouse entered at button {buttonID}.");
        buttonText.color = highlightedColor;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Mouse left at button {buttonID}.");
        buttonText.color = defaultColor;
    }
}
