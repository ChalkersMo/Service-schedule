using UnityEngine;

public class RoomSaving : MonoBehaviour
{
    private GameObject _room;
    private string _roomName;

    public GameObject Room
    {
        get => _room;
        set => _room = value;
    }

    public string RoomName
    {
        get => _roomName;
        set => _roomName = value;
    }
}
