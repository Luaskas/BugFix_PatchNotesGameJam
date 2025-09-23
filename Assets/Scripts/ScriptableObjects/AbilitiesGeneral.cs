using Controller;
using UnityEngine;

public enum AbilitiyType
{
    Teleport,
    Shrink,
    Sprint,
    DoubleJump
}

[CreateAssetMenu(fileName = "AbilitiesGeneral", menuName = "Scriptable Objects/AbilitiesGeneral")]
public class AbilitiesGeneral : ScriptableObject
{
    public string abilitieName;
    public string description;
    public Sprite icon;
}
