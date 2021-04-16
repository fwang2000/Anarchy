using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomMenu : MonoBehaviourPunCallbacks
{
    private const int MaxPlayersPerRoom = 3; // reset to 8 laters

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " Entered");

        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            if (PhotonNetwork.IsMasterClient)
            {
                LoadGame();
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left");
    }

    private void LoadGame()
    {
        Debug.Log("Game is Ready To Begin");

        PhotonNetwork.LoadLevel("MainGame");
    }
}
