using System.IO;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PersonCreation : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Dropdown dropdown;

    private Texture2D texture2D;

    public void AssignPhoto()
    {
        texture2D = PickImage(100);
    }
    public void SaveData()
    {
        //Create person object
        PersonScriptable personData = (PersonScriptable)ScriptableObject.CreateInstance("PersonScriptable");
        //Set person name
        if(nameInputField.text.Length > 1)
        {
            personData.name = nameInputField.text;
            PhotonNetwork.NickName = nameInputField.text;
        }
        
        //Set person player/singer type
        int optionId = dropdown.value;
        personData.Type = dropdown.options[optionId].text;
        //Set PersonPhoto texture
        personData.Photo = texture2D;
 
        //Saving to json
        string json = JsonUtility.ToJson(personData);
        File.WriteAllText(Application.persistentDataPath + "/PersonData.json", json);

        //Connect to master
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        //Loading next scene
        if (TryGetComponent(out GoToScene goToScene))
            goToScene.LoadScene();
    }

    private Texture2D PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

              /*/  Material material = GetComponent<Renderer>().material;
                if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");/*/

               // material.mainTexture = texture;

                GetComponent<RawImage>().texture = texture;
                texture2D = texture;
            }
        });

        Debug.Log("Permission result: " + permission);
        return texture2D;
    }

    public void LoadData()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/PersonData.json");
        PersonScriptable personData = JsonUtility.FromJson<PersonScriptable>(json);
    }
}