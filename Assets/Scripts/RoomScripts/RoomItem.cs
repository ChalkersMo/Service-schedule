using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    public string password;

    public TextMeshProUGUI roomName;

    private LobbyManager lobbyManager;

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void SetRoomPassword(string _roomPassword)
    {
        password = _roomPassword;
    }

    public void OnClickButton()
    {
        lobbyManager.OnClickToJoinRoom(this);
    }
}
