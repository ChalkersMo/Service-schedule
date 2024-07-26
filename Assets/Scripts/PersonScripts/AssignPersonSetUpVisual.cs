using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class AssignPersonSetUpVisual : MonoBehaviour
{
    [SerializeField] private PersonSetUp personSetUp;

    private TextMeshProUGUI NameText;
    private RawImage PhotoImage;

    public void Assign(Player player)
    {
        NameText = GetComponentInChildren<TextMeshProUGUI>();
        PhotoImage = GetComponentInChildren<RawImage>();

        personSetUp.SetPlayerInfo(player);
        NameText.text = personSetUp.Name;
        PhotoImage.texture = personSetUp.Photo;
    }
}