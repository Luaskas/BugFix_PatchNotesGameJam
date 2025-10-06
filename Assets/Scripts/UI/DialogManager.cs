using System;
using System.Collections.Generic;
using Controller;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class DialogManager : MonoBehaviour
{
    [Header("Singleton")]
    public static DialogManager Instance;
    
    [Header("References")]
    public TMP_Text npcText;
    public TMP_Text npcName;
    public Dialogs currentDialog;
    public GameObject cursor;

    [Header("ButtonManagement")]
    public Button buttonPrefab;
    public Transform buttonParent;
    
    
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
    

    // --- Is called initial by NPC-Script, when starting its "StartConversation"-Method
    // --- Creates Buttons for the possible dialog choice options for the player, by getting the length of choices
    // -- Array at the Dialog-Scriptable Objects
    // --- Is after then called every time, GetNextDialog() is called by the DialogButtonScript-Method OnClick()
    // -- hanging on every created button in ShowDialog()
    public void ShowDialog(Dialogs dialog)
    {
        DeactivateCursorScript();

        npcName.text = dialog.speakerName;
        currentDialog = dialog;
        int localButtonCounter = 0;
        npcText.text = currentDialog.npcText;
        
        foreach (var VARIABLE in currentDialog.choices)
        {
            var btn = Instantiate(buttonPrefab, buttonParent);
            btn.GetComponentInChildren<DialogButtonScript>().buttonID = localButtonCounter;
            btn.GetComponentInChildren<TMP_Text>().text = currentDialog.choices[localButtonCounter].choiceText;
            localButtonCounter++;
        }
    }
    
    // --- Gets the next dialog, by taking the given integer from OnClick() in DialogButtonScript and looking
    // -- at the next position in the dialog.choices Array, where its calling again ShowDialog() with the 
    // -- new Dialog-Scriptable Object
    public void GetNextDialog(int choice)
    {
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }
        var newDialog = currentDialog.choices[choice].nextDialog;
        ShowDialog(newDialog);
    }
    
    // --- When a button with the DialogButtonScript registers, that the currentDialog-Scriptable Object
    // -- has its canEndDialog-bool on true, it will call EndDialog() instead of GetNextDialog().
    // -- As GetNextDialog() has shown the last possible player-choice, EndDialog() can now call the event "EndDialog"
    // -- from the PlayerController-Script, which is the signal for all subscribers to start their Behaviour for
    // -- this event. DialogManager is only destroying all buttons and activates the cursor script again.
    public void EndDialog()
    {
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }
        PlayerController.Instance.EndDialog();
        ActivateCursorScript();
    }

    // --- Deactivates the Cursor script, so the player can use the mouse during the Dialog-UI
    void DeactivateCursorScript()
    {
        var cursorScript = cursor.GetComponent<CursorController>();
        cursorScript.ShowCursor();
        cursorScript.enabled = false;
    }

    // --- Activates the Cursor script again, when Dialog ends.
    void ActivateCursorScript()
    {
        var cursorScript = cursor.GetComponent<CursorController>();
        cursorScript.enabled = true;
        cursorScript.HideCursor();
    }
    
}
