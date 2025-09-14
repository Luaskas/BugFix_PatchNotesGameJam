using Controller;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilitiesGeneral", menuName = "Scriptable Objects/AbilitiesGeneral")]
public abstract class AbilitiesGeneral : ScriptableObject
{
    public string abilitieName;
    public string description;
    public Sprite icon;
}
