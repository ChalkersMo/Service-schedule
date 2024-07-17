using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersonSettings : MonoBehaviour
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
        if (PlayerPrefs.GetInt("IsRegistered") != 0 && PlayerPrefs.GetInt("IsInRoom") != 0)
            SceneManager.LoadScene(3);

        else if(PlayerPrefs.GetInt("IsRegistered") != 0 && PlayerPrefs.GetInt("IsInRoom") != 1)
            SceneManager.LoadScene(2);  
        
        else
            SceneManager.LoadScene(1);
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
