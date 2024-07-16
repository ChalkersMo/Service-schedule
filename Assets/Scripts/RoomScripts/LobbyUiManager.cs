using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LobbyManager))]
public class LobbyUiManager : MonoBehaviour
{
    [SerializeField] private GameObject CreatePasswordPanel;
    [SerializeField] private GameObject EnterPasswordPanel;

    [SerializeField] private GameObject WrongPassword;

    public TMP_InputField ShowCreatePasswordPanel()
    {
        CreatePasswordPanel.GetComponent<Image>().DOFade(0, 0);
        CreatePasswordPanel.SetActive(true);
        CreatePasswordPanel.GetComponent<Image>().DOFade(1, 1);

        return CreatePasswordPanel.GetComponentInChildren<TMP_InputField>();
    }
    public TMP_InputField ShowPasswordPanel()
    {
        EnterPasswordPanel.GetComponent<Image>().DOFade(0, 0);
        EnterPasswordPanel.SetActive(true);
        EnterPasswordPanel.GetComponent<Image>().DOFade(1, 1);

        return CreatePasswordPanel.GetComponentInChildren<TMP_InputField>();
    }

    public void WrongPasswordEntered()
    {
        WrongPassword.GetComponent<Image>().DOFade(0, 0);
        WrongPassword.SetActive(true);
        WrongPassword.GetComponent<Image>().DOFade(1, 1);
    }

    public void HideCreatePasswordPanel()
    {
        CreatePasswordPanel.SetActive(false);
    }
    public void HidePasswordPanel()
    {
        EnterPasswordPanel.SetActive(false);
    }
}
