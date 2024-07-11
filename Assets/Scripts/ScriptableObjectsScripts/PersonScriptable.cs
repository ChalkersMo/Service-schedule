using UnityEngine;

[CreateAssetMenu(fileName = "NewPerson", menuName = "Peron")]
public class PersonScriptable : ScriptableObject
{
    public string Name;
    public Texture2D Photo;
    public string Type;
}
