using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;
using Firebase.Database;
using System.Net.WebSockets;
using System.Collections;
using UnityEditor;
using System.IO;

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

    private RoomSavingData roomSaving;

    private TMP_InputField PasswordInputField;

    private List<RoomItem> roomItems = new List<RoomItem>();

    private LobbyUiManager lobbyUiManager;

    private TMP_InputField passwordInput;

    private RoomItem tempRoomItem;
    private PersonSettings personSettings;
    private RoomController roomController;

    private DatabaseReference databaseReference;


    private float nextUpdateTime;
    private float timeBetweenUpdates = 1.5f;

    private void Start()
    {
        databaseReference = FirebaseDatabase.GetInstance("https://servise-shedule-data-default-rtdb.europe-west1.firebasedatabase.app").RootReference;
        lobbyUiManager = GetComponent<LobbyUiManager>();
        personSettings = FindObjectOfType<PersonSettings>();
        if (personSettings.IsInRoom)
        {
            ReadRoomData();
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
        SaveRoomObj();
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
            PhotonNetwork.JoinOrCreateRoom(tempRoomItem.roomName.text);
        }
        else
        {
            lobbyUiManager.WrongPasswordEntered();
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if(personSettings.IsInRoom)
            PhotonNetwork.CreateRoom(personSettings.RoomName);
    }

    public override void OnJoinedRoom()
    {
        if (!personSettings.IsInRoom)
        {
            personSettings.SaveData(true, true, PhotonNetwork.CurrentRoom.Name);
        }

        lobbyObj.SetActive(false);
   
        GameObject tempRoom = Instantiate(roomObj);
        roomName = tempRoom.GetComponent<RoomObjsFind>().NameText.GetComponent<TextMeshProUGUI>();
        personContent = tempRoom.GetComponent<RoomObjsFind>().PersonContent.transform;
        roomObj = tempRoom;

        roomName.text = PhotonNetwork.CurrentRoom.Name;
        roomSaving.RoomName = roomName.text;

        if(roomObj.TryGetComponent(out RoomSavingData _roomSaving))
            roomSaving = _roomSaving;
        else
            roomSaving = roomObj.AddComponent<RoomSavingData>();


        UpdatePlayerlist();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerlist();
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

    #region saving room
    private void SaveRoomObj()
    {    
        string json = JsonUtility.ToJson(roomSaving);

        databaseReference.Child("Room").Child(personSettings.RoomName).SetRawJsonValueAsync(json).
            ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Room was loaded to firebase!");
                }
                else
                    Debug.Log("Room wasn't loaded to firebase FAIL");
            });
    }

    private void ReadRoomData()
    {
        StartCoroutine(LoadDataEnum());
    }

    private IEnumerator LoadDataEnum()
    {
        var serverData = databaseReference.Child("Room").Child(personSettings.RoomName).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        DataSnapshot dataSnapshot = serverData.Result;
        string jsonData = dataSnapshot.GetRawJsonValue();

        if (jsonData != null)
        {
            roomSaving = JsonUtility.FromJson<RoomSavingData>(jsonData);
        }
        else
            print("No data found");

        PhotonNetwork.JoinOrCreateRoom(personSettings.RoomName);
    }
    #endregion

    private void OnApplicationQuit()
    {
        if(personSettings.IsInRoom)
            SaveRoomObj();
    }
}
