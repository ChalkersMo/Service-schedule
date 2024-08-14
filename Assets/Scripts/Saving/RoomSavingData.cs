using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomSavingData : MonoBehaviour
{
    public string RoomName;
    public List<string> Instruments;
    public List<PersonSetUp> personSetUps;
}
