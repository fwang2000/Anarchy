using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject findOpponentPanel = null;

    [SerializeField]
    private GameObject waitingStatusPanel = null;

    [SerializeField]
    private TextMeshProUGUI waitingStatusText = null;

    private bool isConnecting = false;

    private const string GameVersion = "0.1"; // if we update the game, we want to ensure that players with different game versions cannot play each other
    
    private const int MaxPlayersPerRoom = 3; // reset to 8 laters
    /*
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }*/

    public void FindOpponent()
    {

        isConnecting = true;

        findOpponentPanel.SetActive(false);
        waitingStatusPanel.SetActive(true);

        waitingStatusText.text = "Searching...";

        if (PhotonNetwork.IsConnected)
        {

            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");

        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        waitingStatusPanel.SetActive(false);
        findOpponentPanel.SetActive(true);

        Debug.Log($"Disconnected due to: {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No clients are waiting for an opponent, creating a new room");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Client successfully joined a room");

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        Debug.Log(playerCount);

        if (playerCount != MaxPlayersPerRoom)
        {
            waitingStatusText.text = "Waiting for more players...";
            Debug.Log("Client is waiting for an opponent");
        }
        else
        {
            Debug.Log("Match will begin");
            waitingStatusText.text = "Opponent Found!";
            //LoadGame(); UNCOMMENT FOR 1 PLAYER MOCK TESTING
        }

        PhotonNetwork.LoadLevel("Room");
    }
    /*
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player Entered");

        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.AutomaticallySyncScene = true;

            if (PhotonNetwork.IsMasterClient)
            {
                LoadGame();
            }
        }
    }

    private void LoadGame()
    {
        waitingStatusText.text = "Enough Players";
        Debug.Log("Game is Ready To Begin");

        PhotonNetwork.LoadLevel("MainGame");
    }*/
}
