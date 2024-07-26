using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PersonSetUp
{
    public string Name;
    public Texture2D Photo;
    public string Type;

    private PersonSetUp tempSetUp;

    public void SetPlayerInfo(Player player)
    {
        if (player.IsLocal)
            tempSetUp = PersonDataLoad.Instance.LoadData();

        Name = tempSetUp.Name;
        Photo = tempSetUp.Photo;
        Type = tempSetUp.Type;
    }
}