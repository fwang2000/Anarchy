using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameTMP;

    [SerializeField]
    private Button continueButton;

    private const string PlayerPrefsNameKey = "Player Name";

    private void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameTMP.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        string playerName = nameTMP.text;

        PhotonNetwork.NickName = playerName;

        Debug.Log(playerName);

        PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
    }
}
