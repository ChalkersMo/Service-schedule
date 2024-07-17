using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PersonSettings : MonoBehaviourPunCallbacks
{
    public bool IsRegistered;
    public bool IsInRoom;
    public string RoomName;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    private int BoolToInt(bool _bool)
    {
        if(_bool)
            return 1;
        else 
            return 0;
    }

    private bool IntToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }

    public void EnterTheSystem()
    {
        if (!PhotonNetwork.IsConnected)
        {
            if (IsRegistered)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            if (IsRegistered)
            {
                if (IsInRoom)
                    PhotonNetwork.JoinRoom(RoomName);

                else
                    SceneManager.LoadScene(2);
            }
            else
            {
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene(1);
            }
        }
        
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if(IsInRoom)
            PhotonNetwork.JoinRoom(RoomName);
          
        else
            SceneManager.LoadScene(2);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(3);
    }

    public void SaveData(bool _isRegistered, bool _isInRoom, string _roomName)
    {
        IsRegistered = _isRegistered;
        IsInRoom = _isInRoom;
        RoomName = _roomName;

        PlayerPrefs.SetString("RoomName", RoomName);
        PlayerPrefs.SetInt("IsRegistered", BoolToInt(IsRegistered));
        PlayerPrefs.SetInt("IsInRoom", BoolToInt(IsInRoom));
    }
    public void LoadData()
    {
        RoomName = PlayerPrefs.GetString("RoomName");
        IsRegistered = IntToBool(PlayerPrefs.GetInt("IsRegistered"));
        IsInRoom = IntToBool(PlayerPrefs.GetInt("IsInRoom"));
    }
}
