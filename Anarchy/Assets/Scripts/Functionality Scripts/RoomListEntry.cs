using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class RoomListEntry : MonoBehaviour
{
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI RoomIDText;
    public TextMeshProUGUI RoomPlayersText;
    public Button JoinRoomButton;

    private string roomName;

    public void Start()
    {
        JoinRoomButton.onClick.AddListener(() =>
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

            PhotonNetwork.JoinRoom(roomName);
        });
    }

    public void Initialize(string ownerName, string id, byte currentPlayers, byte maxPlayers)
    {
        roomName = id;

        RoomNameText.text = ownerName + "'s room";
        RoomIDText.text = id.Substring(5);
        RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
        JoinRoomButton.GetComponentInChildren<TextMeshProUGUI>().text = "JOIN";
    }
}
