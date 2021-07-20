using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EditGameSettings : MonoBehaviour
{
    [SerializeField]
    private GameObject personalSettingsButton;

    [SerializeField]
    private GameObject gameSettingsButton;

    [SerializeField]
    private GameObject personalSettings;

    [SerializeField]
    private GameObject gameSettings;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            personalSettingsButton.SetActive(true);
            gameSettingsButton.SetActive(true);
        }
    }

    public void OnClickedCloseEditButton()
    {
        transform.gameObject.SetActive(false);
    }

    public void OnPersonalSettingsButtonClicked()
    {
        personalSettings.SetActive(true);
        gameSettings.SetActive(false);
    }

    public void OnGameSettingsButtonClicked()
    {
        personalSettings.SetActive(false);
        gameSettings.SetActive(true);
    }
}
