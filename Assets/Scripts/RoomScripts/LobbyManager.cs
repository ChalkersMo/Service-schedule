using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public RoomItem roomItemObj;

    public List<AssignPersonSetUpVisual> personList = new List<AssignPersonSetUpVisual>();
    public AssignPersonSetUpVisual personSetup;

    [SerializeField] TextMeshProUGUI roomName;

    [SerializeField] private TMP_InputField createRoomInput;
    [SerializeField] private TMP_InputField joinRoomInput;

    [SerializeField] private GameObject contentObj;
    [SerializeField] private Transform personContent;

    [SerializeField] private GameObject lobbyObj;
    [SerializeField] private GameObject roomObj;

    private TMP_InputField PasswordInputField;

    private List<RoomItem> roomItems = new List<RoomItem>();

    private LobbyUiManager lobbyUiManager;

    private TMP_InputField passwordInput;

    private RoomItem tempRoomItem;
    private PersonSettings personSettings;


    private float nextUpdateTime;
    private float timeBetweenUpdates = 1.5f;

    private void Start()
    {
        lobbyUiManager = GetComponent<LobbyUiManager>();
        personSettings = FindObjectOfType<PersonSettings>();
        if (personSettings.IsInRoom)
        {
            PhotonNetwork.JoinRoom(personSettings.RoomName);
        }
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
        PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions {CleanupCacheOnLeave = false});
        Debug.Log("Creating the room..");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
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
        if (!personSettings.IsInRoom)
        {
            personSettings.SaveData(true, true, PhotonNetwork.CurrentRoom.Name);

        }

        lobbyObj.SetActive(false);
        roomObj.SetActive(true);

        roomName.text = PhotonNetwork.CurrentRoom.Name;

        UpdatePlayerlist();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerlist();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerlist();
    }

    public override void OnLeftRoom()
    {
        personSettings.SaveData(true, false, null);

        lobbyObj.SetActive(true);
        roomObj.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(Time.time > nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
        
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
            RoomItem newRoom = Instantiate(roomItemObj, contentObj.transform);
            newRoom.SetRoomName(roomItem.Name);
            newRoom.SetRoomPassword(passwordInput.text);
            roomItems.Add(newRoom);
        }
    }

    private void UpdatePlayerlist()
    {
        foreach(AssignPersonSetUpVisual person in personList)
        {
            Destroy(person.gameObject);
        }
        personList.Clear();

        if (PhotonNetwork.CurrentRoom == null) 
        {
            return;
        }

        foreach(KeyValuePair<int, Player> person in PhotonNetwork.CurrentRoom.Players)
        {
            AssignPersonSetUpVisual personSetUp = Instantiate(personSetup, personContent);
            personSetUp.Assign(person.Value);
            personList.Add(personSetUp);
        }
    }
}
