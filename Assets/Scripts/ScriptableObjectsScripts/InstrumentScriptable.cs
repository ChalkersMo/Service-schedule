using UnityEngine;

[CreateAssetMenu(fileName = "NewInstrument", menuName = "Instrument")]
public class InstrumentScriptable : ScriptableObject
{
    public string Name;
    public Texture2D Photo;
    public string Type;
}
