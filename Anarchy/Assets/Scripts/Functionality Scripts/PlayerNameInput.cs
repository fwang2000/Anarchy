﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField]
    private InputField nameInputField;

    [SerializeField]
    private Button continueButton;

    private const string PlayerPrefsNameKey = "Player Name";

    private void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        Debug.Log("A");

        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        Debug.Log("Default Name Exists");

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        Debug.Log(defaultName);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        string playerName = nameInputField.text;

        PhotonNetwork.NickName = playerName;

        PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
    }
}
