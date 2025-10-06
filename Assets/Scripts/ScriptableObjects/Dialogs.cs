using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class DialogChoices
{
    public string choiceText;
    public Dialogs nextDialog;
}

[CreateAssetMenu(fileName = "Dialogs", menuName = "Scriptable Objects/Dialogs")]
public class Dialogs : ScriptableObject
{
    public string speakerName;
    public string npcText;
    public List<DialogChoices> choices;
    public bool canEndDialog;

}
