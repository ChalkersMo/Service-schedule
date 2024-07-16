using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public RoomItem roomItemObj;

    [SerializeField] private TMP_InputField createRoomInput;
    [SerializeField] private TMP_InputField joinRoomInput;

    [SerializeField] private GameObject contentObj;

    private TMP_InputField PasswordInputField;

    private List<RoomItem> roomItems = new List<RoomItem>();

    private LobbyUiManager lobbyUiManager;

    private TMP_InputField passwordInput;

    private RoomItem tempRoomItem;

    void Start()
    {
        lobbyUiManager = GetComponent<LobbyUiManager>();
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreateRoom()
    {
        if (createRoomInput.text.Length >= 1)
        {
            PasswordInputField = lobbyUiManager.ShowCreatePasswordPanel();    
        }
    }

    public void AddRoom()
    {
        if(PasswordInputField.text.Length > 1)
        {
            roomItems.Add(roomItemObj);
            Invoke(nameof(CreateRoom), 1);
        }     
    }

    private void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoomInput.text);
    }
    public void JoinRoomByName()
    {
        foreach (RoomItem item in roomItems)
        {
            if(joinRoomInput.text == item.roomName.text)
            {
                OnClickToJoinRoom(item);
                return;
            }
        }
    }

    public void OnClickToJoinRoom(RoomItem room)
    {
        PasswordInputField = lobbyUiManager.ShowPasswordPanel();
        tempRoomItem = room;
    }
    public void JoinRoomPasswordEntering()
    {
        if(tempRoomItem.password == PasswordInputField.text)
        {
            PhotonNetwork.JoinRoom(tempRoomItem.roomName.text);
        }
        else
        {
            lobbyUiManager.WrongPasswordEntered();
        }
    }

    public override void OnJoinedRoom()
    {
        if (TryGetComponent(out GoToScene goToScene))
            goToScene.LoadScene();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomItem item in roomItems) 
        { 
            Destroy(item.gameObject);
        }
        roomItems.Clear();

        foreach(RoomInfo roomItem in roomList)
        {
            RoomItem newRoom =  Instantiate(roomItemObj, contentObj.transform);
            newRoom.SetRoomName(roomItem.Name);
            newRoom.SetRoomPassword(passwordInput.text);
            roomItems.Add(newRoom);
        }
    }
}
