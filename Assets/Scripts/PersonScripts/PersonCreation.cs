using System.IO;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PersonCreation : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Dropdown dropdown;

    private PhotoChanging photoChanging;

    private PersonSettings personSettings;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        photoChanging = FindObjectOfType<PhotoChanging>();
        personSettings = FindObjectOfType<PersonSettings>();
    }

    public void SaveData()
    {
        //Create person object
        PersonSetUp personData = new PersonSetUp();
        //Set person name
        if(nameInputField.text.Length > 1)
        {
            personData.Name = nameInputField.text;
            PhotonNetwork.NickName = nameInputField.text;
        }
        
        //Set person player/singer type
        int optionId = dropdown.value;
        personData.Type = dropdown.options[optionId].text;
        //Set PersonPhoto texture
        personData.Photo = photoChanging.texture2D;
 
        //Saving to json
        string json = JsonUtility.ToJson(personData);
        File.WriteAllText(Application.persistentDataPath + "/PersonData.json", json);

        //Connect to master
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    { 
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        //Loading next scene
        personSettings.SaveData(true, false, null);
        if (TryGetComponent(out GoToScene goToScene))
            goToScene.LoadScene();
    }
   
    public void LoadData()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/PersonData.json");
        PersonSetUp personData = JsonUtility.FromJson<PersonSetUp>(json);
    }
}