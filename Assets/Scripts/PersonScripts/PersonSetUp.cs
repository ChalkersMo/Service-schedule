using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonSetUp : MonoBehaviour
{
    public string Name;
    public Texture2D Photo;
    public string Type;

    public TextMeshProUGUI NameText;
    public RawImage PhotoImage;

    public void SetPlayerInfo(Player player)
    {
        NameText.text = player.NickName;
    }
}
