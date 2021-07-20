using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomSettings : MonoBehaviour
{
    [SerializeField]
    private GameObject CharacterSelect;

    [SerializeField]
    private GameObject EditPanel;

    [SerializeField]
    private GameObject HelpPanel;

    [SerializeField]
    private GameObject personalSettingsButton;

    [SerializeField]
    private GameObject gameSettingsButton;

    [SerializeField]
    private GameObject StartGameButton;

    private bool open;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().enabled = true;
        open = false;
    }

    public void OnClickedSettingsButton()
    {
        if (open)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        open = !open;
    }

    public void OnClickedCharacterSelectButton()
    {
        CharacterSelect.SetActive(true);
    }

    public void OnClickedLeaveButton()
    {
        ResetCharacterSelect();
        ResetMasterClient(false);
        ResetSettings();
        PhotonNetwork.LeaveRoom();
    }
    public void OnClickedEditButton()
    {
        EditPanel.SetActive(true);

    }
    public void OnClickedHelpButton()
    {
        HelpPanel.SetActive(true);
    }

    private void ResetSettings()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        EditPanel.SetActive(false);
        HelpPanel.SetActive(false);

        open = false;
    }

    private void ResetRoomProperties()
    {
        string[] nicknames = (string[]) PhotonNetwork.CurrentRoom.CustomProperties["nicknames"];

        nicknames[PhotonNetwork.LocalPlayer.GetPlayerNumber()] = "";

        Hashtable setValue = new Hashtable();
        setValue.Add("nicknames", nicknames);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);
    }

    private void ResetCharacterSelect()
    {
        string oldCharacterName = (string)PhotonNetwork.LocalPlayer.CustomProperties["character"];
        if (oldCharacterName != null)
        {
            CharacterSelect.GetComponent<PhotonView>().RPC("ActivateButton", RpcTarget.AllBuffered, oldCharacterName);
            Hashtable resetName = new Hashtable { { "character", null } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(resetName);
        }
    }

    public void ResetMasterClient(bool isActive)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartGameButton.SetActive(isActive); 
            personalSettingsButton.SetActive(isActive);
            gameSettingsButton.SetActive(isActive);
        }
    }
}
