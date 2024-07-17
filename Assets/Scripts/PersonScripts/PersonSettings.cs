using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PersonSettings : MonoBehaviourPunCallbacks
{
    public bool IsRegistered;
    public bool IsInRoom;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private int BoolToInt(bool _bool)
    {
        if(_bool)
            return 1;
        else 
            return 0;
    }

    public void EnterTheSystem()
    {
        if (PlayerPrefs.GetInt("IsRegistered") != 0)
            PhotonNetwork.ConnectUsingSettings();

        else
            SceneManager.LoadScene(1);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if(PlayerPrefs.GetInt("IsInRoom") != 0)
            SceneManager.LoadScene(3);
        else
            SceneManager.LoadScene(2);
    }
    public void SaveData(bool _isRegistered, bool _isInRoom)
    {
        IsRegistered = _isRegistered;
        IsInRoom = _isInRoom;
       
        PlayerPrefs.SetInt("IsRegistered", BoolToInt(IsRegistered));
        PlayerPrefs.SetInt("IsInRoom", BoolToInt(IsInRoom));
    }
    public void LoadData()
    {
        PlayerPrefs.GetInt("IsRegistered");
        PlayerPrefs.GetInt("IsInRoom");
    }
}
