using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class RoomSaving
{
    private GameObject _room;
    private string _roomName;

    [FirestoreProperty]
    public GameObject Room
    {
        get => _room;
        set => _room = value;
    }

    [FirestoreProperty]
    public string RoomName
    {
        get => _roomName;
        set => _roomName = value;
    }
}
